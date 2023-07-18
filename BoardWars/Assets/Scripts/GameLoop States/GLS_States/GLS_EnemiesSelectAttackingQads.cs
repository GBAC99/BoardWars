using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GLS_EnemiesSelectAttackingQads : GameLoopStates
{

    public GLS_EnemiesSelectAttackingQads(GameLoopControler gC)
    { 

        gC.QAD_MANAGER.ClearEnemyAttackQads();

        gC.QAD_MANAGER.GetAttackingEnemyQads();

        gC.QAD_MANAGER.ActivateEnemyAttackQads();

        timeToChange = 1.5f;
    }

    public override void CheckTransition(GameLoopControler gC)
    {

        if (change) gC.ChangeState(new GLS_EnemiesAttack(gC));

    }

    public override void Update(GameLoopControler gC)
    {
        if (timeToChange >= 0)
        {
            timeToChange -= Time.deltaTime;
        }
        else
        {
            change = true;
        }
    }
}
