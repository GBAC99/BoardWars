using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GLS_EnemiesSelectAttackingQads : GameLoopStates
{

    public GLS_EnemiesSelectAttackingQads(GameLoopControler gC)
    {
        Debug.Log("Enemies check qads to attack");

        gC.QAD_MANAGER.GetAttackingEnemyQads();

        gC.QAD_MANAGER.ActivateEnemyAttackQads();

        change = true;
    }

    public override void CheckTransition(GameLoopControler gC)
    {

        if (change) gC.ChangeState(new GLS_EnemiesAttack(gC));

    }

    public override void Update(GameLoopControler gC)
    {

    }
}
