using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Object = UnityEngine.Object;

public class GameLvlController
{
    private bool _needToInteract;
    private bool _canInteract;

    private Camera _cam;
    private Services _servises;
    private GameLvlInitializator _gameLvlInitializator;
    private GameObject _finger;

    private DateTime _lastTimeClicked;

    public AfkStatus _currentStatus;

    public SpriteRenderer WrongTail { get; private set; }
  
    public event Action<bool, AnimalId> TailClicked = delegate { };
    public event Action WrongTailActionEnded = delegate { };
    public event Action RightTailClicked = delegate { };
    public event Action<bool, AfkStatus> PlayerActivityChanged = delegate { };

    public GameLvlController(Services servises, GameLvlInitializator gameLvlInitializator)
    {
        _canInteract = true;
        _servises = servises;
        _gameLvlInitializator = gameLvlInitializator;
        _gameLvlInitializator.FixedUpdateCalled += OnFixedUpdate;
        _gameLvlInitializator.UpdateCalled += OnUpdate;
        _gameLvlInitializator.Destroyed += OnDestroy;
        _cam = Camera.main;
        _lastTimeClicked = DateTime.Now;
        WrongTail = new GameObject("WrongTail").AddComponent<SpriteRenderer>();
        WrongTail.gameObject.SetActive(false);
    }

    public bool OnTailClicked(AnimalId animalId)
    {
        _canInteract = false;
        bool rightTail = animalId == _servises.CurrentAnimal;
        TailClicked(rightTail, animalId);

        if (rightTail)
        {
            if (_finger != null)
            {
                _finger.SetActive(false);
            }

            RightTailClicked();
        }
        return rightTail;
    }

    public void OnWrongTailActionEnded()
    {
        RemoveWrongTail();
        WrongTailActionEnded();
        _canInteract = true;
    }

    public void ShowFinger(Vector3 position)
    {
        _finger = Object.Instantiate(_servises.LvlLoader.LoadFiger(), position, Quaternion.identity);
        _servises.LvlLoader.Unload();
    }

    public void SetWrongTail(AnimalId animalId, Vector3 position, string sortingLayer, int orderInLayer)
    {
        WrongTail.sprite = _servises.LvlLoader.LoadWrongTail(animalId);
        WrongTail.sortingOrder = orderInLayer;
        WrongTail.sortingLayerName = sortingLayer;
        WrongTail.transform.position = position;
        WrongTail.gameObject.SetActive(true);
    }

    private void OnUpdate()
    {
        if (!_canInteract)
        {
            _lastTimeClicked = DateTime.Now;
            return;
        }

        if (Input.GetButtonDown("Fire1"))
        {
            _needToInteract = true;
            
            if (_currentStatus != AfkStatus.Active)
                OnPlayerActivityChanged(true, AfkStatus.Active);
            return;
        }

        double afkTime = (DateTime.Now - _lastTimeClicked).TotalSeconds;

        if (afkTime > 14 && _currentStatus == AfkStatus.SevenSeconds)
        {
            OnPlayerActivityChanged(false, AfkStatus.FourteenSeconds);
            return;
        }

        if(afkTime > 7 && _currentStatus == AfkStatus.Active)
        {
            OnPlayerActivityChanged(false, AfkStatus.SevenSeconds);
        }
    }

    private void OnFixedUpdate()
    {
        if(_needToInteract)
        {
            Collider2D collider = Physics2D.OverlapPoint(_cam.ScreenToWorldPoint(Input.mousePosition));
            collider?.GetComponent<IInteractable>()?.Interact(this);
           
        }
        _needToInteract = false;
    }
    private void RemoveWrongTail()
    {
        WrongTail.sprite = null;
        WrongTail.gameObject.SetActive(false);
        _servises.LvlLoader.Unload();
    }

    private void OnPlayerActivityChanged(bool active, AfkStatus afkStatus)
    {
        _currentStatus = afkStatus;
        PlayerActivityChanged(active, afkStatus);
    }

    private void OnDestroy()
    {
        _gameLvlInitializator.FixedUpdateCalled -= OnFixedUpdate;
        _gameLvlInitializator.UpdateCalled -= OnUpdate;
        _gameLvlInitializator.Destroyed -= OnDestroy;
    }
}

