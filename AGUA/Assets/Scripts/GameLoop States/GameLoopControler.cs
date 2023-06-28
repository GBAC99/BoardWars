using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class GameLoopControler : MonoBehaviour
{
    //Controls game states and UI needs.
    private GameLoopControler instance;


    public QadManager QAD_MANAGER;
    public Qad selectedQad;

    public GameObject selectedPlayerPiece;
    public Qad selectecQadPos;
    [HideInInspector]
    public int currentPlayerPiece = 0;

    public int currentEnemyPiece = 0;


    //Variables for checking states and transitioning.

    private GameLoopStates currentState;

    public bool levelWin;

    public bool firstRound;
 

    // Start is called before the first frame update
    void Start()
    {

        if (instance == null) instance = this;
        else Destroy(gameObject);
         

        ChangeState(new GLS_InitLevel(this));

        firstRound = true;

    }

    // Update is called once per frame
    void Update()
    {
        currentState.Update(this); 
    }

    private void LateUpdate()
    {
        currentState.CheckTransition(this); 
    }

    public void ChangeState(GameLoopStates gls)
    {
        currentState = gls;
    }
     
    public void SpawnSelectedPlayer()
    { 
        QAD_MANAGER.SpawnPlayerPiece(selectecQadPos, selectedPlayerPiece);
    }

    public void LoadNextLevel()
    {
        Debug.Log("Next Level Load!");
    }
    

}
