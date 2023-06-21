using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GLS_PlayerPieceCheckQads : GameLoopStates
{
    public GLS_PlayerPieceCheckQads(GameLoopControler gC)
    {
        Debug.Log("Checking Qads");
        Debug.Log(gC.currentPlayerPiece);
        gC.QAD_MANAGER.activePlayerPieces[gC.currentPlayerPiece].GetComponent<PlayerPieceControler>().FindSelectableQads();
        change = true;
    }

    public override void CheckTransition(GameLoopControler gC)
    {
        if (change) gC.ChangeState(new GLS_PlayerSelectMoves(gC));
    }

    public override void Update(GameLoopControler gC)
    { 

    }
}
