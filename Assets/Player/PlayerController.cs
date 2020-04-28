using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public Vector3 playerPosition;
    public Vector2 input;
    public float movementSpeed = 5;
    Dash dashScript;

    public Direction playerDirection;
    
    public Animator anim;

    private bool isInputLocked;
    [Header("Attack")]
    [SerializeField]
    AttackPoints AttackPoints;
    Dictionary<Direction, Transform> DirToAttackPoint;
    Rigidbody rb;
    [SerializeField]
    List<float> BasicAttDelay;
    [SerializeField]
    List<float> BasicAttDmg;
    [SerializeField]
    List<float> BasicAttDuration;
    [SerializeField]
    List<float> BasicAttRange;
    int ComboIndex;

    List<int> fpsAVG = new List<int>();
    

    private void Start()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody>();
        dashScript = GetComponent<Dash>();

        playerDirection = Direction.Down;
        SetPlayerDirection(Direction.Down, Vector2.zero);

        DirToAttackPoint = new Dictionary<Direction, Transform>(){
            {Direction.Up, AttackPoints.upAttackPoint},
            {Direction.UpLeft, AttackPoints.upLeftAttackPoint},
            {Direction.Left, AttackPoints.leftAttackPoint},
            {Direction.DownLeft, AttackPoints.downLeftAttackPoint},
            {Direction.Down, AttackPoints.downAttackPoint},
            {Direction.DownRight, AttackPoints.downRightAttackPoint},
            {Direction.Right, AttackPoints.rightAttackPoint},
            {Direction.UpRight, AttackPoints.upLeftAttackPoint}
        };
    }

    void Update()
    {
        if (!isInputLocked){
            input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;

            if(Input.GetMouseButtonDown(0)){

                Attack();
            }

        }
        else {
            input = Vector2.zero;
            }

        if (input == Vector2.zero){ //is not moving
            anim.SetBool("moving", false);
        }


        //FPSCounter();
    }
    private void FixedUpdate()
    {
        playerPosition = transform.position;
        if (input != Vector2.zero)
        {
            Vector3 dir = new Vector3(input.x, 0, input.y).normalized;

            rb.MovePosition(playerPosition+(dir * movementSpeed));

            AnimatePlayerMovement(Vector3.Angle(dir, transform.forward), input);
        }
    }
    public Vector3 getPointerPos(){
        //raycasting from screenpoint to floor
        Ray r = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Physics.Raycast(r, out hit, 200, LayerMask.GetMask("Floor"));
        if(hit.collider){
            //hit connected with floor
            return hit.point;
        }
        else{
            //hit didn't connect
            return Vector3.zero;
        }
    }
    public void AnimatePlayerMovement(float angle, Vector2 direction)
    {
        anim.SetBool("moving", true);
        ChangeDirectionAccordingToInput(angle, direction);
    }

    public void ChangeDirectionAccordingToInput(float angle, Vector2 direction)
    {
        if (angle < 22.5f && playerDirection != Direction.Up){ // Looking up
            SetPlayerDirection(Direction.Up, direction);
        }

        if (angle >= 22.5f && angle < 67.5f) // Diagonal up
        {
            if (direction.x <= -0.1f && playerDirection != Direction.UpLeft) // Diagonal up-left
                SetPlayerDirection(Direction.UpLeft, direction);
            else if (direction.x >= 0.1f && playerDirection != Direction.UpRight) // Diagonal up-right
                SetPlayerDirection(Direction.UpRight, direction);
        }

        if (angle >= 67.5f && angle < 112.5f) // Horizontal
        {
            if (direction.x <= -0.1f && playerDirection != Direction.Left) // Looking left
                SetPlayerDirection(Direction.Left, direction);
            else if (direction.x >= 0.1f && playerDirection != Direction.Right) // Looking right
                SetPlayerDirection(Direction.Right, direction);
        }

        if (angle >= 112.5f && angle < 157.5f) // Diagonal down
        {
            if (direction.x <= -0.1f && playerDirection != Direction.DownLeft) // Diagonal down-left
                SetPlayerDirection(Direction.DownLeft, direction);
            else if (direction.x >= 0.1f && playerDirection != Direction.DownRight) // Diagonal down-right
                SetPlayerDirection(Direction.DownRight, direction);
        }

        if (angle >= 157.5f && playerDirection != Direction.Down) // Looking down
            SetPlayerDirection(Direction.Down, direction);
    }

    public Vector2 GetMouseClickScreenDirection()
    {
        Vector3 playerPos = Camera.main.WorldToScreenPoint(transform.position);
        Vector2 temp = Input.mousePosition - playerPos;
        return temp;
    }
    public void SetPlayerDirection(Direction direction, Vector2 vector)
    {
        int y = -1;
        int x = -1;
        playerDirection = direction;
        if(direction == Direction.Up || direction == Direction.UpLeft || direction == Direction.UpRight){
            y = 1;
        }
        else if(direction == Direction.Left || direction == Direction.Right){
            y = 0;
        }
        //y is default at -1 (down)
        if(direction == Direction.Right || direction == Direction.UpRight || direction == Direction.DownRight){
            x = 1;
        }
        else if(direction == Direction.Up || direction == Direction.Down){
            x = 0;
        }
        //x is default at -1 (left)
        anim.SetInteger("YLook", y);
        anim.SetInteger("XLook", x);

        anim.SetFloat("xmovement", x);
        anim.SetFloat("ymovement", y);
    }

    public void LockInput(bool value)
    {
        isInputLocked = value;
    }
    void Attack(){
        LookAtCursor();
        StartCoroutine("BasicAtt");
    }  
    public void LookAtCursor(){
        float angle = Vector3.Angle(getPointerPos() - transform.position, transform.forward);
        ChangeDirectionAccordingToInput(angle, getPointerPos() - transform.position);
    }
    void AttackDefinedPoint(Transform AttackPoint, float Damage){
        foreach(Collider c in Physics.OverlapSphere(AttackPoint.position, BasicAttRange[ComboIndex], LayerMask.GetMask("Ennemy"))){
            IDamagable Damageable = c.gameObject.GetComponent<IDamagable>();
            if(Damageable != null){
                Damageable.TakeDamage(BasicAttDmg[ComboIndex]);
            }
        }
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


    //attacks
    IEnumerator BasicAtt(){
        LockInput(true);
        anim.SetInteger("ComboState", ComboIndex);
        anim.SetBool("Attacking", true);
        yield return new WaitForSecondsRealtime(BasicAttDelay[ComboIndex]);
        anim.SetBool("Attacking", false);
        AttackDefinedPoint(DirToAttackPoint[playerDirection], BasicAttDmg[ComboIndex]);
        yield return new WaitForSecondsRealtime(BasicAttDuration[ComboIndex] - BasicAttDelay[ComboIndex]);
        ComboIndex ++;
        if(ComboIndex > 4){
            ComboIndex = 0;
        }
        LockInput(false);
    }

}
