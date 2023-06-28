using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GLS_SpawnPlayerPiece : GameLoopStates
{
    bool nextState, loopAgain;

    int activePlayerPieces;

    public GLS_SpawnPlayerPiece(GameLoopControler gC)
    {
        Debug.Log("SpawnPlayerPiece");
        gC.SpawnSelectedPlayer();
        activePlayerPieces = gC.QAD_MANAGER.GetActivePlayerPiecesCount();

        if (activePlayerPieces == 1 || activePlayerPieces == 2)
        {
            loopAgain = true;
        }
        else if (activePlayerPieces == 3)
        {
            nextState = true;
        }
    }

    public override void CheckTransition(GameLoopControler gC)
    {
        if (nextState)
        {
            if (gC.firstRound)
            {
                SetSelectableSigns(gC);
            }
            gC.ChangeState(new GLS_EnemiesMove(gC));
        }
        if (loopAgain) gC.ChangeState(new GLS_PlayerSelectPiece(gC));
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

                    Debug.Log("Still being selectable");
                    gC.QAD_MANAGER.signPlayerPieces[i].GetComponentInChildren<PlayerPieceSign>().selectable = true;
                    //Debug.Log("Im selectable: " + gC.QAD_MANAGER.signPlayerPieces[i].GetComponentInChildren<HoverControl>().selectable);


                }
            }

        }
    }

}
