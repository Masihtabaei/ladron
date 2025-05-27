using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private PlayerInput playerInput;
    private PlayerInput.WalkingActions walking;
    private PlayerEngine player;
    void Awake()
    {
        playerInput = new PlayerInput();
        walking = playerInput.Walking;
        player = GetComponent<PlayerEngine>();
    }

    void FixedUpdate()
    {
        player.Move(walking.Movement.ReadValue<Vector2>());
    }

    private void LateUpdate()
    {
        player.Look(walking.Visual.ReadValue<Vector2>());
    }

    private void OnEnable()
    {
        walking.Enable();
    }

    private void OnDisable()
    {
        walking.Disable();
    }
}
