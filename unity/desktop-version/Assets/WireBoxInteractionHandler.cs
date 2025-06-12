using UnityEngine;

public class WireBoxInteractionHandler : MonoBehaviour, IInteractable
{
    public bool inBox = false;
    private Camera originCam;
    public Camera minigameCam;
    public PlayerEngine player;
    //public GameObject TextBg;
    //public InteractionUserInterfaceManager interactionUserInterfaceManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public string GetHint()
    {
        if (!inBox)
            return "Press E to fix the wires!";
        else
            return "Press E to return to Ladron's room";
    }
    public void React()
    {
        originCam = player._eyes.GetComponent<Camera>();
        if (!inBox)
        {
            player._speed = 0;
            inBox = true;
            originCam.enabled = false;
            minigameCam.enabled = true;
        }
        else {
            player._speed = 5;
            inBox = false;
            originCam.enabled = true;
            minigameCam.enabled = false;
        }
    }
}
