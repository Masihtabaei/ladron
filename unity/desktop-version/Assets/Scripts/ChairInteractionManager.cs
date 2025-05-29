using UnityEngine;

public class ChairInteractionManager : MonoBehaviour, IInteractable
{
    public Camera mainCamera;
    public Camera fixedCamera;

    [SerializeField]
    private GameObject _crosshair;

    public string GetHint()
    {
        return "Press E to sit.";
    }

    public void React()
    {
        _crosshair.SetActive(false);
        if (fixedCamera != null) fixedCamera.enabled = true;
        if (mainCamera != null) mainCamera.enabled = false;
    }
}
