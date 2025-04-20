using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // This function can be called from other scripts or UI buttons
    public void LoadGame()
    {
        SceneManager.LoadScene("LEVEL_1");
    }
}
