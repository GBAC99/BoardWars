using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFocusControler : MonoBehaviour
{

    public float camSpeed = 3.0f;

    public GameObject mainCamera;
    public float distance = 10;

    Vector3 initPos;

    // Start is called before the first frame update
    void Start()
    {
        initPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //Moving the camera's focus but keeping it in a desired distance form the origin
        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(Vector3.forward * camSpeed * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(Vector3.forward * -camSpeed * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(Vector3.right * camSpeed * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(Vector3.right * -camSpeed * Time.deltaTime);
        }
         
        transform.position = initPos + Vector3.ClampMagnitude(transform.position - initPos, distance);
         
    }


}
