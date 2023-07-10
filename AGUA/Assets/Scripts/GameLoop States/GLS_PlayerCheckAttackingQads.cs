using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GLS_PlayerCheckAttackingQads : GameLoopStates
{
    public GLS_PlayerCheckAttackingQads(GameLoopControler gC)
    { 

        if (gC.currentPlayerPiece > 2 || !gC.QAD_MANAGER.activePlayerPieces[gC.currentPlayerPiece].GetComponent<PlayerPieceControler>().alive)
        {
            change = true;
        }
        else
        {
            gC.QAD_MANAGER.activePlayerPieces[gC.currentPlayerPiece].GetComponent<PlayerPieceControler>().FindAttackingQads();
            change = true;
        }
    }

    public override void CheckTransition(GameLoopControler gC)
    {
        if (change) gC.ChangeState(new GLS_PlayerSelectAttacks(gC));
    }

    public override void Update(GameLoopControler gC)
    {

    }


}
