using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GLS_PlayerCheckAttackingQads : GameLoopStates
{
    public GLS_PlayerCheckAttackingQads(GameLoopControler gC)
    {
        Debug.Log("Player selects attack");
        Debug.Log("The current piece attacking is: " + gC.currentPlayerPiece);

        if (gC.currentPlayerPiece > 2)
        {
            change = true;
        }
        else
        {
            gC.QAD_MANAGER.ActivatePlayerAttackQads
                (gC.QAD_MANAGER.activePlayerPieces[gC.currentEnemyPiece].
                GetComponent<PlayerPieceControler>());

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
