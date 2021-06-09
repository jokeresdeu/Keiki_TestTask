using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class GameLvlInitializator : LvlInitializator
{
    public readonly Vector2[] offsets =
    {
            new Vector3(0.3f, 0.25f),
            new Vector2(0.65f, 0.5f),
            new Vector3(0.3f, 0.75f),
    };
    public readonly Vector2 AnimalOffset = new Vector2(0, -3);
    public readonly int TailsPerPanel = 3;

    [SerializeField] private SpriteRenderer _leftPanel;
    [SerializeField] private SpriteRenderer _rightPanel;
    
    public SpriteRenderer LeftPanel => _leftPanel;
    public SpriteRenderer RightPanel => _rightPanel;

    public event Action UpdateCalled = delegate { };
    public event Action FixedUpdateCalled = delegate { };
    public event Action Destroyed = delegate { };

    private void Start()
    {
        if (Services == null)
        {
            SceneManager.LoadScene(Constants.MainMenu);
            return;
        }

        ResizePanels();
        Services.OnGameLvlLoaded(this);
    }
   
    private void Update()
    {
        UpdateCalled();
    }

    private void FixedUpdate()
    {
        FixedUpdateCalled();
    }

    private void OnDestroy()
    {
        Destroyed();
    }

    private void ResizePanels()
    {
        float height = Camera.main.orthographicSize * 2;
        float width = height / Screen.height * Screen.width;
        float spriteHeigth = _leftPanel.bounds.size.y;
        float neededHeigh = height / spriteHeigth;

        _leftPanel.transform.position = new Vector3(-width / 2, height / 2, 0);
        _rightPanel.transform.position = new Vector3(width / 2, height / 2, 0);
        _leftPanel.transform.localScale = new Vector3(neededHeigh, neededHeigh, 0);
        _rightPanel.transform.localScale = new Vector3(neededHeigh, neededHeigh, 0);
    }
}
