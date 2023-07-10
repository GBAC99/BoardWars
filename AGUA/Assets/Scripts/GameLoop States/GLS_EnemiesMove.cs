using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GLS_EnemiesMove : GameLoopStates
{
    public GLS_EnemiesMove(GameLoopControler gC)
    { 
        gC.SetRoundInfo(gC.roundInfoDictionary.enemiesMove);

        timeToChange = 3;

        change = gC.QAD_MANAGER.ControlEnemies();

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
                gC.ChangeState(new GLS_PlayerPieceCheckQads(gC));
            }
        }

    }

    public override void Update(GameLoopControler gC)
    {
    }




}
