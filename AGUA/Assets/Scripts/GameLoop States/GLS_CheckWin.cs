using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GLS_CheckWin : GameLoopStates
{

    public GLS_CheckWin(GameLoopControler gC)
    {
        Debug.Log("Check if Enemies alive");
    }

    public override void CheckTransition(GameLoopControler gC)
    {
        if (CheckWin(gC)) gC.LoadNextLevel();

        else
        {

            gC.StartNewRound();
            gC.QAD_MANAGER.ClearSelectableQads(true);
            gC.ChangeState(new GLS_PlayerSelectPiece(gC,false));
        }
    }

    public override void Update(GameLoopControler gC)
    { }

    bool CheckWin(GameLoopControler gC)
    {
        for (int i = 0; i < gC.QAD_MANAGER.activeEnemyPieces.Length; i++)
        {
            if (gC.QAD_MANAGER.activeEnemyPieces[i].GetComponent<EnemyControler>().alive)
            {
                return false;
            }
        }

        return true;

    }

}
