using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GLS_PlayerAttack : GameLoopStates
{
    public GLS_PlayerAttack(GameLoopControler gC)
    {
        gC.SetRoundInfo(gC.roundInfoDictionary.playerAttack);

        timeToChange = 2f;

        for (int i = 0; i < gC.QAD_MANAGER.activePlayerPieces.Length; i++)
        {
            if (gC.QAD_MANAGER.activeEnemyPieces[i].GetComponent<EnemyControler>().alive)
            {
                gC.QAD_MANAGER.activeEnemyPieces[i].GetComponent<EnemyControler>().GetCurrentQad();
                gC.QAD_MANAGER.activeEnemyPieces[i].GetComponent<EnemyControler>().currentQad.ActivateAttackingParticleEffect();
                gC.QAD_MANAGER.activeEnemyPieces[i].GetComponent<EnemyControler>().TakeDamage();
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
                for (int i = 0; i < gC.QAD_MANAGER.activeEnemyPieces.Length; i++)
                {
                    gC.QAD_MANAGER.activeEnemyPieces[i].GetComponent<EnemyControler>().ResetLists();
                }

                gC.ChangeState(new GLS_CheckWin(gC));
            }
        }

    }

    public override void Update(GameLoopControler gC)
    {
    }


}
