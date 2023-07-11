using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GLS_PlayerPieceCheckQads : GameLoopStates
{
    public GLS_PlayerPieceCheckQads(GameLoopControler gC)
    { 
        gC.QAD_MANAGER.ClearSelectableQads(true); 

        if (gC.currentPlayerPiece > 2)
        {
            change = true;
        }
        else
        {
            if (gC.QAD_MANAGER.activePlayerPieces[gC.currentPlayerPiece].GetComponent<PlayerPieceControler>().alive)
            { 
                gC.QAD_MANAGER.activePlayerPieces[gC.currentPlayerPiece].GetComponent<PlayerPieceControler>().FindSelectableQads();
            }

            change = true;

        }
    }

    public override void CheckTransition(GameLoopControler gC)
    {
        if (change)
        {
            gC.ChangeState(new GLS_PlayerSelectMoves(gC));
        }
    }

    public override void Update(GameLoopControler gC)
    {

    }
}
