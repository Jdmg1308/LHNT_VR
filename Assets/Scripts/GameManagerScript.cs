using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManagerScript : MonoBehaviour
{
    public static GameManagerScript instance { get; private set; }
    public GameObject bloodyOverlayPrefab;
    public float timeToDie = 4f;
    private bool isDying = false;


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

    [ContextMenu("Start Death Sequence")]
    public void StartDeathSequence()
    {
        // Start the death sequence
        isDying = true;
        bloodyOverlayPrefab.SetActive(true);
    }

    void Update()
    {
        if(isDying){
            timeToDie -= Time.deltaTime;
          
            // Increase alpha of bloody overlay image
            Color overlayColor = bloodyOverlayPrefab.GetComponent<Image>().color;
            overlayColor.a += Time.deltaTime / 3f; // Adjust the speed of fading in
            Image bloodyOverlayImage = bloodyOverlayPrefab.GetComponent<Image>();
            bloodyOverlayImage.color = overlayColor;

            if(timeToDie <= 0f){
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Reload the current scene
            }
        }
    }

}
