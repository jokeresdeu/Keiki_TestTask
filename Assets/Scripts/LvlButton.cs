using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LvlButton : MonoBehaviour
{
    [SerializeField] private AnimalId _animalId;
    private Button _button;
    private LvlLoader _lvlLoader;

    public void Init(LvlLoader lvlLoader)
    {
        _button = GetComponent<Button>();
        _lvlLoader = lvlLoader;
        _button.onClick.AddListener(OnButtonClicked);
    }

    private void OnButtonClicked()
    {
        _lvlLoader.LoadLvl(_animalId);
    }

    private void OnDestroy()
    {
        _button.onClick.RemoveAllListeners();
    }
}


