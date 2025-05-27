using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class PlayerEngine : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;
    public float gravity = -9.8f;
    public float speed = 5f;
    public float distance = 3f;
    public LayerMask mask;
    public Camera eyes;
    public float xRotation = 0.0f;
    public float xSensitivity = 30.0f;
    public float ySensitivity = 30.0f;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        isGrounded = controller.isGrounded;

        Ray ray = new Ray(eyes.transform.position, eyes.transform.forward);
        Debug.DrawRay(ray.origin, ray.direction * distance);
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo, distance, mask))
        {
            var interactable = hitInfo.collider.GetComponent<IInteractable>();
            Debug.Log(interactable.GetHint());

        }
    }

    public void Move(Vector2 input)
    {
        Vector3 direction = Vector3.zero;
        direction.x = input.x;
        direction.z = input.y;

        controller.Move(transform.TransformDirection(direction) * speed * Time.deltaTime);


        velocity.y = gravity * Time.deltaTime;
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2.0f;
        }
        controller.Move(velocity * Time.deltaTime);
    }

    public void Look(Vector2 input) 
    {
        float mouseX = input.x;
        float mouseY = input.y;

        xRotation -= (mouseY * Time.deltaTime) * ySensitivity;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);
        eyes.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
        transform.Rotate(Vector3.up * (mouseX * Time.deltaTime) * xSensitivity);
    }
}
