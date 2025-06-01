using UnityEngine;

public class ChairInteractionManager : MonoBehaviour, IInteractable
{
    [SerializeField]
    private Camera _mainCamera;
    [SerializeField]
    private Camera _fixedCamera;
    [SerializeField]
    private GameObject _crosshair;

    public string GetHint()
    {
        return "Press E to sit.";
    }

    public void React()
    {
        _crosshair.SetActive(false);
        if (_fixedCamera != null) _fixedCamera.enabled = true;
        if (_mainCamera != null) _mainCamera.enabled = false;
    }
}
