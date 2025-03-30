using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameplayUIScript : MonoBehaviour
{
    public static GameplayUIScript instance { get; private set; }
    public Slider healthBar;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }   
    }
}
