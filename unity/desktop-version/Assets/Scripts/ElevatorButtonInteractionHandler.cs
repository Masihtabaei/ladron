using System.Collections;
using GLTF.Schema;
using UnityEngine;

public class ElevatorButtonInteractionHandler : MonoBehaviour, IInteractable
{
    public float delay=1f;

    [SerializeField]
    private new Animator animation;

    public string GetHint()
    {
        return "Press E to open Elevator";
    }

    public void React()
    {
        StartCoroutine(OpenElevator());
        animation.Play("Scene");
        StartCoroutine(CloseElevator());
        animation.Play("Close");
    }

    private IEnumerator CloseElevator() {
        yield return new WaitForSeconds(delay);
        
    }
    private IEnumerator OpenElevator()
    {yield return new WaitForSeconds(delay);
        

    }
}
