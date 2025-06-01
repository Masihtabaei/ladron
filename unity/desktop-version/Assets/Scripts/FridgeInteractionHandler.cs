using UnityEngine;

public class FridgeInteractionHandler : MonoBehaviour, IInteractable
{
    
    private bool isFridgeOpen;
    public GameObject fridge_door;
    public string GetHint()
    {
        return "Open/close the fridge by pressing E.";
    }

    public void React()
    {
        isFridgeOpen = !isFridgeOpen;
        fridge_door.GetComponent<Animator>().SetBool("isFridgeOpen", isFridgeOpen);
    }


}
