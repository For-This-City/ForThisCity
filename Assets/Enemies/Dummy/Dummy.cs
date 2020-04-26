using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dummy : MonoBehaviour, IDamagable
{
    Animator anim;
    [SerializeField] float maxHealth = 3;
    private float currentHealth;

    void Start()
    {
        anim = GetComponent<Animator>();

        currentHealth = maxHealth;
    }
    
    public void TakeDamage(float damage)
    {
        if (this.gameObject == null)
        {
            return;
        }

        if(anim != null)
            anim.SetTrigger("TakeDamage");
        currentHealth -= damage;

        if(currentHealth <= 0)
        {
            Destroy(gameObject, 0.2f);
        }
    }
}
