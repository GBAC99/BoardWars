using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GLS_InitLevel : GameLoopStates
{
    //Show level and some relevant things 
    public GLS_InitLevel(GameLoopControler gC)
    { 
        timeToChange = 3f; 
        Debug.Log("InitLevel");
    }

    public override void CheckTransition(GameLoopControler gC)
    {
        if (change) gC.ChangeState(new GLS_SpawnEnemies(gC));
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
