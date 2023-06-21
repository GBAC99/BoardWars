using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GLS_EnemiesMove : GameLoopStates
{ 
    public GLS_EnemiesMove(GameLoopControler gC)
    {
        Debug.Log("move enemies");

        change = gC.QAD_MANAGER.ControlEnemies();

    }


    public override void CheckTransition(GameLoopControler gC)
    {

        if (change) gC.ChangeState(new GLS_PlayerPieceCheckQads(gC));

    }

    public override void Update(GameLoopControler gC)
    { 
    }




}
