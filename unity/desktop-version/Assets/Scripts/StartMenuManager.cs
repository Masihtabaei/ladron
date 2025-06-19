using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartMenuManager : MonoBehaviour
{
    [SerializeField] 
    private GameObject _startMenu;

    [SerializeField] 
    private GameObject _loading;

    [SerializeField]
    private GameObject _credits;


    [SerializeField]
    private Slider _loadingSlider;

    public void StartGame()
    {
        _startMenu.SetActive(false);
        _loading.SetActive(true);
        StartCoroutine(LoadGameAsync());
    }

    public void ShowCredits()
    { 
        _startMenu.SetActive(false);
        _credits.SetActive(true);
    }

    public void ExitCredits()
    {
        _credits.SetActive(false);
        _startMenu.SetActive(true);
    }

    public void Exit()
    {
        Application.Quit();
    }

    IEnumerator LoadGameAsync()
    { 
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync("Main");
        while (!loadOperation.isDone)
        {
            float progressValue = Mathf.Clamp01(loadOperation.progress / 0.9f);
            _loadingSlider.value = progressValue;
            yield return null;
        }
    }
}
