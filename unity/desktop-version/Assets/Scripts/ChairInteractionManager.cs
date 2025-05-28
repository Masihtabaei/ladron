using UnityEngine;

public class ChairInteractionManager : MonoBehaviour, IInteractable
{
    public Camera mainCamera;
    public Camera fixedCamera;
    public string GetHint()
    {
        return "Press E to sit.";
    }

    public void React()
    {
        if (fixedCamera != null) fixedCamera.enabled = true;
        if (mainCamera != null) mainCamera.enabled = false;
    }
}
