using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameLoopControler : MonoBehaviour
{ 
    //Controls game states and UI needs. 
    
    [Header("--Game Control Variables--")]
    private GameLoopControler instance;

    public QadManager QAD_MANAGER;
    public Qad selectedQad;

    public GameObject selectedPlayerPiece;
    public int selectedPlayerPieceNum;
    public Qad selectedQadPos;

    [HideInInspector] public int currentPlayerPiece = 0;
    [HideInInspector] public int currentEnemyPiece = 0;


    [Header("--UI Objects--")]
    public GameObject anchorControlObj;
    public Animator anchorControl;
    public GameObject anchorInfoObj;
    public Animator anchorInfoControl;

    public GameObject enemyH_infoPanel;
    public GameObject enemyHZ_infoPanel;
    public GameObject enemyV_infoPanel;
    public GameObject playerArrow_infoPanel;
    public GameObject playerSchyte_infoPanel;
    public GameObject playerHammer_infoPanel;
    public GameObject playerShip_infoPanel;

    public GameObject losePanel;
    public GameObject winPanel;

    public TextMeshProUGUI roundInfo;
    public TextMeshProUGUI roundInfo2;
    public RoundInfo roundInfoDictionary;


    //Variables for checking states and transitioning.

    private GameLoopStates currentState;

    [HideInInspector]
    public bool levelWin;

    [HideInInspector]
    public bool firstRound;

    [HideInInspector]
    public bool piecesActive;

    // Start is called before the first frame update
    void Start()
    {

        if (instance == null) instance = this;
        else Destroy(gameObject);

        SetRoundInfo(" ");

        ChangeState(new GLS_InitLevel(this));

        levelWin = false;
        piecesActive = false;
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
        QAD_MANAGER.SpawnPlayerPiece(selectedQadPos, selectedPlayerPiece);
    }

    public void PlaceSelectedPiece()
    {
        QAD_MANAGER.PlaceSelectedPiece(selectedQadPos, selectedPlayerPieceNum);
    }

    public void StartNewRound()
    {

        firstRound = false;
        for (int i = 0; i < QAD_MANAGER.activePlayerPieces.Length; i++)
        {
            if (!QAD_MANAGER.activePlayerPieces[i].GetComponent<PlayerPieceControler>().alive)
            {
                Destroy(QAD_MANAGER.activeSigns[i]);
            }
        }

        QAD_MANAGER.SaveAlivePieces();
        QAD_MANAGER.ResetSigns();
        QAD_MANAGER.ResetTiles();
        QAD_MANAGER.ClearEnemyAttackQads();
        QAD_MANAGER.ClearPlayerAttackQads();
        QAD_MANAGER.ClearSelectableQads(true);
        currentPlayerPiece = 0;
    }

    public void ShowInfo(string characterType)
    {
        anchorInfoObj.SetActive(true);
        anchorInfoControl.SetBool("ShowP", true);

        switch (characterType)
        {
            case "enemyH":
                enemyH_infoPanel.SetActive(true);

                break;
            case "enemyHZ":
                enemyHZ_infoPanel.SetActive(true);

                break;
            case "enemyV":
                enemyV_infoPanel.SetActive(true);

                break;
            case "playerArrow":
                playerArrow_infoPanel.SetActive(true);

                break;
            case "playerSchyte":
                playerSchyte_infoPanel.SetActive(true);

                break;
            case "playerHammer":
                playerHammer_infoPanel.SetActive(true);

                break;
            case "playerShip":
                playerShip_infoPanel.SetActive(true);

                break;
        }
    }

    public void HideInfo(string characterType)
    {
        anchorInfoControl.SetBool("ShowP", false);

        switch (characterType)
        {
            case "enemyH":
                enemyH_infoPanel.SetActive(false);

                break;
            case "enemyHZ":
                enemyHZ_infoPanel.SetActive(false);

                break;
            case "enemyV":
                enemyV_infoPanel.SetActive(false);

                break;
            case "playerArrow":
                playerArrow_infoPanel.SetActive(false);

                break;
            case "playerSchyte":
                playerSchyte_infoPanel.SetActive(false);

                break;
            case "playerHammer":
                playerHammer_infoPanel.SetActive(false);

                break;
            case "playerShip":
                playerShip_infoPanel.SetActive(false);

                break;
        }
    }

    public void SetRoundInfo(string _roundInfo)
    {
        roundInfo.text = _roundInfo;
        roundInfo2.text = _roundInfo;
    }

    public void LoadNextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void LoseGame()
    {
        if (!QAD_MANAGER.CheckPlayerAlive())
        {
            anchorControlObj.SetActive(false);
            anchorInfoObj.SetActive(false);
            losePanel.SetActive(true);
        }
    }

    public void WinLevel()
    { 
        anchorControlObj.SetActive(false);
        anchorInfoObj.SetActive(false);
        winPanel.SetActive(true);
    }

}
