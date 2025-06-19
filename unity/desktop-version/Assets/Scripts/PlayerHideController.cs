using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerHideController : MonoBehaviour, IInteractable
{
    [SerializeField]
    private Transform _hiddenCameraTransform;

    [SerializeField]
    private PlayerEngine _player;

    [SerializeField]
    private InteractionUserInterfaceManager _ui;

    void Start()
    {
    }

    public string GetHint()
    {
        return "Press E to hide!";
    }

    public void React()
    {
        _player.HideCamera(_hiddenCameraTransform);
    }

}
