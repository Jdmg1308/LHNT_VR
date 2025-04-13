using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public static PlayerScript instance { get; private set; }
    public float health = 100f;
    private float maxHealth;

    void Awake(){
        if(instance == null){
            instance = this;
        }else{
            Destroy(gameObject);
        }
    }


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
            GameManagerScript.instance.StartDeathSequence(); // Start the death sequence
        }
    }
    
    [ContextMenu("Take Damage")]
    public void TakeDamage()
    {
        TakeDamage(10f); // Example damage value
    }

    public void MoveForward(){
        // Move the player forward in the direction they are facing
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        transform.position += forward * Time.deltaTime;
    }
}
