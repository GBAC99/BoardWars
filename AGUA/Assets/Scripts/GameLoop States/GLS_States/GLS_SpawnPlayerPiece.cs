using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GLS_SpawnPlayerPiece : GameLoopStates
{
    bool nextState, loopAgain;

    int activePlayerPieces;

    public GLS_SpawnPlayerPiece(GameLoopControler gC)
    { 

        timeToChange = 2f;
        if (gC.firstRound)
        {
            gC.SpawnSelectedPlayer();
        }
        else
        {
            gC.PlaceSelectedPiece();
        }

        if (gC.firstRound)
        {
            activePlayerPieces = gC.QAD_MANAGER.GetActivePlayerPiecesCount(true);
            if (activePlayerPieces == 1 || activePlayerPieces == 2)
            {
                loopAgain = true;
            }
            else if (activePlayerPieces == 3)
            {
                nextState = true;
            }
        }
        else
        {
            activePlayerPieces = gC.QAD_MANAGER.GetActivePlayerPiecesCount(false);

            if (gC.QAD_MANAGER.GetAlivePlayerPiecesCount() == 1)
            {
                if (activePlayerPieces == 1)
                {
                    nextState = true;
                }
            }
            else if (gC.QAD_MANAGER.GetAlivePlayerPiecesCount() == 2)
            {
                if (activePlayerPieces < 2)
                {
                    loopAgain = true;

                }
                else
                {
                    nextState = true;
                }
            }
            else if (gC.QAD_MANAGER.GetAlivePlayerPiecesCount() == 3)
            {
                if (activePlayerPieces < 3)
                {
                    loopAgain = true;
                }
                else
                {
                    nextState = true;
                }
            }
        }
    }

    public override void CheckTransition(GameLoopControler gC)
    {
        if (nextState)
        {
            if (gC.firstRound)
            {
                SetSelectableSigns(gC);
                gC.QAD_MANAGER.SetSignCurrentHp();
                gC.anchorControl.SetBool("ShowP", true);
            }
            gC.ChangeState(new GLS_PlayerSelectPiece(gC,true));
        }
        if (loopAgain) gC.ChangeState(new GLS_PlayerSelectPiece(gC,false));
    }

    public override void Update(GameLoopControler gC)
    {
    }

    void SetSelectableSigns(GameLoopControler gC)
    {
        for (int i = 0; i < gC.QAD_MANAGER.signPlayerPieces.Length; i++)
        {
            gC.QAD_MANAGER.signPlayerPieces[i].GetComponentInChildren<PlayerPieceSign>().selectable = false;
            for (int j = 0; j < gC.QAD_MANAGER.activePlayerPieces.Length; j++)
            {
                if (gC.QAD_MANAGER.signPlayerPieces[i].GetComponent<PlayerPieceSign>().piece.GetComponent<PlayerPieceControler>().characterType
                    == gC.QAD_MANAGER.activePlayerPieces[j].GetComponent<PlayerPieceControler>().characterType)
                {

                    gC.QAD_MANAGER.signPlayerPieces[i].GetComponentInChildren<PlayerPieceSign>().selectable = true;

                }
            }

        }
    }

}
