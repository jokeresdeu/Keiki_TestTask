using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuInitializator : LvlInitializator
{
    protected void Start()
    {
        ServiceLocator.Init();
        ServiceLocator.Services.OnMainMenuLvlLoaded(this);
    }
}
