using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GLS_CheckWin : GameLoopStates
{
    public bool showWinPanel;
    public GLS_CheckWin(GameLoopControler gC)
    {

        showWinPanel = false;

    }

    public override void CheckTransition(GameLoopControler gC)
    {
        if (CheckWin(gC))
        {
            gC.QAD_MANAGER.ExplodeQads();
            change = true;
            if (showWinPanel)
            {
                gC.WinLevel();
            }
        }

        else
        {
            gC.StartNewRound();
            gC.ChangeState(new GLS_PlayerSelectPiece(gC, false));
        }
    }

    public override void Update(GameLoopControler gC)
    {
        if (change)
        {
            if (timeToChange >= 0)
            {
                timeToChange -= Time.deltaTime;
            }
            else
            {

                showWinPanel = true;

            }
        }
    }

    bool CheckWin(GameLoopControler gC)
    {
        for (int i = 0; i < gC.QAD_MANAGER.activeEnemyPieces.Length; i++)
        {
            if (gC.QAD_MANAGER.activeEnemyPieces[i].GetComponent<EnemyControler>().alive 
                && gC.QAD_MANAGER.activeEnemyPieces[i] != null)
            {
                return false;
            }
        }

        return true;

    }

}
