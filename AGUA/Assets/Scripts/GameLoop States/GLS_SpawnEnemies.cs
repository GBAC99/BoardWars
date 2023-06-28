using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GLS_SpawnEnemies : GameLoopStates
{
    //Spawn and place enemies


    public GLS_SpawnEnemies(GameLoopControler gC)
    {
        gC.QAD_MANAGER.ClearLists();
        Debug.Log("Spawn Enemies!");
        timeToChange = 1f;
        gC.QAD_MANAGER.SpawnEnemies();

        for (int i = 0; i < gC.QAD_MANAGER.signPlayerPieces.Length; ++i)
        {
            gC.QAD_MANAGER.signPlayerPieces[i].GetComponent<PlayerPieceSign>().selectable = true;
        }

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
