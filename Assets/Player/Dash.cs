using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash : MonoBehaviour
{
    [SerializeField]
    LineRenderer Laser;
    bool isDashing;
    [SerializeField]
    Vector3 LaserStartOffset;
    PlayerController PC;
    [SerializeField]
    float DashSpeed;
    [HideInInspector]
    public bool IsDashing;
    [SerializeField]
    Rigidbody rb;
    [SerializeField]
    float Damage;
    float DistanceDashed = 0;
    private void Start() {
        PC = gameObject.GetComponent<PlayerController>();
    }
    void DrawLine(Vector3 Pos){
        Vector3[] Positions = new Vector3[2];
        Positions[0] = transform.position + LaserStartOffset;
        Positions[1] = Pos;
        Laser.SetPositions(Positions);
    }

    private void Update() {
        if(Input.GetMouseButton(1)){
            //GetMousePos And draw line
            Vector3 pointerPos = PC.getPointerPos();
            DrawLine(pointerPos);
        }
        else if(Input.GetMouseButtonUp(1)){
            //when mouse button is released, erase the line by drawing from transform.pos to to transform.pos
            DrawLine(transform.position + LaserStartOffset);
            //dash to pos
            IEnumerator coroutine = PerformDash(PC.getPointerPos());
            StartCoroutine(coroutine);
            //setting player direction to face the dash
            PC.LookAtCursor();
        }
    }
    
    IEnumerator PerformDash(Vector3 DashTo){
        DistanceDashed = 0;
        PC.LockInput(true);
        IsDashing = true;
        Vector3 DashDir = (transform.position - DashTo).normalized;
        float DashDistance = Vector3.Distance(transform.position, DashTo);
        while(DistanceDashed < DashDistance){
            rb.MovePosition(transform.position + DashDir * -DashSpeed);
            DistanceDashed += DashSpeed;
            yield return new WaitForFixedUpdate();
        }
        PC.LockInput(false);
        IsDashing = false;
    }
    //collision and damage
    private void OnCollisionEnter(Collision col) {
        if(IsDashing){
            IDamagable Damageable = col.gameObject.GetComponent<IDamagable>();
            if(Damageable != null){
                Damageable.TakeDamage(Damage);
            }
            else{ //stop dash
            DistanceDashed = Mathf.Infinity;
                PC.LockInput(false);
                IsDashing = false;
            }
        }
    }
}
