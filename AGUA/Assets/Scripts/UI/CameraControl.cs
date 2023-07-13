using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{

    public Animator camAnim;

    public void Shake()
    {
        if (!camAnim.GetBool("Shake"))
        {
            camAnim.SetBool("Shake", true);
        }
    }

    public void ShakeOut()
    {
        camAnim.SetBool("Shake", false);
    }

}
