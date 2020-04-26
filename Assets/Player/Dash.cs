using System.Collections;
using UnityEngine;

public class Dash : MonoBehaviour
{
    private PlayerController_Jack playerController;
    private Rigidbody rb;

    [Tooltip("Distance in units, divided by 20, that the dash moves per step")]
    public float dashSpeed = 50f;
    [Tooltip("Maximum distance in units for dashing")]
    public float dashMaxDist = 2f;
    [Tooltip("For visibility purposes, automatically calculated value.")]
    public float dashMaxDuration = 0.1f;
    [Tooltip("For visibility purposes, automatically calculated value.")]
    public float dashDistance = 2f;
    private Vector3 dashDirection;

    [Range(0, 3)]
    public float DashCooldown = 0;
    public float dashCooldown = 1f;

    [Range(0.01f, 2)][Tooltip("Player won't be able to move during the stuck time")]
    public float pastDashStuckDuration = 0.1f;

    public bool shouldDash;
    private float timeElapsedSinceLastDash;
    private float dashTime, dashAngle;
    Vector3 dashVector;

    public Transform laserPointer;
    
    protected CapsuleCollider capsuleCollider;
    Ray ray = new Ray();
    RaycastHit rhit;


    private void Awake()
    {
        capsuleCollider = GetComponent<CapsuleCollider>();
    }

    private void Start()
    {
        playerController = GetComponent<PlayerController_Jack>();
        rb = GetComponent<Rigidbody>();
        
        dashCooldown = dashMaxDuration;
        dashTime = dashMaxDuration;
    }

    // Update is called once per frame
    private void Update()
    {
        if (timeElapsedSinceLastDash > dashCooldown && !shouldDash)
        {
            if (Input.GetMouseButton(1) && !shouldDash)
            {
                if (!laserPointer.gameObject.activeInHierarchy)
                    laserPointer.gameObject.SetActive(true);

                dashVector = Vector3.zero;

                // Raycasts from camera to world to get where player is pointing at
                Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
                Physics.Raycast(camRay, out rhit, 200, LayerMask.GetMask("Floor"));
                if (rhit.collider)
                {
                    dashVector = rhit.point - transform.position;
                    Debug.DrawLine(Camera.main.transform.position, rhit.point, Color.yellow, 0.2f);
                }

                // Converts camera raycast into a vector from player to raycast
                dashDirection = new Vector3(dashVector.x, 0, dashVector.z).normalized;

                // Cap dashLength to maximum distance.
                dashDistance = dashVector.magnitude;
                if (dashDistance > dashMaxDist)
                    dashDistance = dashMaxDist;

                // Set arrow/laser pointer's size and angles.
                laserPointer.localScale = new Vector3(dashDistance / 4, dashDistance /2, 1);
                laserPointer.eulerAngles = new Vector3(90, -Vector3.SignedAngle(dashDirection, transform.forward, transform.up), 0);

                // Turns pointer red if it's too close to a wall. Turns yellow if the dash will be interrupted, red if it's impossible
                ray.origin = transform.position;
                ray.direction = dashDirection;
                Physics.Raycast(ray, out rhit, dashDistance /(dashSpeed/20) + 0.1f, LayerMask.GetMask("Wall"));
                if (rhit.collider)
                    laserPointer.GetComponent<Renderer>().material.color = Color.red;
                else
                {
                    Physics.Raycast(ray, out rhit, dashDistance, LayerMask.GetMask("Wall"));
                    if (rhit.collider)
                        laserPointer.GetComponent<Renderer>().material.color = Color.yellow;
                    else
                        laserPointer.GetComponent<Renderer>().material.color = Color.white;
                }


            }
            else if (Input.GetMouseButtonUp(1) && dashVector != Vector3.zero)
            {
                laserPointer.gameObject.SetActive(false);
                timeElapsedSinceLastDash = 0;
                shouldDash = true;

                // Stores direction (used in PerfromDash())
                dashAngle = Vector3.Angle(dashDirection, transform.forward);

                // Taking "dashDistance" and "dashSpeed", calculates how far it can go.
                // "dashSpeed" is divided by 20 (Time.deltaTime)
                dashMaxDuration = dashDistance / (dashSpeed / 20);
                dashTime = dashMaxDuration;
                playerController.LockInput(true);
                playerController.AnimatePlayerMovement(dashAngle, new Vector2(dashDirection.x, dashDirection.z));

            }
        }

        if (shouldDash)
        {
            PerformDash();
        }
        timeElapsedSinceLastDash += Time.deltaTime;
    }

    private void PerformDash()
    {
        // Adds extra collision detection, so the player doesn't clip through walls, by raycasting ahead.
        ray.origin = transform.position;
        Physics.Raycast(ray, out rhit, dashDistance / (dashSpeed / 20) + 0.1f, LayerMask.GetMask("Wall"));
        if (rhit.collider) dashTime = -1;

        // Dashes as long as it can
        if (dashTime > 0)
        {
            dashTime -= 1 * Time.deltaTime * 20;

            rb.MovePosition(transform.position + (dashDirection * dashSpeed * Time.deltaTime));
        }
        else
        {
            StartCoroutine(UnlockPlayerControls());
            shouldDash = false;
        }
    }

    private IEnumerator UnlockPlayerControls()
    {
        yield return new WaitForSeconds(pastDashStuckDuration);
        playerController.LockInput(false);
    }

    public void AddToDashTime(float time)
    {
        dashTime += time;
    }
}