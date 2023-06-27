using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GLS_PlayerAttack : GameLoopStates
{
    public GLS_PlayerAttack(GameLoopControler gC)
    {

        Debug.Log("player attack");
        for (int i = 0; i < gC.QAD_MANAGER.activePlayerPieces.Length; i++)
        {
            gC.QAD_MANAGER.activeEnemyPieces[i].GetComponent<EnemyControler>().GetCurrentQad();
            gC.QAD_MANAGER.activeEnemyPieces[i].GetComponent<EnemyControler>().TakeDamage(); 
        }

        change = true;
    }

    public override void CheckTransition(GameLoopControler gC)
    {
        if (change)
        { 
            for (int i = 0; i < gC.QAD_MANAGER.activeEnemyPieces.Length; i++)
            {
                gC.QAD_MANAGER.activeEnemyPieces[i].GetComponent<PlayerPieceControler>().ResetLists();
            }

            gC.ChangeState(new GLS_PlayerCheckAttackingQads(gC));
        }
    }

    public override void Update(GameLoopControler gC)
    {
    }


}
