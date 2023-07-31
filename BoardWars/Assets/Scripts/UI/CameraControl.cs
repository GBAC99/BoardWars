using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{

    public Animator camAnim;


    [Header("VFX")]

    public Animator dirLightAnim;

    public void Shake()
    {
        if (!camAnim.GetBool("Shake"))
        {
            camAnim.SetBool("Shake", true);
            dirLightAnim.SetBool("Strike",true);
        }
    }

    public void ShakeOut()
    {
        camAnim.SetBool("Shake", false);
        dirLightAnim.SetBool("Strike", false);
    }

}
