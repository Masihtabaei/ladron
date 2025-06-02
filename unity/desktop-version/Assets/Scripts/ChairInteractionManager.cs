using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ChairInteractionManager : MonoBehaviour, IInteractable
{
    [SerializeField]
    private PlayerEngine _player;
    [SerializeField]
    private Camera _fixedCamera;
    [SerializeField]
    private GameObject _crosshair;

   

    private bool _isSitting = false;
    public string GetHint()
    {
        return "Press E to sit.";
    }

    public void React()
    {
        _isSitting = !_isSitting;
        _crosshair.SetActive(!_isSitting);
        if (_fixedCamera != null) _fixedCamera.enabled = _isSitting;
        if (_player != null) _player.gameObject.SetActive(!_isSitting);
        if (!_isSitting && EventSystem.current != null) EventSystem.current.SetSelectedGameObject(null);

        
    }
}
