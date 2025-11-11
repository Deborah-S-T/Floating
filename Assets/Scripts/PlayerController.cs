using UnityEngine;
using IS = UnityEngine.InputSystem;


public class PlayerController : MonoBehaviour
{

    public float rotationSpeed = 10.0f;
    public float speed = 5.0f;

    private Rigidbody rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError(Equals(gameObject.name, " Rigidbody component not found!"));
            enabled = false;
            return;
        }
    }

    // Fixed Update is called at a fixed interval
    void FixedUpdate()
    {
        float angle = rotationSpeed * Time.fixedDeltaTime;
        Quaternion turn;

        if (IS.Keyboard.current == null) // Input System not ready
            return;

        if (IS.Keyboard.current.dKey.isPressed)
        {
            turn = Quaternion.Euler(0f, angle, 0f);
            rb.MoveRotation(rb.rotation * turn);
        }

        if (IS.Keyboard.current.qKey.isPressed)
        {
            turn = Quaternion.Euler(0f, -angle, 0f); // doesn't work -- learn to use input system properly
            rb.MoveRotation(rb.rotation * turn);
        }
    }
}
