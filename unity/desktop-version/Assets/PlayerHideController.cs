using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEditor.Experimental.GraphView.GraphView;

public class PlayerHideController : MonoBehaviour, IInteractable
{
    [SerializeField]
    private Camera _hiddenCamera;

    [SerializeField]
    private GameObject _player;

    private bool _isHidden = false;


    public string GetHint()
    {
        return "Press E to hide!";
    }

    public void React()
    {
        _isHidden = !_isHidden;
        if (_hiddenCamera != null) _hiddenCamera.enabled = _isHidden;
        if (_player != null) _player.gameObject.SetActive(!_isHidden);
        if (!_isHidden && EventSystem.current != null) EventSystem.current.SetSelectedGameObject(null);

    }
}
