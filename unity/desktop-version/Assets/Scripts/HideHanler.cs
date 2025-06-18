using System.Diagnostics;
using UnityEngine;

public class CouchInteractionHandler : MonoBehaviour, IInteractable
{
    public Camera _mainCamera;
    public Camera _fixedCamera;

    public bool _isUnderCouch;

    [SerializeField]
    private GameObject _crosshair;

    public string GetHint()
    {
        if (_isUnderCouch) return "Press E to get out";
        
        else return "Press E to hide under the couch";
    }

    public void React()
    {
        _isUnderCouch = !_isUnderCouch;
        _crosshair.SetActive(!_isUnderCouch);
        if (_fixedCamera != null) _fixedCamera.enabled = _isUnderCouch;
        if (_mainCamera != null) _mainCamera.enabled = !_isUnderCouch;
    }
}
