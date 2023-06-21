using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GLS_PlayerSelectQad : GameLoopStates
{  
    public GLS_PlayerSelectQad(GameLoopControler gC)
    {
        gC.selectecQadPos = null;
        Debug.Log("PPPlaceSelection"); 
    }

    public override void CheckTransition(GameLoopControler gC)
    {
        if (change) gC.ChangeState(new GLS_SpawnPlayerPiece(gC));
    }

    public override void Update(GameLoopControler gC)
    {
        if (Input.GetMouseButtonDown(0))
        { 
            Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit raycastHit;

            if (Physics.Raycast(mouseRay, out raycastHit, 300, gC.QAD_MANAGER.layerQad.value))
            {
                if (raycastHit.collider.tag == "Qad")
                {
                    Qad q = raycastHit.collider.GetComponent<Qad>();
                    if (q.walkable)
                    { 
                        gC.selectecQadPos = q;
                        change = true;
                    } 
                }

            }
        }
    }
}
