using UnityEngine;
using UnityEngine.InputSystem;

public class Example : MonoBehaviour
{
    // inputs
    InputAction moveAction;
    InputAction propulsionAction;

    // movement variables
    public float rotationSpeed = 10.0f;
    public float speed = 5.0f;
    public float minipropulsionForce = 5.0f;

    public float properPropulsionForce = 10.0f;

    Vector2 moveValue;

    // rigidbody
    private Rigidbody rb;

    // -------------------------START----------------------------------
    private void Start()
    {
        moveAction = InputSystem.actions.FindAction("Move");
        propulsionAction = InputSystem.actions.FindAction("Jump");
        rb = GetComponent<Rigidbody>();
    }

    // --------------------------UPDATE---------------------------------
    void Update()
    {
        // input handling in update
        moveValue = moveAction.ReadValue<Vector2>();
    }

    // --------------------------FIXED UPDATE---------------------------
    void FixedUpdate()
    {
        // physics handling in fixed update
        MovePlayer();
    }

    // ------------------------COLLISION DETECTION-----------------------
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            // if collided with a wall, propel the player away from it
            Vector3 wallNormal = collision.contacts[0].normal;
            Propulsion(properPropulsionForce, wallNormal);
        }
    }
    /*
    // ------------------------TRIGGER DETECTION-------------------------
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Wall"))
        {
            Propulsion(properPropulsionForce, );
        }
    }*/

    // ----------------------------------------------------------------------------------------------------

    // -----------------------------MOVE PLAYER-------------------------
    void MovePlayer()
    {
        // rotate forword/backward and sidways
        float yawAmount = moveValue.x * rotationSpeed * Time.fixedDeltaTime;
        float pitchAmount = moveValue.y * rotationSpeed * Time.fixedDeltaTime;

        // apply the rotation
        Quaternion Rotation = Quaternion.Euler(pitchAmount, yawAmount, 0f);
        rb.MoveRotation(rb.rotation * Rotation);

        // apply mini propulsion upward
        if (propulsionAction.triggered)
        {
            Propulsion(minipropulsionForce, transform.up);
        }
    }

    // -----------------------------PROPULSION---------------------------
    void Propulsion(float propForce, Vector3 direction)
    {
        // add velocity towards the wanted direction
        rb.AddForce(direction * propForce, ForceMode.Acceleration);
    }

}
