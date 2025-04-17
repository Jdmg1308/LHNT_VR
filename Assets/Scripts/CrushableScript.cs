using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrushableScript : MonoBehaviour
{
    private bool crushing = false;

    void FixedUpdate()
    {
        if (crushing){
            if(transform.localScale.y > .1f){
                transform.localScale -= new Vector3(0, 0.01f, 0);
            }else{
                crushing = false;
            }
        }
    }

    public void Crush(){
        crushing = true;
    }
}
