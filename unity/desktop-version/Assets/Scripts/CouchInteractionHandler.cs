using System.Diagnostics;
using UnityEngine;

public class CouchInteractionHandler : MonoBehaviour, IInteractable
{
    public Camera mainCamera;
    public Camera fixedCamera;

    public bool isUnderCouch = false;

    [SerializeField]
    private GameObject _crosshair;

    public string GetHint()
    {
        return "Press E to hide under the couch or to get out";
    }

    public void React()
    {

        if (!isUnderCouch)
        {
            _crosshair.SetActive(false);
            if (fixedCamera != null) fixedCamera.enabled = true;
            if (mainCamera != null) mainCamera.enabled = false;
        }
        else
        {
            _crosshair.SetActive(true);
            if (fixedCamera != null) fixedCamera.enabled = false;
            if (mainCamera != null) mainCamera.enabled = true;
        }

        isUnderCouch = !isUnderCouch;
    }
}
