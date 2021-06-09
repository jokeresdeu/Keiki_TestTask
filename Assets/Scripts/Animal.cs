using Spine;
using Spine.Unity;
using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using Animation = Spine.Animation;
using AnimationState = Spine.AnimationState;

[RequireComponent(typeof(AudioSource), typeof(SkeletonAnimation))]
public class Animal : MonoBehaviour, IInteractable
{
    private readonly string _spriteLayer = "Animal";
    private readonly int _spriteOrder = 1;
    private readonly int _trakIndex = 0;

    [SerializeField] private Transform _wrongTailPosition;

    [Header("Animations")]
    [SerializeField] private AnimationReferenceAsset Idle;
    [SerializeField] private AnimationReferenceAsset Happy;
    [SerializeField] private AnimationReferenceAsset Sad;
    [SerializeField] private AnimationReferenceAsset Denial;
    [SerializeField] private AnimationReferenceAsset StandingTap;

    private Services _servises;
    private AnimationState _animationState;
    private SkeletonAnimation _skeletonAnimation;
    private Animation _previousAnimation;
    private bool _canInteract = true;

    private Attachment _tail;
    private Slot _tailSlot;
    private AnimalId _lastClickedTail;
    private AudioSource _audioSourse;

    public event Action AnimationCompleted = delegate { };
    public event Action LvlCompleted = delegate{};

    public void Init(Services servises, AnimalId currentId)
    {
        _audioSourse = GetComponent<AudioSource>();
        _skeletonAnimation = GetComponent<SkeletonAnimation>();
        _lastClickedTail = currentId;
        _servises = servises;
        _servises.GameLvlController.TailClicked += OnTailClicked;
        _animationState = _skeletonAnimation.AnimationState;

        RemoveTail();

        _skeletonAnimation.AnimationState.SetAnimation(_trakIndex, Idle, true);
        _servises.AudioManager.PlayAnimalSound(_audioSourse, currentId, SoundId.WhereIsMyTail);
    }

    public void Interact(GameLvlController lvlController)
    {
        if (_canInteract)
        {
            PlayAnimation(StandingTap, 2f);
        }
    }

    private void RemoveTail()
    {
        _tailSlot = _skeletonAnimation.Skeleton.Slots.Items.First(s => s.Data.Name.Contains("ail"));
        _tail = _tailSlot.Attachment;
        _tailSlot.Attachment = null;
    }

    private void PlayAnimation(AnimationReferenceAsset animation, float duration = 0)
    {
        _previousAnimation = _skeletonAnimation.state.GetCurrent(_trakIndex).Animation;

        if (_previousAnimation != null && _previousAnimation == animation.Animation)
        {
            return;
        }

        _skeletonAnimation.AnimationState.SetAnimation(_trakIndex, animation, true);

        if (duration != 0)
        {
            StartCoroutine(StopAnimationAfter(duration));
        }
           
        return;
    }
 
    private IEnumerator StopAnimationAfter(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        PlayAnimation(Idle);
    }

    private void OnTailClicked(bool correctTail, AnimalId animalId)
    {
        if (!_canInteract)
        {
            return;
        }
            
        _canInteract = false;

        if (correctTail)
        {
            _tailSlot.Attachment = _tail;
            PlayAnimation(Happy, 2f);
            _animationState.End += OnHappinessEnded;
            _servises.AudioManager.PlayRandomSound(SoundId.CorrectAnswer);
            return;
        }

        _servises.AudioManager.PlayRandomSound(SoundId.IncorrectAnswer);
       
        if (_lastClickedTail != animalId)
        {
            PlayAnimation(Denial, 2f);
            _lastClickedTail = animalId;
        }
        else
        {
            PlayAnimation(Sad, 2f);
        }

        _animationState.End += OnWrongTailActionEdned;
        _servises.GameLvlController.WrongTail.transform.position = _wrongTailPosition.position;
        _servises.GameLvlController.SetWrongTail(animalId, _wrongTailPosition.position, _spriteLayer, _spriteOrder - 1);
    }

    private void OnHappinessEnded(TrackEntry trackEntry)
    {
        if (_previousAnimation != Happy.Animation)
        {
            return;
        }
           
        LvlCompleted();
    }

    private void OnWrongTailActionEdned(TrackEntry trackEntry)
    {
        if (_previousAnimation != Denial.Animation && _previousAnimation != Sad.Animation)
        {
            return;
        }

        PlayAnimation(Idle);
        _canInteract = true;
        _servises.GameLvlController.OnWrongTailActionEnded();
        _animationState.End -= OnWrongTailActionEdned;
    }
}

