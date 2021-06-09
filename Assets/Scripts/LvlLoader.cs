using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.IO;
using Spine.Unity;
using UnityEngine.SceneManagement;
public class LvlLoader // Якби проект був більший, загрузку можна було б організувати через AssetBundle але при таких розмірах нема сенсу
{
    private readonly string _lvlsPanelPath = "LvlButtonsPanel";
    private readonly string _tailsButtonPath = "TailButtons";
    private readonly string _tailsPath = "Tails";
    private readonly string _animalsPath = "Animals";
    private readonly string _soundMap = "SoundMap";
    private readonly string _finger = "Finger";
    
    private Services _services;
    private Animal _animal;

    public LvlLoader(Services services)
    {
        _services = services;
    }

    public SoundMap LoadSounds()
    {
        return Resources.Load<SoundMap>(_soundMap);
    }

    public void LoadLvl(AnimalId animalId)
    {
        _services.CurrentAnimal = animalId;
        SceneManager.LoadScene(Constants.GameLvl);
    }

    public void LoadMainMenu(Transform mainMenu)
    {
        GameObject lvlButtonsPanel =  Object.Instantiate(Resources.Load<GameObject>(_lvlsPanelPath), mainMenu);
        LvlButton[] lvlButtons = lvlButtonsPanel.GetComponentsInChildren<LvlButton>();

        for (int i = 0; i < lvlButtons.Length; i++)
        {
            lvlButtons[i].Init(this);
        }
            
        Unload();
    }

    public void PrepareGameLvl(GameLvlInitializator initalizator)
    {
        List<Tail> tails = Resources.LoadAll<Tail>(_tailsButtonPath).ToList();

        for(int i =0; i < initalizator.offsets.Length; i++)
        {
            InstantiateRandomFromList(tails, initalizator.LeftPanel, initalizator.offsets[i]);
            InstantiateRandomFromList(tails, initalizator.RightPanel, initalizator.offsets[i]);
        }

        _animal = Object.Instantiate(Resources.Load<Animal>(Path.Combine(_animalsPath, _services.CurrentAnimal.ToString())), initalizator.AnimalOffset, Quaternion.identity);

        _animal.LvlCompleted += OnGameLvlCompleted;
        _animal.Init(_services, _services.CurrentAnimal);
        Unload();
    }

    public Sprite LoadWrongTail(AnimalId animalId)
    {
        return Resources.Load<Sprite>(Path.Combine(_tailsPath, animalId.ToString()));
    }

    public void Unload()
    {
        Resources.UnloadUnusedAssets();
    }

    public GameObject LoadFiger()
    {
        return Resources.Load<GameObject>(_finger);
    }

    private void InstantiateRandomFromList(List<Tail> list, SpriteRenderer panel, Vector2 offset)
    {
        if (list.Count == 0)
        {
            Debug.LogError("There is no tails to instantiate");
            return;
        }

        int index = Random.Range(0, list.Count - 1);
        Tail tail = Object.Instantiate(list[index], panel.transform);
        tail.transform.localPosition = new Vector2(panel.sprite.bounds.size.x * offset.x, -panel.sprite.bounds.size.y * offset.y);
        tail.Init(_services.GameLvlController, _services.CurrentAnimal);
        list.RemoveAt(index);
    }

    private void OnGameLvlCompleted()
    {
        _animal.LvlCompleted -= OnGameLvlCompleted;
        SceneManager.LoadScene(Constants.MainMenu);
    }
}



