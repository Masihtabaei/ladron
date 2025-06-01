using UnityEngine;

public class DoorInteractionHandler : MonoBehaviour, IInteractable
{
    private bool isOpen;

    [SerializeField]
    private GameObject _door;

    public string GetHint()
    {
        return "Press E to open/close the door.";
    }

    public void React()
    {
        isOpen = !isOpen;
        _door.GetComponent<Animator>().SetBool("IsOpen", isOpen);
    }
}
