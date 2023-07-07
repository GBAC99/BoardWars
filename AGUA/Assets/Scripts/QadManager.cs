using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QadManager : MonoBehaviour
{
    //Main controler class for the game

    public CameraControler mainCam;

    public GameObject[] enemyPieces; //should recieve the information from the tool!
    public GameObject[] activeEnemyPieces;

    [Header("Set EnemyTypes | 0 -> EnemyVertical, 1 -> EnemyHorizontal, 2 -> EnemyHorse")]
    public int enemy1type;
    public int enemy2type;
    public int enemy3type;

    [Header("Set positions for the enemies || You can choose between 0 to 35")]
    public int enemy1Position;
    public int enemy2Position;
    public int enemy3Position;

    [SerializeField]
    List<Qad> enemyAttackedQads = new List<Qad>();
    public List<Qad> enemySelectableQads = new List<Qad>();

    List<GameObject> playerPieces = new List<GameObject>();
    public List<Qad> playerSelectableQads = new List<Qad>();

    public GameObject[] activePlayerPieces;
    public PlayerControler Player;
    int playerPieceQadPositionInit;

    public GameObject[] signPlayerPieces;
    public PlayerPieceSign[] activeSigns = new PlayerPieceSign[3];
    public Transform[] signPositions;

    [SerializeField]
    List<Qad> playerAttackedQads = new List<Qad>();

    public GameObject[] testingPlayerPieces;

    //Map creation variables
    public float offset = 0.05f;

    public GameObject Qad;
    public int x;
    public int y;

    public static int dimensions = 0;

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

    // Start is called before the first frame update
    void Start()
    {
        //playerPieces = GameObject.FindGameObjectsWithTag("PlayerPiece");
        glc.SetActive(false);
        CreateMap();

        //SpawnEnemies();
        //mainCam = Instantiate(mainCam);

    }

    private void Update()
    {
      /*  if (Input.GetKeyDown(KeyCode.Space))
        {
            ControlEnemies();
        }*/
    }

    //Create Map 
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
            QadList[i].GetComponent<Renderer>().enabled = true;
            QadList[i].GetComponent<QadHoverScript>().enabled = true;
            QadList[i].GetComponent<Animator>().enabled = true;

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

    public int SearchQadList(Qad qadToCheck)//Maybe delete this
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

    public GameObject GetQadbyIndex(int index)
    {
        return QadList[index];
    }

    public void QadCheckUp()
    {
        foreach (GameObject qad in QadList)
        {
            Qad q = qad.GetComponent<Qad>();

            q.CheckUp(q.transform);
        }
    }

    //Control Game

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

    public void SpawnPlayerPieceTESTING()
    {

        testingPlayerPieces[2].GetComponent<PlayerPieceControler>().Spawn(new Vector3(QadList[14].transform.position.x,
                  gameObject.transform.position.y + QadList[0].GetComponent<Collider>().bounds.extents.y + offset,
                  QadList[14].transform.position.z));

    }

    public void SpawnEnemies() //On development
    {

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

    //Creates the selected team of pieces to later reselect.
    public void SpawnSign(PlayerPieceSign pps, int pieceNum)
    {
        activeSigns[pieceNum] = pps.Spawn(pps, signPositions[pieceNum]);
        activeSigns[pieceNum].activeNumPiece = pieceNum;
    }

    public void SetSignCurrentHp()
    {
        for (int i = 0; i < activeSigns.Length; i++)
        {
            activeSigns[i].attachedPpc = activePlayerPieces[i].GetComponent<PlayerPieceControler>();
        }
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

    public void GetAttackingPlayerQads()
    {
        foreach (GameObject eP in activePlayerPieces)
        {
            eP.GetComponent<PlayerControler>().FindAttackingQads();
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
        enemyAttackedQads.Clear();
    }

    public void ClearPlayerAttackQads()
    {
        playerAttackedQads.Clear();
    }

    public void ClearLists()
    {
        activePlayerPieces = new GameObject[0];
        activeEnemyPieces = new GameObject[0];
    }



    public void CheckHover()
    {
        foreach (GameObject piece in playerPieces)
        {
            if (piece.GetComponent<HoverControl>().hover)
            {
                //piece.GetComponent<HoverControl>().hover;
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
