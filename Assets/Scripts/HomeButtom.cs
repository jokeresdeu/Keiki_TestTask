using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HomeButtom : MonoBehaviour
{
    private Button _button;
    private void Start()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(() => SceneManager.LoadScene(Constants.MainMenu));
    }

    private void OnDestroy()
    {
        _button.onClick.RemoveAllListeners();
    }
}
