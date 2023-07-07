using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GLS_PlayerSelectPiece : GameLoopStates
{
    bool loopAgain, nextState;

    public GLS_PlayerSelectPiece(GameLoopControler gC, bool enemiesAttack)
    {
        loopAgain = false;
        nextState = false;

        timeToChange = 2f;

        if (enemiesAttack)
        {
            nextState = true;
        }

        gC.selectedPlayerPiece = null;
        gC.QAD_MANAGER.GetEnemySelectableQads();
        gC.QAD_MANAGER.ActivateEnemySelectableQads();
        gC.QAD_MANAGER.ActivatePlayerSelectableQads();
        Debug.Log("PPSelection");
    }

    public override void CheckTransition(GameLoopControler gC)
    {
        if (loopAgain)
        {
            gC.QAD_MANAGER.ClearSelectableQads(false);
            gC.ChangeState(new GLS_PlayerSelectQad(gC));
        }
        if (change)
        {

            gC.QAD_MANAGER.ClearSelectableQads(true);
            gC.ChangeState(new GLS_EnemiesMove(gC));
        }

    }

    public override void Update(GameLoopControler gC)
    {
        SelectPiece(gC);

        if (nextState)
        {
            if (timeToChange >= 0)
            {
                timeToChange -= Time.deltaTime;
            }
            else
            {
                change = true;
            }
        }

    }


    void SelectPiece(GameLoopControler gC)
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
                    if (p != null && p.selectable)
                    {

                        if (gC.firstRound)
                        {
                            gC.selectedPlayerPiece = p.getPieceSign();
                            gC.QAD_MANAGER.SpawnSign(p, gC.QAD_MANAGER.GetActivePlayerPiecesCount(true));
                        }
                        else
                        {
                            p.usingPiece = true;
                            gC.selectedPlayerPieceNum = p.GetPieceNum();
                        }

                        loopAgain = true;
                    }
                }
            }
        }
    }
}
