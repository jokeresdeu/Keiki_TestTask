using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Services 
{
    public AnimalId CurrentAnimal { get; set; }
    public LvlLoader LvlLoader { get; private set; }
    public GameLvlController GameLvlController { get; private set; }
    public AudioManager AudioManager { get; private set; }

    public Services()
    {
        LvlLoader = new LvlLoader(this);
        AudioManager  = new AudioManager(LvlLoader.LoadSounds());
    }

    public void OnGameLvlLoaded(GameLvlInitializator initializator)
    {
       
        GameLvlController = new GameLvlController(this, initializator);
        AudioManager.CreateAudiosourseForLvl(initializator.transform);
        LvlLoader.PrepareGameLvl(initializator);
        
    }

    public void OnMainMenuLvlLoaded(MainMenuInitializator initializator)
    {
        GameLvlController = null;
        AudioManager.CreateAudiosourseForLvl(initializator.transform);
        LvlLoader.LoadMainMenu(initializator.transform);
    }
}
