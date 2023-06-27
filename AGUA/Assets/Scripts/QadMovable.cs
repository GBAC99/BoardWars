using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QadMovable : MonoBehaviour
{

    public float healthPoints;
    [SerializeField]
    public bool alive;

    public float damage;

    protected QadManager qadManager;

    public float jumpHeight = 1;
    public float moveSpeed = 2;
    public int move = 2;
    public int attackRange = 2;
    int VorH = 0;
    public bool moving = false;
    public List<Qad> selectableQads = new List<Qad>();
    public List<Qad> attackingQads = new List<Qad>();

    protected GameObject[] QadList;
    public Stack<Qad> path = new Stack<Qad>();
    public Qad currentQad;

    Vector3 velocity = new Vector3();
    Vector3 heading = new Vector3();

    float halfHeight = 0;

    public enum CharacterT
    {
        ENEMY_HORIZONTAL,
        ENEMY_VERTICAL,
        ENEMY_HORSE,
        PLAYER_ARROW,
        PLAYER_SCHYTE,
        PLAYER_HAMMMER,
        PLAYER_SHIP
    }

    public CharacterT characterType;

    public void Init()
    {
        qadManager = FindObjectOfType<QadManager>();
        QadList = qadManager.GetQadList(); 
        alive = true;
    }


    private void Update()
    {

    }

    //Pathfinding Movement
    public void GetCurrentQad()//Gets the quad under the piece
    {
        
        currentQad = GetTargetQad(gameObject);
        currentQad.current = true;
        currentQad.walkable = false;

    }

    public Qad GetTargetQad(GameObject target)
    {
        RaycastHit hit;
        Qad qad = null;

        if (Physics.Raycast(target.transform.position, -Vector3.up, out hit, 1))
        {
            if (hit.collider.gameObject.tag == "Qad")
            {
                qad = hit.collider.GetComponent<Qad>();
            }
        }
        return qad;
    }

    public void ComputeAdjacencyLists(bool checkForMoves) //Computes all adjacent Qads to each Qad
    {
        foreach (GameObject qad in QadList)
        {
            Qad q = qad.GetComponent<Qad>();
            q.FindNeighbors(jumpHeight, checkForMoves, gameObject.GetComponent<PlayerPieceControler>());
        }
    }

    public int GetCurrentQadIndex()
    {
        for (int i = 0; i < QadList.Length; i++)
        {
            if (QadList[i].GetComponent<Qad>().current)
            {
                return i;
            }
        }

        return 0;
    }

    public int GetQadIndex(GameObject q)
    {
        for (int i = 0; i < QadList.Length; i++)
        {
            if (QadList[i] == q)
            {
                return i;
            }
        }

        return 0;
    }

    //BFS Algorithm
    /* There are quite a bunch of posibilites when trying to reach selectable Qads for each type of movement.
     * There are 4 possible cases: A Qad can have from 1 to 4 adjacent quads.
     * A Qad cannot select as adjacent the currentQad Qad.
     * Those adjacent Qads will not always be reachable, so they have to be processed in different ways. 
     * 
     * Then, theres another major condition that will modify the requirements for a qad to be selectable.
     * A Piece can be placed in a Corner, inside the Upper or Bottom Row and inside the left or right Column.
     * 
     */

    //Moving IA
    public void FindSelectableQads()
    {
        Debug.Log("finding selectable qads"); 
        ComputeAdjacencyLists(true); 
        GetCurrentQad();
        Queue<Qad> process = new Queue<Qad>(); 

        process.Enqueue(currentQad);
        currentQad.visited = true;

        while (process.Count > 0)
        {
            Qad q = process.Dequeue();

            if (q != null && !q.current)
            {
                selectableQads.Add(q);
                q.selectable = true;
            }

            if (q.distance < move)
            {
                foreach (Qad qad in q.adjacencyList)
                {
                    if (!qad.visited)
                    {
                        switch (characterType)
                        {
                            case CharacterT.ENEMY_HORIZONTAL:

                                if (q.adjacencyList.Count == 4)
                                {
                                    if (q.adjacencyList[2] == qad || q.adjacencyList[3] == qad)
                                    {
                                        qad.qParent = q;
                                        qad.visited = true; //it is processed
                                        qad.distance = 1 + q.distance;
                                        process.Enqueue(qad);
                                    }
                                }

                                if (q.adjacencyList.Count == 3)
                                {
                                    if (currentQad.row)
                                    {
                                        if (q.adjacencyList[1] == qad || q.adjacencyList[2] == qad)
                                        {
                                            qad.qParent = q;
                                            qad.visited = true; //it is processed
                                            qad.distance = 1 + q.distance;
                                            process.Enqueue(qad);
                                        }
                                    }
                                    else
                                    {

                                        if ((q.adjacencyList[2] == qad))
                                        {
                                            qad.qParent = q;
                                            qad.visited = true; //it is processed
                                            qad.distance = 1 + q.distance;
                                            process.Enqueue(qad);
                                        }

                                    }


                                }

                                if (q.adjacencyList.Count == 2)
                                {
                                    if (currentQad.corner)
                                    {
                                        if (q.adjacencyList[1] == qad)
                                        {
                                            qad.qParent = q;
                                            qad.visited = true; //it is processed
                                            qad.distance = 1 + q.distance;
                                            process.Enqueue(qad);
                                        }
                                    }
                                    if (currentQad.row)
                                    {
                                        if (q.adjacencyList[1] == qad)
                                        {
                                            qad.qParent = q;
                                            qad.visited = true; //it is processed
                                            qad.distance = 1 + q.distance;
                                            process.Enqueue(qad);
                                        }
                                    }
                                }

                                break;

                            case CharacterT.ENEMY_VERTICAL:

                                if (q.adjacencyList.Count == 4)
                                {
                                    if ((q.adjacencyList[0] == qad || q.adjacencyList[1] == qad))
                                    {
                                        qad.qParent = q;
                                        qad.visited = true; //it is processed
                                        qad.distance = 1 + q.distance;
                                        process.Enqueue(qad);
                                    }
                                }

                                if (q.adjacencyList.Count == 3)
                                {
                                    //Conditions if the character starts the movement in the inital or final column of the grid.
                                    if (currentQad.col)
                                    {
                                        if ((q.adjacencyList[0] == qad || q.adjacencyList[1] == qad))//Check qads index
                                        {
                                            qad.qParent = q;
                                            qad.visited = true; //it is processed
                                            qad.distance = 1 + q.distance;
                                            process.Enqueue(qad);
                                        }
                                    }
                                    else
                                    {
                                        if ((q.adjacencyList[0] == qad))
                                        {
                                            qad.qParent = q;
                                            qad.visited = true; //it is processed
                                            qad.distance = 1 + q.distance;
                                            process.Enqueue(qad);
                                        }
                                    }
                                }

                                if (q.adjacencyList.Count == 2)
                                {
                                    if (currentQad.corner && q.adjacencyList[0] == qad)
                                    {
                                        qad.qParent = q;
                                        qad.visited = true; //it is processed
                                        qad.distance = 1 + q.distance;
                                        process.Enqueue(qad);
                                    }
                                    if (currentQad.col && q.adjacencyList[0] == qad)
                                    {
                                        qad.qParent = q;
                                        qad.visited = true; //it is processed
                                        qad.distance = 1 + q.distance;
                                        process.Enqueue(qad);
                                    }
                                }

                                break;

                            case CharacterT.ENEMY_HORSE:

                                if (q.adjacencyList.Count == 4)
                                {
                                    qad.qParent = q;
                                    qad.visited = true; //it is processed
                                    qad.distance = 1 + q.distance;
                                    process.Enqueue(qad);

                                }

                                if (q.adjacencyList.Count == 3)
                                {
                                    if (currentQad.col || currentQad.row)
                                    {
                                        if (currentQad.row)
                                        {
                                            if (q.adjacencyList[1] == qad || q.adjacencyList[2] == qad)
                                            {
                                                qad.qParent = q;
                                                qad.visited = true; //it is processed
                                                qad.distance = 1 + q.distance;
                                                process.Enqueue(qad);
                                            }
                                        }
                                        else if (currentQad.col)
                                        {
                                            if (q.adjacencyList[1] == qad || q.adjacencyList[0] == qad)
                                            {
                                                qad.qParent = q;
                                                qad.visited = true; //it is processed
                                                qad.distance = 1 + q.distance;
                                                process.Enqueue(qad);
                                            }
                                        }
                                        else
                                        {
                                            if (q.adjacencyList[1] == qad || q.adjacencyList[2] == qad)
                                            {
                                                qad.qParent = q;
                                                qad.visited = true; //it is processed
                                                qad.distance = 1 + q.distance;
                                                process.Enqueue(qad);
                                            }
                                        }


                                    }
                                    else
                                    {
                                        if ((q.adjacencyList[1] == qad))
                                        {
                                            qad.qParent = q;
                                            qad.visited = true; //it is processed
                                            qad.distance = 1 + q.distance;
                                            process.Enqueue(qad);
                                        }
                                        if (((q.adjacencyList[2] == qad)) && (GetCurrentQadIndex() - GetQadIndex(q.gameObject) == 1))
                                        {
                                            qad.qParent = q;
                                            qad.visited = true; //it is processed
                                            qad.distance = 1 + q.distance;
                                            process.Enqueue(qad);
                                        }
                                    }



                                }

                                if (q.adjacencyList.Count == 2)
                                {
                                    if (currentQad.corner)
                                    {
                                        if (q.current)
                                        {
                                            qad.qParent = q;
                                            qad.visited = true; //it is processed
                                            qad.distance = 1 + q.distance;
                                            process.Enqueue(qad);
                                        }
                                        else
                                        {
                                            if (q.col && (q.adjacencyList[1] == qad))
                                            {
                                                qad.qParent = q;
                                                qad.visited = true; //it is processed
                                                qad.distance = 1 + q.distance;
                                                process.Enqueue(qad);
                                            }
                                            else if (q.row && (q.adjacencyList[0] == qad))
                                            {
                                                qad.qParent = q;
                                                qad.visited = true; //it is processed
                                                qad.distance = 1 + q.distance;
                                                process.Enqueue(qad);
                                            }
                                        }
                                    }

                                    if (currentQad.row && q.adjacencyList[0] == qad)
                                    {
                                        qad.qParent = q;
                                        qad.visited = true; //it is processed
                                        qad.distance = 1 + q.distance;
                                        process.Enqueue(qad);
                                    }

                                    if (currentQad.col && q.adjacencyList[1] == qad)
                                    {
                                        qad.qParent = q;
                                        qad.visited = true; //it is processed
                                        qad.distance = 1 + q.distance;
                                        process.Enqueue(qad);
                                    }

                                    if ((!currentQad.col && !currentQad.row && !currentQad.corner))
                                    {
                                        qad.qParent = q;
                                        qad.visited = true; //it is processed
                                        qad.distance = 1 + q.distance;
                                        process.Enqueue(qad);
                                    }
                                }

                                if (q.adjacencyList.Count == 1 && !qad.current)
                                {
                                    qad.qParent = q;
                                    qad.visited = true; //it is processed
                                    qad.distance = 1 + q.distance;
                                    process.Enqueue(qad);
                                }

                                break;

                            case CharacterT.PLAYER_ARROW:

                                if (q.adjacencyList.Count == 4)
                                {
                                    if ((q.adjacencyList[0] == qad || q.adjacencyList[1] == qad))
                                    {
                                        qad.qParent = q;
                                        qad.visited = true; //it is processed
                                        qad.distance = 1 + q.distance;
                                        process.Enqueue(qad);
                                    }
                                }

                                if (q.adjacencyList.Count == 3)
                                {
                                    //Conditions if the character starts the movement in the inital or final column of the grid.
                                    if (currentQad.col)
                                    {
                                        if ((q.adjacencyList[0] == qad || q.adjacencyList[1] == qad))//Check qads index
                                        {
                                            qad.qParent = q;
                                            qad.visited = true; //it is processed
                                            qad.distance = 1 + q.distance;
                                            process.Enqueue(qad);
                                        }
                                    }
                                    else
                                    {
                                        if ((q.adjacencyList[0] == qad))
                                        {
                                            qad.qParent = q;
                                            qad.visited = true; //it is processed
                                            qad.distance = 1 + q.distance;
                                            process.Enqueue(qad);
                                        }
                                    }
                                }

                                if (q.adjacencyList.Count == 2)
                                {

                                    if (currentQad.corner && q.adjacencyList[0] == qad)
                                    {
                                        qad.qParent = q;
                                        qad.visited = true; //it is processed
                                        qad.distance = 1 + q.distance;
                                        process.Enqueue(qad);
                                    }
                                    else if (currentQad.col && q.adjacencyList[0] == qad)
                                    {
                                        qad.qParent = q;
                                        qad.visited = true; //it is processed
                                        qad.distance = 1 + q.distance;
                                        process.Enqueue(qad);
                                    }
                                }

                                break;
                            case CharacterT.PLAYER_HAMMMER:

                                if (q.adjacencyList.Count == 4)
                                {
                                    qad.qParent = q;
                                    qad.visited = true; //it is processed
                                    qad.distance = 1 + q.distance;
                                    process.Enqueue(qad);

                                }

                                if (q.adjacencyList.Count == 3)
                                {
                                    if (currentQad.col || currentQad.row)
                                    {
                                        if (currentQad.row)
                                        {
                                            if (q.adjacencyList[1] == qad || q.adjacencyList[2] == qad)
                                            {
                                                qad.qParent = q;
                                                qad.visited = true; //it is processed
                                                qad.distance = 1 + q.distance;
                                                process.Enqueue(qad);
                                            }
                                        }
                                        else if (currentQad.col)
                                        {
                                            if (q.adjacencyList[1] == qad || q.adjacencyList[0] == qad)
                                            {
                                                qad.qParent = q;
                                                qad.visited = true; //it is processed
                                                qad.distance = 1 + q.distance;
                                                process.Enqueue(qad);
                                            }
                                        }
                                        else
                                        {
                                            if (q.adjacencyList[1] == qad || q.adjacencyList[2] == qad)
                                            {
                                                qad.qParent = q;
                                                qad.visited = true; //it is processed
                                                qad.distance = 1 + q.distance;
                                                process.Enqueue(qad);
                                            }
                                        }


                                    }
                                    else
                                    {
                                        if ((q.adjacencyList[1] == qad))
                                        {
                                            qad.qParent = q;
                                            qad.visited = true; //it is processed
                                            qad.distance = 1 + q.distance;
                                            process.Enqueue(qad);
                                        }
                                        if (((q.adjacencyList[2] == qad)) && (GetCurrentQadIndex() - GetQadIndex(q.gameObject) == 1))
                                        {
                                            qad.qParent = q;
                                            qad.visited = true; //it is processed
                                            qad.distance = 1 + q.distance;
                                            process.Enqueue(qad);
                                        }
                                    }



                                }

                                if (q.adjacencyList.Count == 2)
                                {
                                    if (currentQad.corner)
                                    {
                                        if (q.current)
                                        {
                                            qad.qParent = q;
                                            qad.visited = true; //it is processed
                                            qad.distance = 1 + q.distance;
                                            process.Enqueue(qad);
                                        }
                                        else
                                        {
                                            if (q.col && (q.adjacencyList[1] == qad))
                                            {
                                                qad.qParent = q;
                                                qad.visited = true; //it is processed
                                                qad.distance = 1 + q.distance;
                                                process.Enqueue(qad);
                                            }
                                            else if (q.row && (q.adjacencyList[0] == qad))
                                            {
                                                qad.qParent = q;
                                                qad.visited = true; //it is processed
                                                qad.distance = 1 + q.distance;
                                                process.Enqueue(qad);
                                            }
                                        }
                                    }

                                    if (currentQad.row && q.adjacencyList[0] == qad)
                                    {
                                        qad.qParent = q;
                                        qad.visited = true; //it is processed
                                        qad.distance = 1 + q.distance;
                                        process.Enqueue(qad);
                                    }

                                    if (currentQad.col && q.adjacencyList[1] == qad)
                                    {
                                        qad.qParent = q;
                                        qad.visited = true; //it is processed
                                        qad.distance = 1 + q.distance;
                                        process.Enqueue(qad);
                                    }

                                    if ((!currentQad.col && !currentQad.row && !currentQad.corner))
                                    {
                                        qad.qParent = q;
                                        qad.visited = true; //it is processed
                                        qad.distance = 1 + q.distance;
                                        process.Enqueue(qad);
                                    }
                                }

                                if (q.adjacencyList.Count == 1 && !qad.current)
                                {
                                    qad.qParent = q;
                                    qad.visited = true; //it is processed
                                    qad.distance = 1 + q.distance;
                                    process.Enqueue(qad);
                                }
                                break;
                            case CharacterT.PLAYER_SCHYTE:

                                if (q.adjacencyList.Count == 4)
                                {
                                    qad.qParent = q;
                                    qad.visited = true; //it is processed
                                    qad.distance = 1 + q.distance;
                                    process.Enqueue(qad);

                                }

                                if (q.adjacencyList.Count == 3)
                                {
                                    if (currentQad.col || currentQad.row)
                                    {
                                        if (currentQad.row)
                                        {
                                            if (q.adjacencyList[1] == qad || q.adjacencyList[2] == qad)
                                            {
                                                qad.qParent = q;
                                                qad.visited = true; //it is processed
                                                qad.distance = 1 + q.distance;
                                                process.Enqueue(qad);
                                            }
                                        }
                                        else if (currentQad.col)
                                        {
                                            if (q.adjacencyList[1] == qad || q.adjacencyList[0] == qad)
                                            {
                                                qad.qParent = q;
                                                qad.visited = true; //it is processed
                                                qad.distance = 1 + q.distance;
                                                process.Enqueue(qad);
                                            }
                                        }
                                        else
                                        {
                                            if (q.adjacencyList[1] == qad || q.adjacencyList[2] == qad)
                                            {
                                                qad.qParent = q;
                                                qad.visited = true; //it is processed
                                                qad.distance = 1 + q.distance;
                                                process.Enqueue(qad);
                                            }
                                        }


                                    }
                                    else
                                    {
                                        if ((q.adjacencyList[1] == qad))
                                        {
                                            qad.qParent = q;
                                            qad.visited = true; //it is processed
                                            qad.distance = 1 + q.distance;
                                            process.Enqueue(qad);
                                        }
                                        if (((q.adjacencyList[2] == qad)) && (GetCurrentQadIndex() - GetQadIndex(q.gameObject) == 1))
                                        {
                                            qad.qParent = q;
                                            qad.visited = true; //it is processed
                                            qad.distance = 1 + q.distance;
                                            process.Enqueue(qad);
                                        }
                                    }



                                }

                                if (q.adjacencyList.Count == 2)
                                {
                                    if (currentQad.corner)
                                    {
                                        if (q.current)
                                        {
                                            qad.qParent = q;
                                            qad.visited = true; //it is processed
                                            qad.distance = 1 + q.distance;
                                            process.Enqueue(qad);
                                        }
                                        else
                                        {
                                            if (q.col && (q.adjacencyList[1] == qad))
                                            {
                                                qad.qParent = q;
                                                qad.visited = true; //it is processed
                                                qad.distance = 1 + q.distance;
                                                process.Enqueue(qad);
                                            }
                                            else if (q.row && (q.adjacencyList[0] == qad))
                                            {
                                                qad.qParent = q;
                                                qad.visited = true; //it is processed
                                                qad.distance = 1 + q.distance;
                                                process.Enqueue(qad);
                                            }
                                        }
                                    }

                                    if (currentQad.row && q.adjacencyList[0] == qad)
                                    {
                                        qad.qParent = q;
                                        qad.visited = true; //it is processed
                                        qad.distance = 1 + q.distance;
                                        process.Enqueue(qad);
                                    }

                                    if (currentQad.col && q.adjacencyList[1] == qad)
                                    {
                                        qad.qParent = q;
                                        qad.visited = true; //it is processed
                                        qad.distance = 1 + q.distance;
                                        process.Enqueue(qad);
                                    }

                                    if ((!currentQad.col && !currentQad.row && !currentQad.corner))
                                    {
                                        qad.qParent = q;
                                        qad.visited = true; //it is processed
                                        qad.distance = 1 + q.distance;
                                        process.Enqueue(qad);
                                    }
                                }

                                if (q.adjacencyList.Count == 1 && !qad.current)
                                {
                                    qad.qParent = q;
                                    qad.visited = true; //it is processed
                                    qad.distance = 1 + q.distance;
                                    process.Enqueue(qad);
                                }
                                break;
                            case CharacterT.PLAYER_SHIP:

                                if (q.adjacencyList.Count == 4)
                                {
                                    if (q.adjacencyList[1] == qad)
                                    {
                                        qad.qParent = q;
                                        qad.visited = true; //it is processed
                                        qad.distance = 1 + q.distance;
                                        process.Enqueue(qad);
                                    }
                                }

                                if (q.adjacencyList.Count == 3)
                                {
                                    //Conditions if the character starts the movement in the inital or final column of the grid.
                                    if (currentQad.col)
                                    {
                                        if (q.adjacencyList[0] == qad)//Check qads index
                                        {
                                            qad.qParent = q;
                                            qad.visited = true; //it is processed
                                            qad.distance = 1 + q.distance;
                                            process.Enqueue(qad);
                                        }
                                    }
                                    else
                                    {
                                        if ((q.adjacencyList[0] == qad))
                                        {
                                            qad.qParent = q;
                                            qad.visited = true; //it is processed
                                            qad.distance = 1 + q.distance;
                                            process.Enqueue(qad);
                                        }
                                    }
                                }

                                if (q.adjacencyList.Count == 2)
                                {
                                    if (currentQad.corner && q.adjacencyList[0] == qad)
                                    {
                                        qad.qParent = q;
                                        qad.visited = true; //it is processed
                                        qad.distance = 1 + q.distance;
                                        process.Enqueue(qad);
                                    }
                                    if (currentQad.col && q.adjacencyList[0] == qad)
                                    {
                                        qad.qParent = q;
                                        qad.visited = true; //it is processed
                                        qad.distance = 1 + q.distance;
                                        process.Enqueue(qad);
                                    }
                                }
                                break;
                        }
                    }
                }
            }
        }

        foreach (Qad q in selectableQads)
        {
            RaycastHit _hit;

            if (Physics.Raycast(q.transform.position, Vector3.up, out _hit, 1))
            {
                q.selectable = false;
            }
        }


    }

    public void ShowSelectableQads()
    {
        foreach (Qad q in selectableQads)
        {
            q.selectable = true;
        }
    }

    public void MoveToQad(Qad qad) //Sets the target Qad
    {
        path.Clear();
        qad.target = true;
        moving = true;

        Qad next = qad;
        while (next != null)
        {
            path.Push(next);
            next = next.qParent;
        }

    }

    public void Move() //Moves objects to desired qad
    {
        if (path.Count > 0)
        {
            Qad q = path.Peek();
            Vector3 targetPos = q.transform.position;

            targetPos.y += halfHeight + q.GetComponent<Collider>().bounds.extents.y + 0.05f;

            if (Vector3.Distance(transform.position, targetPos) >= 0.05f)
            {
                CalculateHeading(targetPos);
                SetHorizontalVelocity();

                transform.forward = heading;
                transform.position += velocity * Time.deltaTime;
            }
            else
            {
                transform.position = targetPos;
                path.Pop();
            }

        }
        else
        {
            Debug.Log("Stop moving");
            ResetTiles();

            moving = false;
        }
    }

    public void ResetTiles()
    {

        if (currentQad != null)
        {
            currentQad.current = false;
            currentQad = null;
        }

        foreach (Qad q in selectableQads)
        {
            q.Reset();
        }

        selectableQads.Clear();


    }

    public void ResetLists()
    {

        foreach (Qad q in selectableQads)
        {
            q.Reset();
        }
        foreach (Qad q in attackingQads)
        {
            q.Reset();
        }

        selectableQads.Clear();
        attackingQads.Clear();
    }

    void CalculateHeading(Vector3 targetPos)
    {
        heading = targetPos - transform.position;
        heading.Normalize();

    }

    void SetHorizontalVelocity()
    {
        velocity = heading * moveSpeed;
    }

    //Attacking IA

    /* 
     * Works really similar to FindSelectableQads with some other special conditions.
     * To a later easier use of the attacking qads, it is needed to process a new kind of Qad:
     * the Diagonal To Current Qad.
     * */

    public void FindAttackingQads()
    {
        ComputeAdjacencyLists(false);
        GetCurrentQad();
        Queue<Qad> process = new Queue<Qad>();

        process.Enqueue(currentQad);
        currentQad.visited = true;

        //local declaration for random movements
        VorH = Random.Range(0, 2);

        while (process.Count > 0)
        {
            Qad q = process.Dequeue();

            if (!q.current)
            {
                // Filter for diagonal Qad cases and exact selections
                switch (characterType)
                {
                    case CharacterT.ENEMY_VERTICAL:

                        if (q.diagonalToCurrent)
                        {
                            q.attacked = true;
                            attackingQads.Add(q);
                            qadManager.AddtoEnemyAttackQads(q, damage);
                        }

                        else q.attacked = false;

                        break;
                    case CharacterT.ENEMY_HORIZONTAL:
                        attackingQads.Add(q);
                        qadManager.AddtoEnemyAttackQads(q, damage);


                        break;
                    case CharacterT.ENEMY_HORSE:
                        attackingQads.Add(q);
                        qadManager.AddtoEnemyAttackQads(q, damage);


                        break;

                    case CharacterT.PLAYER_ARROW:

                        if (q.diagonalToCurrent)
                        {
                            q.attacked = true;
                            attackingQads.Add(q);
                        }

                        else q.attacked = false;

                        break;

                    case CharacterT.PLAYER_SCHYTE:

                        if (q.distance == 2)
                        {
                            q.attacked = true;
                            attackingQads.Add(q);
                        }

                        break;

                    case CharacterT.PLAYER_HAMMMER:
                        q.attacked = true;

                        attackingQads.Add(q);
                        break;
                    case CharacterT.PLAYER_SHIP:
                        q.attacked = true;

                        attackingQads.Add(q);
                        break;

                }
            }

            // checks if a qad can be attacked
            if (q.distance < attackRange)
            {
                foreach (Qad qad in q.adjacencyList)
                {
                    if (!qad.visited)
                    {
                        switch (characterType)
                        {
                            case CharacterT.ENEMY_HORIZONTAL:

                                qad.qParent = q;
                                qad.visited = true; //it is processed
                                qad.distance = 1 + q.distance;
                                process.Enqueue(qad);

                                break;
                            case CharacterT.ENEMY_VERTICAL:

                                qad.qParent = q;
                                qad.visited = true; //it is processed
                                qad.distance = 1 + q.distance;
                                process.Enqueue(qad);

                                //Checks for diagonal qads to the current qad
                                if ((GetQadIndex(qad.gameObject) - GetCurrentQadIndex()) == (qadManager.y - 1) * 2)
                                    qad.diagonalToCurrent = true;

                                if ((GetCurrentQadIndex() - GetQadIndex(qad.gameObject)) == (qadManager.y + 1) * 2)
                                    qad.diagonalToCurrent = true;

                                if ((GetQadIndex(qad.gameObject) - GetCurrentQadIndex()) == qadManager.y - 1)
                                    qad.diagonalToCurrent = true;

                                if ((GetCurrentQadIndex() - GetQadIndex(qad.gameObject)) == qadManager.y + 1)
                                    qad.diagonalToCurrent = true;

                                break;
                            case CharacterT.ENEMY_HORSE:

                                // V == 0 | H == 1
                                // Random selection of the attacking Qads.
                                // By design purposes, it is always a 50% chance to one of the two sides.
                                if (q.adjacencyList.Count != 1)
                                {
                                    if (VorH == 0)
                                    {
                                        if (currentQad.corner)
                                        {
                                            if (q.adjacencyList[0] == qad)
                                            {
                                                qad.qParent = q;
                                                qad.visited = true; //it is processed
                                                qad.distance = 1 + q.distance;
                                                process.Enqueue(qad);
                                            }
                                        }
                                        else if (currentQad.row)
                                        {
                                            if (q.adjacencyList[0] == qad)
                                            {
                                                qad.qParent = q;
                                                qad.visited = true; //it is processed
                                                qad.distance = 1 + q.distance;
                                                process.Enqueue(qad);
                                            }

                                        }
                                        else if (currentQad.col)
                                        {
                                            if (q.adjacencyList.Count == 2 && q.adjacencyList[0] == qad)
                                            {
                                                qad.qParent = q;
                                                qad.visited = true; //it is processed
                                                qad.distance = 1 + q.distance;
                                                process.Enqueue(qad);
                                            }
                                            else if (q.adjacencyList.Count == 3)
                                            {
                                                if (q.adjacencyList[1] == qad || q.adjacencyList[0] == qad)
                                                {
                                                    qad.qParent = q;
                                                    qad.visited = true; //it is processed
                                                    qad.distance = 1 + q.distance;
                                                    process.Enqueue(qad);
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (q.adjacencyList.Count == 4)
                                            {
                                                if (q.adjacencyList[0] == qad || q.adjacencyList[1] == qad)
                                                {
                                                    qad.qParent = q;
                                                    qad.visited = true; //it is processed
                                                    qad.distance = 1 + q.distance;
                                                    process.Enqueue(qad);
                                                }
                                            }

                                            if (q.adjacencyList.Count == 3)
                                            {
                                                if (q.adjacencyList[0] == qad)
                                                {
                                                    qad.qParent = q;
                                                    qad.visited = true; //it is processed
                                                    qad.distance = 1 + q.distance;
                                                    process.Enqueue(qad);
                                                }

                                            }
                                        }

                                    }

                                    if (VorH == 1)
                                    {
                                        if (currentQad.corner)
                                        {
                                            if (q.adjacencyList[1] == qad)
                                            {
                                                qad.qParent = q;
                                                qad.visited = true; //it is processed
                                                qad.distance = 1 + q.distance;
                                                process.Enqueue(qad);
                                            }
                                        }
                                        else if (currentQad.col)
                                        {
                                            if (q.adjacencyList[0] == qad)
                                            {
                                                qad.qParent = q;
                                                qad.visited = true; //it is processed
                                                qad.distance = 1 + q.distance;
                                                process.Enqueue(qad);
                                            }

                                        }
                                        else if (currentQad.row)
                                        {
                                            if (q.adjacencyList.Count == 2 && q.adjacencyList[1] == qad)
                                            {
                                                qad.qParent = q;
                                                qad.visited = true; //it is processed
                                                qad.distance = 1 + q.distance;
                                                process.Enqueue(qad);
                                            }
                                            else if (q.adjacencyList.Count == 3)
                                            {
                                                if (q.adjacencyList[2] == qad || q.adjacencyList[1] == qad)
                                                {
                                                    qad.qParent = q;
                                                    qad.visited = true; //it is processed
                                                    qad.distance = 1 + q.distance;
                                                    process.Enqueue(qad);
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (q.adjacencyList.Count == 4)
                                            {
                                                if (q.adjacencyList[2] == qad || q.adjacencyList[3] == qad)
                                                {
                                                    qad.qParent = q;
                                                    qad.visited = true; //it is processed
                                                    qad.distance = 1 + q.distance;
                                                    process.Enqueue(qad);
                                                }
                                            }

                                            if (q.adjacencyList.Count == 3)
                                            {
                                                if (q.adjacencyList[2] == qad)
                                                {
                                                    qad.qParent = q;
                                                    qad.visited = true; //it is processed
                                                    qad.distance = 1 + q.distance;
                                                    process.Enqueue(qad);
                                                }

                                            }
                                        }
                                    }
                                }


                                break;
                            case CharacterT.PLAYER_ARROW: // SEGUNDA Y TERCERA CASILLA EN DIAGONAL -- PLAYER INPUT

                                qad.qParent = q;
                                qad.visited = true; //it is processed
                                qad.distance = 1 + q.distance;
                                process.Enqueue(qad);

                                //Checks for diagonal qads to the current qad
                                if ((GetQadIndex(qad.gameObject) - GetCurrentQadIndex()) == (qadManager.y - 1) * 2)//10
                                {

                                    qad.diagonalToCurrent = true;

                                    if (!qad.row && !qad.col)
                                    {
                                        //Diagonal extra qad
                                        Qad dQad = QadList[GetQadIndex(qad.gameObject) + (qadManager.y - 1)].GetComponent<Qad>();

                                        dQad.qParent = q;
                                        dQad.visited = true; //it is processed.
                                        dQad.diagonalToCurrent = true;
                                        dQad.attacked = true;
                                        attackingQads.Add(dQad);
                                    }
                                }


                                if ((GetQadIndex(qad.gameObject) - GetCurrentQadIndex()) == (qadManager.y - 1) * 2 * -1)//-10
                                {

                                    qad.diagonalToCurrent = true;

                                    if (!qad.row && !qad.col)
                                    {
                                        //Diagonal extra qad
                                        Qad dQad = QadList[GetQadIndex(qad.gameObject) - (qadManager.y - 1)].GetComponent<Qad>();

                                        dQad.qParent = q;
                                        dQad.visited = true; //it is processed.
                                        dQad.diagonalToCurrent = true;
                                        dQad.attacked = true;
                                        attackingQads.Add(dQad);
                                    }
                                }

                                if (((GetCurrentQadIndex() - GetQadIndex(qad.gameObject)) == (qadManager.y + 1) * 2 * -1))//-14
                                {
                                    qad.diagonalToCurrent = true;


                                    if (!qad.row && !qad.col)
                                    {
                                        //Diagonal extra qad
                                        Qad dQad = QadList[GetQadIndex(qad.gameObject) + qadManager.y + 1].GetComponent<Qad>();

                                        dQad.qParent = q;
                                        dQad.visited = true; //it is processed.
                                        dQad.diagonalToCurrent = true;
                                        dQad.attacked = true;
                                        attackingQads.Add(dQad);
                                    }
                                }

                                if (((GetCurrentQadIndex() - GetQadIndex(qad.gameObject)) == (qadManager.y + 1) * 2))//14
                                {
                                    qad.diagonalToCurrent = true;


                                    if (!qad.row && !qad.col)
                                    {
                                        //Diagonal extra qad
                                        Qad dQad = QadList[GetQadIndex(qad.gameObject) - (qadManager.y + 1)].GetComponent<Qad>();

                                        int result = GetQadIndex(qad.gameObject) - qadManager.y + 1;


                                        dQad.qParent = q;
                                        dQad.visited = true; //it is processed.
                                        dQad.diagonalToCurrent = true;
                                        dQad.attacked = true;
                                    }
                                }

                                break;

                            case CharacterT.PLAYER_SCHYTE: // SEGUNDA CASILLA DE SU LADO -- PLAYER INPUT?

                                if (currentQad.corner)
                                {
                                    if (q.adjacencyList.Count == 2)
                                    {
                                        if (q.adjacencyList[1] == qad)
                                        {
                                            qad.qParent = q;
                                            qad.visited = true; //it is processed
                                            qad.distance = 1 + q.distance;
                                            process.Enqueue(qad);
                                        }
                                    }
                                }
                                else if (currentQad.col)
                                {
                                    if (q.adjacencyList.Count == 3)
                                    {
                                        if (q.adjacencyList[2] == qad)
                                        {
                                            qad.qParent = q;
                                            qad.visited = true; //it is processed
                                            qad.distance = 1 + q.distance;
                                            process.Enqueue(qad);
                                        }
                                    }
                                }
                                else if (currentQad.row)
                                {
                                    if (q.adjacencyList.Count == 3)
                                    {
                                        if (q.adjacencyList[2] == qad || q.adjacencyList[1] == qad)
                                        {
                                            qad.qParent = q;
                                            qad.visited = true; //it is processed
                                            qad.distance = 1 + q.distance;
                                            process.Enqueue(qad);
                                        }
                                    }
                                    else if (q.adjacencyList.Count == 2)
                                    {
                                        if (q.adjacencyList[1] == qad)
                                        {
                                            qad.qParent = q;
                                            qad.visited = true; //it is processed
                                            qad.distance = 1 + q.distance;
                                            process.Enqueue(qad);
                                        }
                                    }


                                }
                                else
                                {
                                    if (q.adjacencyList.Count == 3)
                                    {
                                        if (q.adjacencyList[2] == qad)
                                        {
                                            qad.qParent = q;
                                            qad.visited = true; //it is processed
                                            qad.distance = 1 + q.distance;
                                            process.Enqueue(qad);
                                        }
                                    }
                                    else if (q.adjacencyList.Count == 4)
                                    {
                                        if (q.adjacencyList[2] == qad || q.adjacencyList[3] == qad)
                                        {
                                            qad.qParent = q;
                                            qad.visited = true; //it is processed
                                            qad.distance = 1 + q.distance;
                                            process.Enqueue(qad);
                                        }
                                    }

                                }

                                break;
                            case CharacterT.PLAYER_HAMMMER: // CASILLA DE ENCIMA SUYO

                                if (currentQad.corner)
                                {
                                    if (q.adjacencyList[0] == qad)
                                    {
                                        qad.qParent = q;
                                        qad.visited = true; //it is processed
                                        qad.distance = 1 + q.distance;
                                        process.Enqueue(qad);
                                    }
                                }
                                else if (currentQad.col)
                                {
                                    if (q.adjacencyList[1] == qad)
                                    {
                                        qad.qParent = q;
                                        qad.visited = true; //it is processed
                                        qad.distance = 1 + q.distance;
                                        process.Enqueue(qad);
                                    }
                                }
                                else if (currentQad.row)
                                {
                                    if (q.adjacencyList[0] == qad)
                                    {
                                        qad.qParent = q;
                                        qad.visited = true; //it is processed
                                        qad.distance = 1 + q.distance;
                                        process.Enqueue(qad);
                                    }
                                }
                                else
                                {
                                    if (q.adjacencyList[1] == qad)
                                    {
                                        qad.qParent = q;
                                        qad.visited = true; //it is processed
                                        qad.distance = 1 + q.distance;
                                        process.Enqueue(qad);
                                    }
                                }


                                break;
                            case CharacterT.PLAYER_SHIP: // CASILLA DE AL LADO -- PLAYER INPUT

                                if (currentQad.corner)
                                {
                                    if (q.adjacencyList[1] == qad)
                                    {
                                        qad.qParent = q;
                                        qad.visited = true; //it is processed
                                        qad.distance = 1 + q.distance;
                                        process.Enqueue(qad);
                                    }
                                }
                                else if (currentQad.col)
                                {
                                    if (q.adjacencyList[2] == qad)
                                    {
                                        qad.qParent = q;
                                        qad.visited = true; //it is processed
                                        qad.distance = 1 + q.distance;
                                        process.Enqueue(qad);
                                    }
                                }
                                else if (currentQad.row)
                                {
                                    if (q.adjacencyList[1] == qad || q.adjacencyList[2] == qad)
                                    {
                                        qad.qParent = q;
                                        qad.visited = true; //it is processed
                                        qad.distance = 1 + q.distance;
                                        process.Enqueue(qad);
                                    }
                                }
                                else
                                {
                                    if (q.adjacencyList[2] == qad || q.adjacencyList[3] == qad)
                                    {
                                        qad.qParent = q;
                                        qad.visited = true; //it is processed
                                        qad.distance = 1 + q.distance;
                                        process.Enqueue(qad);
                                    }
                                }

                                break;
                        }
                    }
                }
            }
        }
    }



   
}
