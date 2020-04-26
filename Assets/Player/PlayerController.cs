using UnityEngine;

public class PlayerController : TopDownCharacterController
{
    public static Vector3 playerPosition;
    [HideInInspector] public Vector2 input;

    public Direction playerDirection;
    Vector2 idleBlendTreeParams;

    public Animator anim;
    private Camera cam;

    private bool isInputLocked;
    public bool IsInputLocked { get { return isInputLocked; } }

    private LayerMask ground = 1 << 9;

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        anim = GetComponentInChildren<Animator>();
        cam = Camera.main;

        playerDirection = Direction.Down;
        SetPlayerDirection(Direction.Down);
    }

    void Update()
    {
        if (!isInputLocked)
            input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        else input = Vector2.zero;

        anim.SetFloat("Horizontal", input.x);
        anim.SetFloat("Vertical", input.y);
        anim.SetFloat("Speed", input.sqrMagnitude);
        ChangeDirectionAccordingToInput();

        playerPosition = transform.position;
    }

    private void ChangeDirectionAccordingToInput()
    {
        if (input.x > 0 && input.y == 0)
        {
            if (playerDirection != Direction.Right)
                SetPlayerDirection(Direction.Right);

        }
        else if (input.x < 0 && input.y == 0)
        {
            if (playerDirection != Direction.Left)
                SetPlayerDirection(Direction.Left);

        }
        else if (input.y > 0 && input.x == 0)
        {
            if (playerDirection != Direction.Up)
                SetPlayerDirection(Direction.Up);

        }
        else if (input.y < 0 && input.x == 0)
        {
            if (playerDirection != Direction.Down)
                SetPlayerDirection(Direction.Down);
        }
        else if (input.x > 0 && input.y > 0)
        {
            if (playerDirection != Direction.UpRight)
                SetPlayerDirection(Direction.UpRight);

        }
        else if (input.x < 0 && input.y < 0)
        {
            if (playerDirection != Direction.DownLeft)
                SetPlayerDirection(Direction.DownLeft);

        }
        else if (input.x > 0 && input.y < 0)
        {
            if (playerDirection != Direction.DownRight)
                SetPlayerDirection(Direction.DownRight);

        }
        else if (input.x < 0 && input.y > 0)
        {
            if (playerDirection != Direction.UpLeft)
                SetPlayerDirection(Direction.UpLeft);
        }
    }

    private void FixedUpdate()
    {
        if (!isInputLocked)
            character.Move(input);
    }

    public void SetPlayerDirection(Direction direction)
    {
        switch (direction)
        {
            case Direction.Right:
                playerDirection = Direction.Right;
                anim.SetFloat("IdleHorizontal", 1);
                anim.SetFloat("IdleVertical", 0);
                return;
            case Direction.Left:
                playerDirection = Direction.Left;
                anim.SetFloat("IdleHorizontal", -1);
                anim.SetFloat("IdleVertical", 0);
                return;
            case Direction.Up:
                playerDirection = Direction.Up;
                anim.SetFloat("IdleHorizontal", 0);
                anim.SetFloat("IdleVertical", 1);
                return;
            case Direction.Down:
                playerDirection = Direction.Down;
                anim.SetFloat("IdleHorizontal", 0);
                anim.SetFloat("IdleVertical", -1);
                return;
            case Direction.DownLeft:
                playerDirection = Direction.DownLeft;
                anim.SetFloat("IdleHorizontal", -1);
                anim.SetFloat("IdleVertical", -1);
                return;
            case Direction.DownRight:
                playerDirection = Direction.DownRight;
                anim.SetFloat("IdleHorizontal", 1);
                anim.SetFloat("IdleVertical", -1);
                return;
            case Direction.UpLeft:
                playerDirection = Direction.Down;
                anim.SetFloat("IdleHorizontal", -1);
                anim.SetFloat("IdleVertical", 1);
                return;
            case Direction.UpRight:
                playerDirection = Direction.Down;
                anim.SetFloat("IdleHorizontal", 1);
                anim.SetFloat("IdleVertical", 1);
                return;
            default:
                playerDirection = Direction.Down;
                anim.SetFloat("IdleHorizontal", 0);
                anim.SetFloat("IdleVertical", -1);
                return;
        }
    }

    public void SetPlayerDirection(Vector3 value)
    {
        Vector2 dir = new Vector2(value.z, value.x);
        float angle = Mathf.Atan2(dir.x, dir.y) * 180 / Mathf.PI;

        if (angle >= -22.5f && angle < 22.5f)
        {
            SetPlayerDirection(Direction.Right);
        }
        else if (angle >= 22.5f && angle < 67.5f)
        {
            SetPlayerDirection(Direction.UpRight);
        }
        else if (angle >= 67.5f && angle < 112.5f)
        {
            SetPlayerDirection(Direction.Up);
        }
        else if (angle >= 112.5f && angle < 157.5f)
        {
            SetPlayerDirection(Direction.UpLeft);
        }
        else if (angle >= -157.5f && angle < -112.5f)
        {
            SetPlayerDirection(Direction.DownLeft);
        }
        else if (angle >= -112.5f && angle < -67.5f)
        {
            SetPlayerDirection(Direction.Down);
        }
        else if (angle >= -67.5f && angle < -22.5f)
        {
            SetPlayerDirection(Direction.DownRight);
        }
        else if (angle >= 157.5f || angle < -157.5f)
        {
            SetPlayerDirection(Direction.Left);
        }
    }

    public void LockInput(bool value)
    {
        isInputLocked = value;
    }

    // Returns vector between click point and player
    public Vector2 GetMouseClickScreenDirection()
    {
        Vector3 playerPos = cam.WorldToScreenPoint(transform.position);
        Vector2 temp = Input.mousePosition - playerPos;
        return temp;
    }

    public Vector2 GetVectorFromPlayerToClickPoint(out float distance)
    {
        RaycastHit hit;
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        if (!Physics.Raycast(ray, out hit))
        {
            distance = 0;
            return Vector2.zero;
        }

        Vector3 temp = hit.point - transform.position;
        distance = Vector3.Distance(hit.point, transform.position);
        return new Vector2(temp.x, temp.z);
    }
}
