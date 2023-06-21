using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameLoopStates 
{

    public float timeToChange;
    public bool change = false;

    public abstract void Update(GameLoopControler gC);

    public abstract void CheckTransition(GameLoopControler gC);

    

}
