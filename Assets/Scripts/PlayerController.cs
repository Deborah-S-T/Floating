using UnityEngine;
using UnityEngine.InputSystem;

public class Example : MonoBehaviour
{
    InputAction moveAction;

    public float rotationSpeed = 10.0f;
    public float speed = 5.0f;

    private Rigidbody rb;

    private void Start()
    {
        moveAction = InputSystem.actions.FindAction("Move");
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        Vector2 moveValue = moveAction.ReadValue<Vector2>();

        float yawAmount = moveValue.x * rotationSpeed * Time.fixedDeltaTime;
        Quaternion yawRotation = Quaternion.Euler(0f, yawAmount, 0f);
        rb.MoveRotation(rb.rotation * yawRotation);

        float pitchAmount = moveValue.y * rotationSpeed * Time.fixedDeltaTime;
        Quaternion pitchRotation = Quaternion.Euler(pitchAmount, 0f, 0f);
        rb.MoveRotation(rb.rotation * pitchRotation);

    }
}
