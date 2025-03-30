using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float lifeTime = 3f;
    public float damage = 30f;
    public GameObject HitSplash;


    private void Awake()
    {
        Destroy(gameObject, lifeTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "VisionDetection"){
            return;
        }
        if(collision.gameObject.tag == "Player"){
            Debug.Log("Player Hit");
            collision.gameObject.GetComponent<PlayerScript>().TakeDamage(damage);
        }

        ContactPoint contact = collision.contacts[0];
        Quaternion rot = Quaternion.FromToRotation(Vector3.forward, contact.normal); // turn to Normal
        Vector3 pos = contact.point;

        if (HitSplash != null)
        {
            var hitVFX = Instantiate(HitSplash, pos, rot);
        }

        Destroy(gameObject);
        Debug.Log("Collision happened");
    }
}
