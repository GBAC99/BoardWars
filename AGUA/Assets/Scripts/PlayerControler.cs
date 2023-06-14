using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControler : QadMovable
{
    public PlayerControler thisPlayer;
    public LayerMask qadLayer;


    // Start is called before the first frame update
    void Start()
    {
        Init();
        thisPlayer = gameObject.GetComponent<PlayerControler>();

        FindSelectableQads();
    }

    // Update is called once per frame
    void Update()
    {
        if (!moving)
        {
            CheckQadPos();
        }
        else
        {
           Move();
        }

    }
    public void StartPlayer(Vector3 initpos)
    {
        //
        Instantiate(thisPlayer, initpos, Quaternion.identity, null);
        //
    }

    void CheckQadPos()
    {
        if (Input.GetMouseButtonDown(0))
        {

            Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit raycastHit;


            if (Physics.Raycast(mouseRay, out raycastHit, 300, qadLayer.value))
            {
                if (raycastHit.collider.tag == "Qad")
                {
                    Qad q = raycastHit.collider.GetComponent<Qad>();
                    if (q.selectable)
                    {
                        MoveToQad(q);
                    }

                }

            }
        }

    }



}
