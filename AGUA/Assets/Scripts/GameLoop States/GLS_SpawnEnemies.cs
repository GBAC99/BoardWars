using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GLS_SpawnEnemies : GameLoopStates
{
    //Spawn and place enemies


    public GLS_SpawnEnemies(GameLoopControler gC)
    {
        Debug.Log("Spawn Enemies!");
        timeToChange = 1;
        gC.QAD_MANAGER.SpawnEnemies();
    }

    public override void CheckTransition(GameLoopControler gC)
    {

        if (change) gC.ChangeState(new GLS_PlayerSelectPiece(gC)); 

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
