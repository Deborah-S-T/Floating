using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.InputSystem;

public class Example : MonoBehaviour
{
    // inputs
    InputAction moveAction;
    InputAction propulsionAction;
    InputAction stickAction;

    // movement variables
    public float rotationSpeed = 10.0f;
    public float speed = 5.0f;

    public float minipropulsionForce = 1.0f;
    public float properPropulsionForce = 50.0f;

    Vector2 moveValue;

    // sticking
    private bool holding = false;
    Vector3 contactNormal;

    // rigidbody
    private Rigidbody rb;

    // -------------------------START----------------------------------
    private void Start()
    {
        // get input actions
        moveAction = InputSystem.actions.FindAction("Move");
        propulsionAction = InputSystem.actions.FindAction("Jump");
        stickAction = InputSystem.actions.FindAction("Interact");

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
            if (!stickAction.IsPressed())
            {
                // if collided with a wall, propel the player away from it
                Vector3 wallNormal = collision.contacts[0].normal;
                Propulsion(properPropulsionForce, wallNormal);
            }

            contactNormal = collision.contacts[0].normal;
            Vector3 contactPoint = collision.contacts[0].point;
            float normalOffset = Vector3.Dot(transform.position - contactPoint, contactNormal);
            normalOffset = Mathf.Max(normalOffset, 0.1f);
            Stick(contactNormal, contactPoint, normalOffset);
        }
    }
    /*
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Wall"))
        {
            if (!stickAction.IsPressed())
            {
                // if collided with a wall, propel the player away from it
                Vector3 wallNormal = (transform.position - other.ClosestPoint(transform.position)).normalized;
                Propulsion(properPropulsionForce, wallNormal);
            }

            contactNormal = (transform.position - other.ClosestPoint(transform.position)).normalized;
            Vector3 contactPoint = other.ClosestPoint(transform.position);
            float normalOffset = Vector3.Dot(transform.position - contactPoint, contactNormal);
            normalOffset = Mathf.Max(normalOffset, 0.1f);
            Stick(contactNormal, contactPoint, normalOffset);
        }
    }*/

    // ----------------------------------------------------------------------------------------------------

    // -----------------------------MOVE PLAYER-------------------------
    void MovePlayer()
    {
        if (!holding)
        {
            // rotate forword/backward and sidways
            float yawAmount = moveValue.x * rotationSpeed * Time.fixedDeltaTime;
            float pitchAmount = moveValue.y * rotationSpeed * Time.fixedDeltaTime;

            // apply the rotation
            Quaternion Rotation = Quaternion.Euler(pitchAmount, yawAmount, 0f);
            rb.MoveRotation(rb.rotation * Rotation);

            // apply mini propulsion upward
            if (propulsionAction.IsPressed())
            {
                Propulsion(minipropulsionForce, transform.up);
            }
        }
        else
        {
            float forward_backMovement = moveValue.y * speed * Time.fixedDeltaTime;
            float sideMovement = moveValue.x * speed * Time.fixedDeltaTime;

            // apply movement
            Vector3 moveDir = Vector3.ProjectOnPlane(transform.up, contactNormal).normalized * forward_backMovement
                + Vector3.ProjectOnPlane(transform.right, contactNormal).normalized * sideMovement;

            transform.position += moveDir;
        }
    }

    // -----------------------------PROPULSION---------------------------
    void Propulsion(float propForce, Vector3 direction)
    {
        // add velocity towards the wanted direction
        rb.AddForce(direction * propForce, ForceMode.Acceleration);
    }

    // -----------------------------STICK--------------------------------
    void Stick(Vector3 normal, Vector3 point, float offset)
    {
        if (stickAction.IsPressed())
        {
            // sticks to the surface it touches
            transform.position = point + offset;
            holding = true;
        }
        else
        {
            holding = false;
        }
        /*
        if (stickAction.IsPressed())
        {
            // stop physics from moving the player
            rb.linearVelocity = Vector3.zero;
            rb.isKinematic = true;

            // snap player slightly above the surface
            transform.position = point + normal * offset;
            holding = true;
        }
        else
        {
            // release stick
            rb.isKinematic = false;
            holding = false;
        }*/
    }
}
