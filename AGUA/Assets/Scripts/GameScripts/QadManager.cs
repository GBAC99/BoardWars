using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class QadManager : MonoBehaviour
{
    //Main controler class for the game//

    public enum EnemyPieces
    {
        VERTICAL = 0,
        HORIZONTAL = 1,
        HORSE = 2
    }
    [Header("--Enemy Pieces that will spawn--")]

    [Header("LEVEL DESIGN TOOL")]

    public EnemyPieces enemyPiece1;
    public EnemyPieces enemyPiece2;
    public EnemyPieces enemyPiece3;

    int enemy1type;
    int enemy2type;
    int enemy3type;

    [Header("--Where each piece will spawn -- You can choose numbers between 0 and 35")]
    public int enemy1Position;
    public int enemy2Position;
    public int enemy3Position;

    [HideInInspector] public GameObject[] enemyPieces;
    [HideInInspector] public GameObject[] activeEnemyPieces;

    List<Qad> enemyAttackedQads = new List<Qad>();
    [HideInInspector] public List<Qad> enemySelectableQads = new List<Qad>();

    List<GameObject> playerPieces = new List<GameObject>();
    [HideInInspector] public List<Qad> playerSelectableQads = new List<Qad>();

    [HideInInspector] public GameObject[] activePlayerPieces;
    int playerPieceQadPositionInit;

    [Header("UI")]

    public GameObject[] signPlayerPieces;
    [HideInInspector] public PlayerPieceSign[] activeSigns = new PlayerPieceSign[3];
    public Transform[] signPositions;
    public GameObject[] arrowExtraSigns;
    public GameObject[] schyteExtraSigns;
    public GameObject[] hammerExtraSigns;
    public GameObject[] shipExtraSigns;

    List<Qad> playerAttackedQads = new List<Qad>();

    [HideInInspector] public GameObject[] testingPlayerPieces;

    [Header("Map creation Variables")]
    //Map creation variables
    public float offset = 0.05f;

    public GameObject Qad;
    public int x;
    public int y;

    public static int dimensions = 0;

    [Header("Qad Managing")]

    public GameObject[] QadList;
    [SerializeField]
    public static GameObject[] _QadList;
    float halfHeight = 0;

    public Transform QadSpawnPos;

    public LayerMask layerQad;
    public LayerMask layerPlayerPiece;

    [HideInInspector]
    public Vector3 mousePosition;

    public GameObject glc;

    private void OnGUI()
    {
        enemyPiece1 = (EnemyPieces)EditorGUILayout.EnumPopup("Select an enemy to place", enemyPiece1);
        enemyPiece2 = (EnemyPieces)EditorGUILayout.EnumPopup("Select an enemy to place", enemyPiece2);
        enemyPiece3 = (EnemyPieces)EditorGUILayout.EnumPopup("Select an enemy to place", enemyPiece3);
    }

    // Start is called before the first frame update
    void Start()
    {
        switch (enemyPiece1)
        {
            case EnemyPieces.VERTICAL:
                enemy1type = 0;

                break;
            case EnemyPieces.HORIZONTAL:
                enemy1type = 1;

                break;
            case EnemyPieces.HORSE:
                enemy1type = 2;

                break;
        }

        switch (enemyPiece2)
        {
            case EnemyPieces.VERTICAL:
                enemy2type = 0;

                break;
            case EnemyPieces.HORIZONTAL:
                enemy2type = 1;

                break;
            case EnemyPieces.HORSE:
                enemy2type = 2;

                break;
        }

        switch (enemyPiece3)
        {
            case EnemyPieces.VERTICAL:
                enemy3type = 0;

                break;
            case EnemyPieces.HORIZONTAL:
                enemy3type = 1;

                break;
            case EnemyPieces.HORSE:
                enemy3type = 2;

                break;
        }

        glc.SetActive(false);
        CreateMap();
    }

    // MAP CREATION //
    /*
     * 6x6 example
     * 
     *          5 11 17 23 29 35  -      Up Row
     *   l      4 10 16 22 28 34    r
     *   e c    3 9  15 21 27 33    i c
     *   f o    2 8  14 20 26 32    g o  
     *   t l    1 7  13 19 25 31    h l
     *          0 6  12 18 24 30  - t    Down Row
     *          
    */

    void CreateMap()
    {
        StartCoroutine(InstantiateQads());
    }

    IEnumerator InstantiateQads()
    {
        for (int i = 0; i < x; i++)
        {
            for (int b = 0; b < y; b++)
            {
                Instantiate(Qad, transform, true);
                yield return new WaitForSeconds(Random.Range(0.02f, 0.2f));
                MoveQadPos('y');
            }
            MoveQadPos('x');
        }
        QadList = GameObject.FindGameObjectsWithTag("Qad");
        StartCoroutine(ShowQads());

    }

    IEnumerator ShowQads()
    {
        for (int i = 0; i < QadList.Length; i++)
        {
            QadList[i].GetComponent<Qad>().enabled = true;
            QadList[i].GetComponent<Qad>().qadMat = QadList[i].GetComponent<Qad>().qadRender.material;
            QadList[i].GetComponent<Renderer>().enabled = true;
            QadList[i].GetComponent<QadHoverScript>().enabled = true;
            QadList[i].GetComponent<Animator>().enabled = true;
            QadList[i].GetComponent<Qad>().qadMat.SetFloat("_SpeedMultiplier", Random.Range(0.5f, 0.8f));
            yield return new WaitForSeconds(Random.Range(0.02f, 0.3f));
        }

        _QadList = QadList;

        int nCol = 0;
        for (int i = 0; i < QadList.Length; i++)
        {
            halfHeight = QadList[i].GetComponent<Collider>().bounds.extents.y;
            //Checks if Qad is in a DownRow or UpRow
            if (i == (y - 1) + x * nCol || i == (y + x * nCol))
            {
                QadList[i].GetComponent<Qad>().row = true;
            }

            //Checks if Qad is in a Initial or End Column 
            if ((i < y - 1 && i > 0) || (i > x * (y - 1) && i < (x + 1) * (y - 1)))
            {
                QadList[i].GetComponent<Qad>().col = true;
            }

            //Checks if Qad is in the corner
            if (i == 0 || i == y - 1 || i == x * (y - 1) || i == (x + 1) * (y - 1))
            {
                if (QadList[i].GetComponent<Qad>().col == true || QadList[i].GetComponent<Qad>().row == true)
                {
                    QadList[i].GetComponent<Qad>().col = false;
                    QadList[i].GetComponent<Qad>().row = false;
                }

                QadList[i].GetComponent<Qad>().corner = true;
            }

            if (i % y == 0 && i >= y)
            {
                nCol = nCol + 1;
            }
        }

        //SpawnPlayerPieceTESTING();
        //SpawnEnemies();
        glc.SetActive(true);

    }


    void MoveQadPos(char direction)
    {
        if (direction == 'x')
        {
            QadSpawnPos.Translate(1, 0, y * -1);
        }
        else if (direction == 'y')
        {
            QadSpawnPos.Translate(0, 0, 1);
        }
    }

    public int SearchQadList(Qad qadToCheck)
    {
        for (int i = 0; i < QadList.Length; i++)
        {
            if (qadToCheck.gameObject == QadList[i].gameObject)
            {
                return i;
            }
        }
        return 0;
    }

    public GameObject[] GetQadList()
    {
        GameObject[] _qadList;

        _qadList = QadList;

        return _qadList;
    }

    public void QadCheckUp()
    {
        foreach (GameObject qad in QadList)
        {
            Qad q = qad.GetComponent<Qad>();

            q.CheckUp(q.transform);
        }
    }

    //Control Game//

    public void ResetTiles()
    {
        for (int i = 0; i < QadList.Length; i++)
        {
            if (QadList[i] != null)
            {
                QadList[i].GetComponent<Qad>().Reset();
            }
        }
    }

    public void AddPlayerPieceSelected(GameObject playerPiece)
    {
        playerPieces.Add(playerPiece);
    }

    public int GetActivePlayerPiecesCount(bool firstRound)
    {
        if (firstRound)
        {

            activePlayerPieces = GameObject.FindGameObjectsWithTag("PlayerPiece");
            return activePlayerPieces.Length;
        }
        else
        {
            int activeCount = 0;
            for (int i = 0; i < activePlayerPieces.Length; i++)
            {
                if (activePlayerPieces[i].activeSelf)
                {
                    activeCount++;
                }
            }
            return activeCount;
        }
    }

    public int GetAlivePlayerPiecesCount()
    {
        int aliveCount = 0;
        for (int i = 0; i < activePlayerPieces.Length; i++)
        {
            if (activePlayerPieces[i].GetComponent<PlayerPieceControler>().alive)
            {
                aliveCount++;
            }
        }
        return aliveCount;
    }

    public void GetPlayerPiecePosition(Qad qSel)
    {
        for (int i = 0; i < QadList.Length; i++)
        {
            if (QadList[i].GetComponent<Qad>() == qSel) playerPieceQadPositionInit = i;
        }

    }

    public void SpawnPlayerPiece(Qad qadPos, GameObject piece)
    {
        playerPieceQadPositionInit = SearchQadList(qadPos);

        piece.GetComponent<PlayerPieceControler>().Spawn(new Vector3(QadList[playerPieceQadPositionInit].transform.position.x,
                  gameObject.transform.position.y + QadList[playerPieceQadPositionInit].GetComponent<Collider>().bounds.extents.y + offset,
                  QadList[playerPieceQadPositionInit].transform.position.z));

    }

    public void PlaceSelectedPiece(Qad qadPos, int pieceNum)
    {
        playerPieceQadPositionInit = SearchQadList(qadPos);

        activePlayerPieces[pieceNum].transform.position = new Vector3(QadList[playerPieceQadPositionInit].transform.position.x,
                  gameObject.transform.position.y + QadList[playerPieceQadPositionInit].GetComponent<Collider>().bounds.extents.y + offset,
                  QadList[playerPieceQadPositionInit].transform.position.z);

        activePlayerPieces[pieceNum].SetActive(true);
    }

    //TESTING AND DEVELOPMENT PURPOSES//
    public void SpawnPlayerPieceTESTING()
    {

        testingPlayerPieces[2].GetComponent<PlayerPieceControler>().Spawn(new Vector3(QadList[35].transform.position.x,
                  gameObject.transform.position.y + QadList[0].GetComponent<Collider>().bounds.extents.y + offset,
                  QadList[35].transform.position.z));

    }

    public void SpawnEnemies()
    {
        //Filter to never spawn enemies in the same exactly qad position.
        if (enemy1Position == enemy2Position && enemy2Position == enemy3Position)
        {
            ++enemy2Position;
            enemy3Position += enemy2Position + 1;
        }

        enemyPieces[enemy1type].GetComponent<EnemyControler>().Spawn(new Vector3(QadList[enemy1Position].transform.position.x,
           gameObject.transform.position.y + QadList[enemy1Position].GetComponent<Collider>().bounds.extents.y + offset,
           QadList[enemy1Position].transform.position.z));

        enemyPieces[enemy2type].GetComponent<EnemyControler>().Spawn(new Vector3(QadList[enemy2Position].transform.position.x,
            gameObject.transform.position.y + QadList[enemy2Position].GetComponent<Collider>().bounds.extents.y + offset,
            QadList[enemy2Position].transform.position.z));

        enemyPieces[enemy3type].GetComponent<EnemyControler>().Spawn(new Vector3(QadList[enemy3Position].transform.position.x,
            gameObject.transform.position.y + QadList[enemy3Position].GetComponent<Collider>().bounds.extents.y + offset,
            QadList[enemy3Position].transform.position.z));

        activeEnemyPieces = GameObject.FindGameObjectsWithTag("EnemyPiece");

    }

    //Creates the selected set of pieces to later reselect.
    public void SpawnSign(PlayerPieceSign pps, int pieceNum)
    {

        //tengo tres sign mas de cada una
        //cada vez que eligo un sign he de coger la primera que no este puesta ya / variable de control?

        switch (pps.characterType)
        {
            case PlayerPieceSign.CharacterT.PLAYER_ARROW:

                for (int i = 0; i < arrowExtraSigns.Length; i++)
                {

                    if (!arrowExtraSigns[i].GetComponentInChildren<PlayerPieceSign>().signPlaced)
                    {
                        arrowExtraSigns[i].SetActive(true);
                        arrowExtraSigns[i].transform.SetParent(signPositions[pieceNum],false);
                        arrowExtraSigns[i].GetComponentInChildren<PlayerPieceSign>().signPlaced = true;
                        activeSigns[pieceNum] = arrowExtraSigns[i].GetComponentInChildren<PlayerPieceSign>();
                        activeSigns[pieceNum].activeNumPiece = pieceNum;
                        break;
                    }

                }

                break;
            case PlayerPieceSign.CharacterT.PLAYER_SCHYTE:
                for (int i = 0; i < schyteExtraSigns.Length; i++)
                {
                    if (!schyteExtraSigns[i].GetComponentInChildren<PlayerPieceSign>().signPlaced)
                    {
                        schyteExtraSigns[i].SetActive(true);
                        schyteExtraSigns[i].transform.SetParent(signPositions[pieceNum], false);
                        schyteExtraSigns[i].GetComponentInChildren<PlayerPieceSign>().signPlaced = true;
                        activeSigns[pieceNum] = schyteExtraSigns[i].GetComponentInChildren<PlayerPieceSign>();
                        activeSigns[pieceNum].activeNumPiece = pieceNum;
                        break;
                    }
                }
                break;
            case PlayerPieceSign.CharacterT.PLAYER_HAMMMER:
                for (int i = 0; i < hammerExtraSigns.Length; i++)
                {
                    if (!hammerExtraSigns[i].GetComponentInChildren<PlayerPieceSign>().signPlaced)
                    {
                        hammerExtraSigns[i].SetActive(true);
                        hammerExtraSigns[i].transform.SetParent(signPositions[pieceNum], false);
                        hammerExtraSigns[i].GetComponentInChildren<PlayerPieceSign>().signPlaced = true;
                        activeSigns[pieceNum] = hammerExtraSigns[i].GetComponentInChildren<PlayerPieceSign>();
                        activeSigns[pieceNum].activeNumPiece = pieceNum;
                        break;
                    }
                }
                break;
            case PlayerPieceSign.CharacterT.PLAYER_SHIP:
                for (int i = 0; i < shipExtraSigns.Length; i++)
                {
                    if (!shipExtraSigns[i].GetComponentInChildren<PlayerPieceSign>().signPlaced)
                    {
                        shipExtraSigns[i].SetActive(true);
                        shipExtraSigns[i].transform.SetParent(signPositions[pieceNum],false);
                        shipExtraSigns[i].GetComponentInChildren<PlayerPieceSign>().signPlaced = true;
                        activeSigns[pieceNum] = shipExtraSigns[i].GetComponentInChildren<PlayerPieceSign>();
                        activeSigns[pieceNum].activeNumPiece = pieceNum;
                        break;
                    }
                }
                break;
        }

        /*
        activeSigns[pieceNum] = pps.Spawn(pps, signPositions[pieceNum]);
        activeSigns[pieceNum].activeNumPiece = pieceNum;*/
    }

    public void SetSignCurrentHp()
    {
        for (int i = 0; i < activeSigns.Length; i++)
        {
            activeSigns[i].attachedPpc = activePlayerPieces[i].GetComponent<PlayerPieceControler>();
        }
    }

    public bool CheckPlayerAlive()
    {
        bool alive = false;

        foreach (GameObject ppC in activePlayerPieces)
        {
            if (ppC.GetComponent<PlayerPieceControler>().alive && ppC != null)
            {
                return true;
            }
        }

        return alive;

    }

    public bool ControlEnemies()
    {
        foreach (GameObject eP in activeEnemyPieces)
        {
            if (eP.GetComponent<EnemyControler>().alive)
            {
                eP.GetComponent<EnemyControler>().FindSelectableQads();
                if (eP.GetComponent<EnemyControler>().selectableQads.Count > 0)
                {
                    eP.GetComponent<EnemyControler>().MoveEnemy();
                }
            }
        }
        return true;

    }

    public void GetEnemySelectableQads()
    {
        foreach (GameObject eP in activeEnemyPieces)
        {
            if (eP.GetComponent<EnemyControler>().alive)
            {
                eP.GetComponent<EnemyControler>().FindSelectableQads();
            }
        }
    }

    public void GetAttackingEnemyQads()
    {
        foreach (GameObject eP in activeEnemyPieces)
        {
            if (eP.GetComponent<EnemyControler>().alive)
            {
                eP.GetComponent<EnemyControler>().FindAttackingQads();
            }
        }
    }

    public void AddtoEnemyAttackQads(Qad q, float _damage)
    {
        enemyAttackedQads.Add(q);
        q.SetDamageInQad(_damage);
    }

    public void AddtoPlayerAttackQads(Qad q, float _damage)
    {
        playerAttackedQads.Add(q);
        q.SetDamageInQad(_damage);
    }

    public void ActivateEnemyAttackQads()
    {
        foreach (Qad qA in enemyAttackedQads)
        {
            qA.attacked = true; 
        }
    }

    public void ClearSelectableQads(bool clearPSQ)
    {
        enemySelectableQads.Clear();

        if (clearPSQ) playerSelectableQads.Clear();

    }

    public void ActivateEnemySelectableQads()
    {
        foreach (Qad qS in enemySelectableQads)
        {
            qS.currentPieceType = 1;
            qS.selectable = true;
        }
    }

    public void ActivatePlayerSelectableQads()
    {
        foreach (Qad qS in playerSelectableQads)
        {
            qS.currentPieceType = 0;
            qS.selectable = true;
        }
    }


    public void ActivatePlayerAttackQads(PlayerPieceControler pC)
    {
        foreach (Qad qA in pC.attackingQads)
        {
            qA.attacked = true;
        }
    }

    public void ClearEnemyAttackQads()
    {
        foreach (Qad q in enemyAttackedQads)
        {
            q.ResetDamage();
        }

        enemyAttackedQads.Clear();
    }

    public void ClearPlayerAttackQads()
    {
        foreach (Qad q in playerAttackedQads)
        {
            q.ResetDamage();
        }
        playerAttackedQads.Clear();
    }

    public void ClearLists()
    {
        activePlayerPieces = new GameObject[0];
        activeEnemyPieces = new GameObject[0];
    }

    public void ResetSigns()
    {
        foreach (PlayerPieceSign sign in activeSigns)
        {
            sign.usingPiece = false;
            sign.rend.material.color = Color.white;
        }
    }

    public void SaveAlivePieces()
    {
        foreach (GameObject p in activePlayerPieces)
        {
            PlayerPieceControler aP = p.GetComponent<PlayerPieceControler>();

            if (aP.alive)
            {
                aP.GetCurrentQad();
                aP.currentQad.currentPieceType = 0;
                aP.currentQad.GetComponent<Animator>().SetBool("RESET", true);
            }

        }
    }

    public void ExplodeQads()
    {
        foreach (GameObject q in QadList)
        {
            if (q != null)
            {
                q.GetComponent<Qad>().AddRigidBody();
            }

        }
    }

    public static int GetQadIndex(Qad q)
    {

        for (int i = 0; i < _QadList.Length; i++)
        {
            if (q.gameObject == _QadList[i].gameObject)
            {
                return i;
            }
        }
        return 0;
    }

}
