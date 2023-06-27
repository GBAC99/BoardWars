using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GLS_PlayerSelectMoves : GameLoopStates
{

    bool loopAgain, nextState, startTimer;
    public GLS_PlayerSelectMoves(GameLoopControler gC)
    {

        loopAgain = false;
        nextState = false;
        startTimer = false;

        timeToChange = 2f;

        Debug.Log("select qad for pieces move");
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
                gC.ChangeState(new GLS_EnemiesSelectAttackingQads(gC));
            }
            
        }
        if (loopAgain)
        {
            if (timeToChange >= 0)
            {
                timeToChange -= Time.deltaTime;
            }
            else
            {
                gC.ChangeState(new GLS_PlayerPieceCheckQads(gC));
            }
          
        }

    }

    public override void Update(GameLoopControler gC)
    {
        if (gC.currentPlayerPiece <= 2)
        {
            CheckQadPos(gC, gC.QAD_MANAGER.activePlayerPieces[gC.currentPlayerPiece]); 
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
                    if (q.selectable)
                    {
                        ppC.GetComponent<PlayerPieceControler>().MoveToQad(q);
                        if (gC.currentPlayerPiece < 3)
                        {
                           // gC.QAD_MANAGER.activePlayerPieces[gC.currentPlayerPiece].GetComponent<PlayerPieceControler>().ResetLists();
                            gC.currentPlayerPiece += 1;
                            loopAgain = true;
                        }

                    }

                }
            }
        }
    }

}
