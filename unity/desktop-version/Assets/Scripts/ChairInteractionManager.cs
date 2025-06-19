using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ChairInteractionManager : MonoBehaviour, IInteractable
{
    [SerializeField]
    private PlayerEngine _player;
    [SerializeField]
    private Camera _fixedCamera;
    [SerializeField]
    private GameObject _crosshair;
    [SerializeField]
    private AudioListener _atPlayer;
    [SerializeField]
    private AudioListener _atComputer;

    [SerializeField]
    private TextMeshProUGUI _inboxButton;

    [SerializeField]
    private TextMeshProUGUI _pauseButton;

    [SerializeField]
    private TextMeshProUGUI _closeButton;

    [SerializeField]
    private Noctula _noctula;

    private bool _isSitting = false;
    public string GetHint()
    {
        return "Press E to sit.";
    }

    public void React()
    {
        
        _isSitting = !_isSitting;
        _crosshair.SetActive(!_isSitting);
        
        if (_player != null) _player.gameObject.SetActive(!_isSitting);
        if (_fixedCamera != null)  _fixedCamera.enabled=_isSitting;
        ;
        if (_atPlayer != null) _atPlayer.enabled = !_isSitting;
        if (_atComputer != null) _atComputer.enabled = _isSitting;
        ;
        if (!_isSitting && EventSystem.current != null) EventSystem.current.SetSelectedGameObject(null);

        if (_isSitting)
        {
            _inboxButton.text = "Inbox";
            _pauseButton.text = "Pause";
            _closeButton.text = "Close";
            _noctula.PlayerSatDown();
        }
        else
        {
            _inboxButton.text = "Inbox [M]";
            _pauseButton.text = "Pause [P]";
            _closeButton.text = "Close [M]";
            _noctula.PlayerLeftChair();
        }

    }
  }