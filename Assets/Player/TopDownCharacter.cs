using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class TopDownCharacter : MonoBehaviour
{
    protected Rigidbody rb;

    public float movementSpeed = 1000.0f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        rb.isKinematic = false;
        rb.angularDrag = 0.0f;
        rb.useGravity = false;
    }

    public void Move(Vector2 velocityDelta)
    {
        if (Mathf.Sqrt(velocityDelta.x * velocityDelta.x + velocityDelta.y * velocityDelta.y) > 1)
            velocityDelta *= 0.7f;

        Vector3 dir = new Vector3(velocityDelta.x, 0, velocityDelta.y);
        rb.velocity = dir * movementSpeed;
    }
}
