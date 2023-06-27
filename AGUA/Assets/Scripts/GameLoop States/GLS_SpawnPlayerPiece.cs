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
        if (nextState) gC.ChangeState(new GLS_EnemiesMove(gC));
        if (loopAgain) gC.ChangeState(new GLS_PlayerSelectPiece(gC));
    }

    public override void Update(GameLoopControler gC)
    {

    }
}
