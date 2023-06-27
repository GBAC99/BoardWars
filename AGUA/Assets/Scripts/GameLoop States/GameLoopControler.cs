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


    /*Maquina de estados 
     * 
     * La tecnica que estoy usando pasa estados como clases a cambiar
     * Los estados tienen tres metodos (por ahora): un Constructor un Update y un CheckTransition
     * Dentro del constructor se inicializan variables necesarias para el estado
     * Tambien suceden las acciones de inicio de estado, como spawnear enemigos
     * Dentro del Update sucede lo que deba suceder durante el estado
     * Por ejemplo: corre un timer para cambiar auto, comprobar si queda algo por hacer...
     * En el CheckTransition simplemente se cambia al estado deseado, bien puede ser uno siguiente en la cadena o uno anterior
     * 
     * Estados (esta apuntado en un papel) : Init state, Spawn Enemies, Select Player Pieces, Place Player Pieces,+
     * +, Select Player Attack (si el character necesita input del player), Move Enemiy Pieces, Move Player Pieces,+
     * +, Enemies Attack, Kill Player Pieces (si muere alguna),Player Attack, Kill Enemy Pieces(si muere alguna), +
     * +, Acabar Nivel (si no quedan enemigos), Continuar nivel - LOOP - (si quedan enemigos).
     * Puede que queden por hacer o sobren
     * Los KILL puede que no hagan falta
     */
 

    // Start is called before the first frame update
    void Start()
    {

        if (instance == null) instance = this;
        else Destroy(gameObject);
         

        ChangeState(new GLS_InitLevel(this));

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
