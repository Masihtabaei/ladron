using UnityEngine;

public class FridgeInteractionHandler : MonoBehaviour, IInteractable
{

    [SerializeField]
    public GameObject _fridge_door;

    private bool _isFridgeOpen;

    public string GetHint()
    {
        if (_isFridgeOpen) return "Press E to close the fridge";
        else return "Press E to open the fridge";
    }

    public void React()
    {
        _isFridgeOpen = !_isFridgeOpen;
        _fridge_door.GetComponent<Animator>().SetBool("isFridgeOpen", _isFridgeOpen);
    }


}
