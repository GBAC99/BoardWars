using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControler : MonoBehaviour
{
    public CameraControler thisCamera;
    public Transform target;

    public float smoothSpeed = 0.125f;
    public Vector3 offset;

    float maxFov = 80f;
    float minFov = 15f;
    float sensitivity = 10f;

    private void Start()
    {
        thisCamera = gameObject.GetComponent<CameraControler>();
        target = GameObject.FindGameObjectWithTag("CMap").transform;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            //Girar camara hacia la derecha
            if (offset.x > 0 && offset.z > 0) //++
            {
                offset.x = offset.x * -1;
            }
            else if (offset.x < 0 && offset.z > 0)//-+
            {
                offset.z = offset.z * -1;
            }
            else if (offset.x < 0 && offset.z < 0)//--
            {
                offset.x = offset.x * -1;
            }
            else if (offset.x > 0 && offset.z < 0)//+-
            {
                offset.z = offset.z * -1;
            }
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            //Girar camara hacia la izquierda 
            if (offset.x > 0 && offset.z > 0) //++
            {
                offset.z = offset.z * -1;
            }
            else if (offset.x < 0 && offset.z > 0)//-+
            {
                offset.x = offset.x * -1;
            }
            else if (offset.x < 0 && offset.z < 0)//--
            {
                offset.z = offset.z * -1;
            }
            else if (offset.x > 0 && offset.z < 0)//+-
            {
                offset.x = offset.x * -1;
            }
        }
    }

    private void FixedUpdate()
    {
        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;

        float mouseY = Input.GetAxis("Mouse Y");
        float mouseX = Input.GetAxis("Mouse X");

        if (Input.GetMouseButton(1))
        {
            offset.y += mouseY * sensitivity * Time.deltaTime;
        }

        offset.y = Mathf.Clamp(offset.y, 6f, 15f);

        float fov = Camera.main.fieldOfView;
        fov += Input.GetAxis("Mouse ScrollWheel") * sensitivity;
        fov = Mathf.Clamp(fov, minFov, maxFov);
        Camera.main.fieldOfView = fov;

        transform.LookAt(target);




    }
}
