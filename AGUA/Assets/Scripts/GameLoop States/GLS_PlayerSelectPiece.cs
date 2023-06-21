using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GLS_PlayerSelectPiece : GameLoopStates
{
    public GLS_PlayerSelectPiece(GameLoopControler gC)
    {
        gC.selectedPlayerPiece = null;
        Debug.Log("PPSelection");
    }

    public override void CheckTransition(GameLoopControler gC)
    {
        if (change) gC.ChangeState(new GLS_PlayerSelectQad(gC));
    }

    public override void Update(GameLoopControler gC)
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit raycastHit;

            if (Physics.Raycast(mouseRay, out raycastHit, 800, gC.QAD_MANAGER.layerPlayerPiece.value))
            {
                if (raycastHit.collider.tag == "PlayerSign")
                {
                    PlayerPieceSign p = raycastHit.collider.GetComponent<PlayerPieceSign>();
                    if (p != null)
                    {
                        gC.selectedPlayerPiece = p.getPieceSign();
                        change = true;
                    }
                }
            }
        }
    }
}
