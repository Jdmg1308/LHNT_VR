using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public float health = 100f;
    private float maxHealth;

    void Start()
    {
        maxHealth = health;
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        GameplayUIScript.instance.healthBar.value = health / maxHealth; 
        if (health <= 0f)
        {
            // Die();
        }
    }
}
