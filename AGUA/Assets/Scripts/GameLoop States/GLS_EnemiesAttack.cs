using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GLS_EnemiesAttack : GameLoopStates
{

    public GLS_EnemiesAttack(GameLoopControler gC)
    {
        gC.SetRoundInfo(gC.roundInfoDictionary.enemiesAttack);

        timeToChange = 2f;
        for (int i = 0; i < gC.QAD_MANAGER.activePlayerPieces.Length; i++)
        {
            if (gC.QAD_MANAGER.activePlayerPieces[i].GetComponent<PlayerPieceControler>().alive)
            {
                gC.QAD_MANAGER.activePlayerPieces[i].GetComponent<PlayerPieceControler>().GetCurrentQad();
                gC.QAD_MANAGER.activePlayerPieces[i].GetComponent<PlayerPieceControler>().TakeDamage();
            }

        }

        change = true;
    }

    public override void CheckTransition(GameLoopControler gC)
    {

        if (change)
        {
            if (timeToChange >= 0)
            {
                timeToChange -= Time.deltaTime;
            }
            else
            {
                gC.currentPlayerPiece = 0;
                for (int i = 0; i < gC.QAD_MANAGER.activeEnemyPieces.Length; i++)
                {
                    gC.QAD_MANAGER.activeEnemyPieces[i].GetComponent<EnemyControler>().ResetLists();
                }

                gC.LoseGame();
                gC.QAD_MANAGER.ClearEnemyAttackQads();
                gC.ChangeState(new GLS_PlayerCheckAttackingQads(gC));
            }

        }
    }

    public override void Update(GameLoopControler gC)
    {

    }
}
