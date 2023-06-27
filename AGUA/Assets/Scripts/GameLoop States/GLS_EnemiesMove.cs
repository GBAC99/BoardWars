using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GLS_EnemiesMove : GameLoopStates
{
    public GLS_EnemiesMove(GameLoopControler gC)
    {
        Debug.Log("move enemies");
        timeToChange = 3; //Time is needed to let the pieces move to the desired spot
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
