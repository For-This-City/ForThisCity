using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Dash))]
public class DashAttack : MonoBehaviour
{
    [SerializeField] private float dashDamage = 1f;
    [SerializeField] private float dashDurationIncrease = 0.05f;

    private Dash dashScript;
    
    void Start()
    {
        dashScript = GetComponent<Dash>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Damagable")
        {
            Debug.Log("Hit with dash");
            Component damageComponent = other.gameObject.GetComponent(typeof(IDamagable));
            if (damageComponent)
            {
                (damageComponent as IDamagable).TakeDamage(dashDamage);
                dashScript.AddToDashTime(dashDurationIncrease);
            }
        }
    }
}
