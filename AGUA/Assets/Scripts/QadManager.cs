using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QadManager : MonoBehaviour
{
    //Main controler class for the game

    public CameraControler mainCam;

    public GameObject[] enemyPieces; //should recieve the information from the tool!
    public GameObject[] activeEnemyPieces;
    public int positionTriangle;
    public int positionDiamond;
    public int positionCube;

    List<GameObject> playerPieces = new List<GameObject>();
    public GameObject[] activePlayerPieces;
    public PlayerControler Player;
    int playerPieceQadPositionInit;

    float offset = 0.05f;

    public GameObject Qad;
    public int x;
    public int y;

    public GameObject[] QadList;
    float halfHeight = 0;

    public Transform QadSpawnPos;

    public LayerMask layerQad;
    public LayerMask layerPlayerPiece;

    [HideInInspector]
    public Vector3 mousePosition;

    // Start is called before the first frame update
    void Start()
    {
        //playerPieces = GameObject.FindGameObjectsWithTag("PlayerPiece");
        CreateMap();
        //SpawnPlayerPiece(); 

        mainCam = Instantiate(mainCam);

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ControlEnemies();
        }
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
        for (int i = 0; i < x; i++)
        {
            for (int b = 0; b < y; b++)
            {
                Instantiate(Qad, transform, true);
                MoveQadPos('y');
            }
            MoveQadPos('x');
        }

        QadList = GameObject.FindGameObjectsWithTag("Qad");


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
                Debug.Log("Qad's index is: " + i);
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

    //Control Game

    public void AddPlayerPieceSelected(GameObject playerPiece)
    {
        playerPieces.Add(playerPiece);
    }

    public void AddToActivePlayerPieces(GameObject activePlayerPiece)
    {
        // activePlayerPieces.Add(activePlayerPiece);
    }



    public int GetActivePlayerPiecesCount()
    {
        activePlayerPieces = GameObject.FindGameObjectsWithTag("PlayerPiece");
        return activePlayerPieces.Length;

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
        positionCube = SearchQadList(qadPos);

        piece.GetComponent<PlayerPieceControler>().Spawn(new Vector3(QadList[positionCube].transform.position.x,
                  gameObject.transform.position.y + QadList[positionCube].GetComponent<Collider>().bounds.extents.y + offset,
                  QadList[positionCube].transform.position.z));
    }

    public void SpawnEnemies()
    {
        enemyPieces[0].GetComponent<EnemyControler>().Spawn(new Vector3(QadList[positionTriangle].transform.position.x,
           gameObject.transform.position.y + QadList[positionTriangle].GetComponent<Collider>().bounds.extents.y + offset,
           QadList[positionTriangle].transform.position.z));

        enemyPieces[1].GetComponent<EnemyControler>().Spawn(new Vector3(QadList[positionDiamond].transform.position.x,
            gameObject.transform.position.y + QadList[positionDiamond].GetComponent<Collider>().bounds.extents.y + offset,
            QadList[positionDiamond].transform.position.z));

        enemyPieces[2].GetComponent<EnemyControler>().Spawn(new Vector3(QadList[positionCube].transform.position.x,
            gameObject.transform.position.y + QadList[positionCube].GetComponent<Collider>().bounds.extents.y + offset,
            QadList[positionCube].transform.position.z));

        activeEnemyPieces = GameObject.FindGameObjectsWithTag("EnemyPiece");
    }


    public bool ControlEnemies()
    {
        foreach (GameObject eP in activeEnemyPieces)
        {
            Debug.Log(eP.name);
            eP.GetComponent<EnemyControler>().FindSelectableQads();
            eP.GetComponent<EnemyControler>().MoveEnemy();
        }

        return true;

    }

    void CheckHover()
    {
        foreach (GameObject piece in playerPieces)
        {
            if (piece.GetComponent<HoverControl>().hover)
            {
                Debug.Log("HOVER !");
            }
        }
    }

}
