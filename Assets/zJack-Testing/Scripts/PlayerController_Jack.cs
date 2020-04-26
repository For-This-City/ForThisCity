using UnityEngine;
using System.Collections.Generic;

public class PlayerController_Jack : MonoBehaviour
{
    public Vector3 playerPosition;
    public Vector2 input;
    public float movementSpeed = 5;
    Dash dashScript;

    public Direction playerDirection;
    
    public Animator anim;

    private bool isInputLocked;

    Rigidbody rb;

    List<int> fpsAVG = new List<int>();
    

    private void Start()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody>();
        dashScript = GetComponent<Dash>();

        playerDirection = Direction.Down;
        SetPlayerDirection(Direction.Down, Vector2.zero);
    }

    void Update()
    {
        if (!isInputLocked)
            input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
        else input = Vector2.zero;

        if (input == Vector2.zero && !dashScript.shouldDash) // Is not moving, nor dashing
            anim.SetBool("moving", false);


        //FPSCounter();
    }
    private void FixedUpdate()
    {
        playerPosition = transform.position;
        if (input != Vector2.zero)
        {
            Vector3 dir = new Vector3(input.x, 0, input.y).normalized;

            rb.MovePosition(playerPosition+(dir * movementSpeed * Time.deltaTime));

            AnimatePlayerMovement(Vector3.Angle(dir, transform.forward), input);
        }
    }

    public void AnimatePlayerMovement(float angle, Vector2 direction)
    {
        anim.SetBool("moving", true);
        ChangeDirectionAccordingToInput(angle, direction);
    }

    public void ChangeDirectionAccordingToInput(float angle, Vector2 direction)
    {
        if (angle == 0 && direction == Vector2.zero && playerDirection != Direction.Down) // No movement
            SetPlayerDirection(Direction.Down, direction);

        if (angle < 22.5f && playerDirection != Direction.Up) // Looking up
            SetPlayerDirection(Direction.Up, direction);

        if (angle >= 22.5f || angle < 67.5f) // Diagonal up
        {
            if (direction.x <= -0.1f && playerDirection != Direction.UpLeft) // Diagonal up-left
                SetPlayerDirection(Direction.UpLeft, direction);
            else if (direction.x >= 0.1f && playerDirection != Direction.UpRight) // Diagonal up-right
                SetPlayerDirection(Direction.UpRight, direction);
        }

        if (angle >= 67.5f || angle < 112.5f) // Horizontal
        {
            if (direction.x <= -0.1f && playerDirection != Direction.Left) // Looking left
                SetPlayerDirection(Direction.Left, direction);
            else if (direction.x >= 0.1f && playerDirection != Direction.Right) // Looking right
                SetPlayerDirection(Direction.Right, direction);
        }

        if (angle >= 112.5f || angle < 157.5f) // Diagonal down
        {
            if (direction.x <= -0.1f && playerDirection != Direction.DownLeft) // Diagonal down-left
                SetPlayerDirection(Direction.DownLeft, direction);
            else if (direction.x >= 0.1f && playerDirection != Direction.DownRight) // Diagonal down-right
                SetPlayerDirection(Direction.DownRight, direction);
        }

        if (angle >= 157.5f && playerDirection != Direction.Down) // Looking down
            SetPlayerDirection(Direction.Down, direction);
    }


    public void SetPlayerDirection(Direction direction, Vector2 vector)
    {
        playerDirection = direction;

        anim.SetFloat("xmovement", vector.x);
        anim.SetFloat("ymovement", vector.y);
    }

    public void LockInput(bool value)
    {
        isInputLocked = value;
    }

    void FPSCounter()
    {

        int newFPS = (int)(1f / Time.unscaledDeltaTime);
        int average = 0;
        fpsAVG.Add(newFPS);
        if (fpsAVG.Count > 20) fpsAVG.RemoveAt(0);

        foreach (int fps in fpsAVG)
        {
            average += fps;
        }
        average /= 20;

        Debug.Log(newFPS + " | Avg: " + average);
    }
}
