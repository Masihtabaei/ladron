using UnityEngine;

public class HidingHandler : MonoBehaviour, IInteractable
{
    public Camera _mainCamera;
    public Camera _hiddenCamera;

    public bool _isHidden;

    [SerializeField]
    private GameObject _crosshair;

    public string GetHint()
    {
        return _isHidden ? "Press E to get out" : "Press E to hide";
    }

    public void React()
    {
        _isHidden = !_isHidden;
        _crosshair.SetActive(!_isHidden);
        if (_hiddenCamera != null) _hiddenCamera.enabled = _isHidden;
        if (_mainCamera != null) _mainCamera.enabled = !_isHidden;
    }
}