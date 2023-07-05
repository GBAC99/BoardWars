using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GLS_PlayerSelectAttacks : GameLoopStates
{

    bool loopAgain, nextState;
    public GLS_PlayerSelectAttacks(GameLoopControler gC)
    {
        loopAgain = false;
        nextState = false;

        timeToChange = 2f;

        Debug.Log("select qad for piece's attack");
    }

    public override void CheckTransition(GameLoopControler gC)
    {
        if (nextState)
        {

            if (timeToChange >= 0)
            {
                timeToChange -= Time.deltaTime;
            }
            else
            {
                gC.ChangeState(new GLS_PlayerAttack(gC));
            }

        }
        else if (loopAgain)
        {
            if (timeToChange >= 0)
            {
                timeToChange -= Time.deltaTime;
            }
            else
            {
                gC.ChangeState(new GLS_PlayerCheckAttackingQads(gC));
            }

        }

    }

    public override void Update(GameLoopControler gC)
    {
        if (gC.currentPlayerPiece < 3)
        {
            if (!gC.QAD_MANAGER.activePlayerPieces[gC.currentPlayerPiece].GetComponent<PlayerPieceControler>().alive)
            {
                gC.currentPlayerPiece += 1;
                loopAgain = true;
            }
            else
            {
                CheckQadPos(gC, gC.QAD_MANAGER.activePlayerPieces[gC.currentPlayerPiece]);

            }

        }
        else
        {
            if (timeToChange >= 0)
            {
                timeToChange -= Time.deltaTime;
            }
            else
            {
                nextState = true;
            }
        }
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
                    if (q.attacked)
                    {
                        ppC.GetComponent<PlayerPieceControler>().SetAttackQad(q);
                        gC.QAD_MANAGER.activePlayerPieces[gC.currentPlayerPiece].GetComponent<PlayerPieceControler>().ResetLists();
                        q.attacked = true;
                        if (gC.currentPlayerPiece < 3)
                        {
                            gC.currentPlayerPiece += 1;
                            loopAgain = true;
                        }

                    }

                }
            }
        }
    }

}
