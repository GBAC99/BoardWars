using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashCanControler : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Qad")
        {
            Destroy(collision.gameObject);
        }
    }
}
