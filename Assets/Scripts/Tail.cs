using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tail : MonoBehaviour, IInteractable
{
    private const string _playerAfk = "PlayerAfk";
    private const string _wrongAnswer = "WrongAnswer";

    [SerializeField] private AnimalId _animalId;
    [SerializeField] private Transform _fingerPos;

    private bool isCorrect;
    private Animator _animator;
    private GameLvlController _gameLvlController;

    public void Init(GameLvlController gameLvlController, AnimalId correctId)
    {
        isCorrect = _animalId == correctId;
        _animator = GetComponent<Animator>();
        _gameLvlController = gameLvlController;
        _gameLvlController.PlayerActivityChanged += OnPlayerActivityChanged;
        _gameLvlController.RightTailClicked += OnRightTailClicked;
    }

    public void Interact(GameLvlController lvlController)
    {
        if (!lvlController.OnTailClicked(_animalId))
        {
            WrongTailClicked();
        }
    }

    private void OnTaiActionEnded()
    {
        _animator.SetBool(_wrongAnswer, false);
        _gameLvlController.WrongTailActionEnded -= OnTaiActionEnded;
    }

    private void WrongTailClicked()
    {
        _animator.SetBool(_wrongAnswer, true);
        _gameLvlController.WrongTailActionEnded += OnTaiActionEnded;
    }    

    private void OnPlayerActivityChanged(bool state, AfkStatus afkStatus)
    {
        if(afkStatus == AfkStatus.SevenSeconds)
        {
            _animator.SetBool(_playerAfk, !state);
        }
           
        if (afkStatus == AfkStatus.FourteenSeconds && isCorrect)
        {
            _gameLvlController.ShowFinger(_fingerPos.position);
        }
    }

    private void OnRightTailClicked()
    {
        gameObject.SetActive(false);
        _animator.SetBool(_playerAfk, false);
        _animator.SetBool(_wrongAnswer, false);
        _gameLvlController.WrongTailActionEnded -= OnTaiActionEnded;
        _gameLvlController.PlayerActivityChanged -= OnPlayerActivityChanged;
        _gameLvlController.RightTailClicked -= OnRightTailClicked;
    }
}
