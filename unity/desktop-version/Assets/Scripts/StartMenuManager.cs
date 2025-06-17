using GLTF.Schema;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuManager : MonoBehaviour
{
    [SerializeField] 
    private GameObject _startMenu;

    [SerializeField]
    private GameObject _credits;

    public void StartGame()
    {
        SceneManager.LoadScene("Main");
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
}
