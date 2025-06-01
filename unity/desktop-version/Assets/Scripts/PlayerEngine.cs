using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class PlayerEngine : MonoBehaviour
{
    [SerializeField]
    private float _gravity = -9.8f;
    [SerializeField]
    private float _speed = 5f;
    [SerializeField]
    private float _range = 3f;
    [SerializeField]
    private LayerMask _mask;
    [SerializeField]
    private Camera _eyes;
    [SerializeField]
    private float _xRotation = 0.0f;
    [SerializeField]
    private float _xSensitivity = 30.0f;
    [SerializeField]
    private float _ySensitivity = 30.0f;
    [SerializeField]
    private InteractionUserInterfaceManager _interactionUserInterfaceManager;

    private InputManager _inputManager;
    private CharacterController _controller;
    private Vector3 _velocity;
    private bool _isGrounded;

    void Start()
    {
        _controller = GetComponent<CharacterController>();
        _inputManager = GetComponent<InputManager>();
    }

    void Update()
    {
        _isGrounded = _controller.isGrounded;
        _interactionUserInterfaceManager.UpdateHint(string.Empty);
        Ray ray = new Ray(_eyes.transform.position, _eyes.transform.forward);
        Debug.DrawRay(ray.origin, ray.direction * _range);
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo, _range, _mask))
        {
            var interactable = hitInfo.collider.GetComponent<IInteractable>();
            if (interactable != null)
            {
                _interactionUserInterfaceManager.UpdateHint(interactable.GetHint());
                if (_inputManager.walking.Interaction.triggered) 
                {
                    interactable.React();
                }
            }
        }
    }

    public void Move(Vector2 input)
    {
        Vector3 direction = Vector3.zero;
        direction.x = input.x;
        direction.z = input.y;

        _controller.Move(transform.TransformDirection(direction) * _speed * Time.deltaTime);


        _velocity.y = _gravity * Time.deltaTime;
        if (_isGrounded && _velocity.y < 0)
        {
            _velocity.y = -2.0f;
        }
        _controller.Move(_velocity * Time.deltaTime);
    }

    public void Look(Vector2 input) 
    {
        float mouseX = input.x;
        float mouseY = input.y;

        _xRotation -= (mouseY * Time.deltaTime) * _ySensitivity;
        _xRotation = Mathf.Clamp(_xRotation, -80f, 80f);
        _eyes.transform.localRotation = Quaternion.Euler(_xRotation, 0, 0);
        transform.Rotate(Vector3.up * (mouseX * Time.deltaTime) * _xSensitivity);
    }
}
