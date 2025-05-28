using UnityEngine;

public class DoorInteractionHandler : MonoBehaviour, IInteractable
{
    private bool isOpen;
    public GameObject door;
    public string GetHint()
    {
        return "Open the door by pressing E";
    }

    public void React()
    {
        isOpen = !isOpen;
        door.GetComponent<Animator>().SetBool("IsOpen", isOpen);
    }
}
