using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QadManager : MonoBehaviour
{

    public CameraControler mainCam;

    public PlayerControler Player;
    Vector3 PlayerInitPos;

    public GameObject Qad;
    public int x;
    public int y;

    public GameObject[] QadList;
    float halfHeight = 0;

    public Transform QadSpawnPos;

    public LayerMask layerQad;

    [HideInInspector]
    public Vector3 mousePosition;

    // Start is called before the first frame update
    void Start()
    {
        CreateMap();
        PlayerInitPos = new Vector3(QadList[15].gameObject.transform.position.x
            , gameObject.transform.position.y + QadList[0].GetComponent<Collider>().bounds.extents.y + 0.05f
            , QadList[15].gameObject.transform.position.z);

        Player.GetComponent<PlayerControler>().StartPlayer(PlayerInitPos);
        Player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControler>();

        mainCam = Instantiate(mainCam);

    }

    //CREAR MAPA
    /*
     *          5 11 17 23 29 35  -  Up Row
     *   i      4 10 16 22 28 34    
     *   n c    3 9  15 21 27 33    e c
     *   i o    2 8  14 20 26 32    n o  
     *   t l    1 7  13 19 25 31    d l
     *          0 6  12 18 24 30  -  Down Row
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
    //CREAR MAPA

    public Qad SearchQadList(GameObject QadCheck)
    {
        Qad QadFind = null;

        for (int i = 0; i < QadList.Length; i++)
        {
            if (QadCheck.gameObject == QadList[i].gameObject)
            {
                QadFind = QadList[i].gameObject.GetComponent<Qad>();
                Debug.Log(QadFind);
            }
        }
        return QadFind;

    }

}
