using UnityEngine;
using TMPro;
using System;
using UnityEngine.SceneManagement;

public class InteractionUserInterfaceManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _hint;

    [SerializeField]
    private TextMeshProUGUI _countdownDisplay;

    [SerializeField]
    private TimeManager _timeManager;

    [SerializeField]
    private GameObject _gameOverlay;

    [SerializeField]
    private GameObject _gameOverOverlay;

    [SerializeField]
    private GameObject _pauseMenuOverlay;

    [SerializeField]
    private GameObject _inboxOverlay;

    [SerializeField]
    private GameObject _perfectStudentOverlay;

    [SerializeField]
    private GameObject _masterPrompterOverlay;

    [SerializeField]
    private GameObject _jokeOverlay;

    [SerializeField]
    private AudioSource _audioSource;


    [SerializeField]
    private Noctula _noctula;

    private bool _isPaused;
    private bool _inboxOpened;

    public void UpdateHint(string message)
    {
        _hint.text = message;
    }

    private void OnTimeUpdated(TimeSpan newValue)
    {
        int wrappedHours = (int)newValue.TotalHours % 24;
        _countdownDisplay.text = $"CLOCK\n{wrappedHours:00}:{newValue.Minutes:00}";
    }

    private void OndDeadLineApproaches()
    {
        _countdownDisplay.color = Color.red;
    }
    private void OnTimeOut()
    {
        Time.timeScale = 0;
        _gameOverlay.SetActive(false);
        _gameOverOverlay.SetActive(true);
    }

    private void Awake()
    {
        _timeManager.TimeUpdated += OnTimeUpdated;
        _timeManager.DeadLineApproaches += OndDeadLineApproaches;
        _timeManager.TimeOut += OnTimeOut;
        _noctula.GameOverReached += OnTimeOut;
        _noctula.PerfectStudentEndingReached += OnPerfectEndingReached;
        _noctula.MasterPrompterEndingReached += OnMasterPrompterEndingReached;
        _noctula.JokeEndingReached += OnJokeEndingReached;
    }

    private void OnDestroy()
    {
        _timeManager.TimeUpdated -= OnTimeUpdated;
        _timeManager.DeadLineApproaches -= OndDeadLineApproaches;
        _timeManager.TimeOut -= OnTimeOut;
        _noctula.GameOverReached -= OnTimeOut;
        _noctula.PerfectStudentEndingReached -= OnPerfectEndingReached;
        _noctula.MasterPrompterEndingReached -= OnMasterPrompterEndingReached;
        _noctula.JokeEndingReached -= OnJokeEndingReached;

    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("Start");
    }

    public void TogglePause()
    {
        AudioSource[] allSources = UnityEngine.Object.FindObjectsByType<AudioSource>(FindObjectsSortMode.None);
        _isPaused = !_isPaused;
        Time.timeScale = (_isPaused ? 0 : 1) & 1;
        if (_isPaused)
        {
            foreach (AudioSource source in allSources)
                source.Pause();
        }
        else
        {
            foreach (AudioSource source in allSources)
                source.UnPause();
        }
        _pauseMenuOverlay.SetActive(_isPaused);
        _gameOverlay.SetActive(!_isPaused);
    }

    public void OnPerfectEndingReached()
    {
        Time.timeScale = 0;
        _gameOverlay.SetActive(false);
        _perfectStudentOverlay.SetActive(true);
    }

    public void OnMasterPrompterEndingReached()
    {
        Time.timeScale = 0;
        _gameOverlay.SetActive(false);
        _masterPrompterOverlay.SetActive(true);
    }

    public void OnJokeEndingReached()
    {
        Time.timeScale = 0;
        _gameOverlay.SetActive(false);
        _jokeOverlay.SetActive(true);
    }

    public void ToggleInbox()
    {
        if (!_isPaused)
        {
            _inboxOpened = !_inboxOpened;
            _inboxOverlay.SetActive(_inboxOpened);
        }
    }

    public void Exit()
    {
        Application.Quit();
    }
}
