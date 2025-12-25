using System.Security.Cryptography;
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

    private static float minipropulsionForce = 50.0f;
    private static float properPropulsionForce = 500.0f;
    private float availablePropulsions = 4 * properPropulsionForce;
    private bool propulsed = false;

    Vector2 moveValue;

    // rigidbody
    private Rigidbody rb;

    // -------------------------START----------------------------------
    private void Start()
    {
        // get input actions
        moveAction = InputSystem.actions.FindAction("Move");
        propulsionAction = InputSystem.actions.FindAction("Jump");

        rb = GetComponent<Rigidbody>();
    }

    // --------------------------UPDATE---------------------------------
    void Update()
    {
        // input handling in update
        moveValue = moveAction.ReadValue<Vector2>();

        if (propulsionAction.triggered)
        {
            propulsed = true;
        }
    }

    // --------------------------FIXED UPDATE---------------------------
    void FixedUpdate()
    {
        // physics handling in fixed update
        MovePlayer();
    }

    // ------------------------BOOSTER DETECTION-----------------------
    void OnTriggerEnter(Collider other)
    {
        Transform parent = other.transform.transform.parent;
        if (parent.CompareTag("Booster"))
        {
            availablePropulsions += 2 * properPropulsionForce;
            Destroy(other.gameObject);
        }
    }

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
        if (propulsed)
        {
            if (availablePropulsions >= 0)
            {
                Propulsion(properPropulsionForce, transform.up);
                availablePropulsions -= properPropulsionForce;
            }
            else
            {
                Propulsion(minipropulsionForce, transform.up);
            }
            propulsed = false;
        }
    }

    // -----------------------------PROPULSION---------------------------
    void Propulsion(float propForce, Vector3 direction)
    {
        // add velocity towards the wanted direction
        rb.AddForce(direction * propForce, ForceMode.Acceleration);
    }

}
