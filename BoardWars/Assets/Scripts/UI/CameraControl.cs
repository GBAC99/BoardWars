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
        if (!camAnim.GetBool("Strike"))
        {
            camAnim.SetBool("Strike", true);
            dirLightAnim.SetBool("Strike",true);
        }
    }

    public void ShakeOut()
    {
        camAnim.SetBool("Strike", false);
        dirLightAnim.SetBool("Strike", false);
    }

}
