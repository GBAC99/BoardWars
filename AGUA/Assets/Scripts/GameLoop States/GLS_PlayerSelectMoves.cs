using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GLS_PlayerSelectMoves : GameLoopStates
{

    bool loopAgain, nextState;
    public GLS_PlayerSelectMoves(GameLoopControler gC)
    {

        loopAgain = false;
        nextState = false;
        Debug.Log("select qad for pieces move");
    }

    public override void CheckTransition(GameLoopControler gC)
    {

        if (nextState) gC.ChangeState(new GLS_EnemiesAttack(gC));
        if (loopAgain) gC.ChangeState(new GLS_PlayerPieceCheckQads(gC));

    }

    public override void Update(GameLoopControler gC)
    {
        CheckQadPos(gC, gC.QAD_MANAGER.activePlayerPieces[gC.currentPlayerPiece]);
    }

    void CheckQadPos(GameLoopControler gC, GameObject ppC)
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
                    if (q.selectable)
                    {
                        ppC.GetComponent<PlayerPieceControler>().MoveToQad(q);
                        if (gC.currentPlayerPiece >= 3)
                        {
                            nextState = true;
                        }
                        else
                        {
                            gC.currentPlayerPiece += 1;
                            loopAgain = true; ;
                        }

                    }

                }

            }
        }

    }

}
