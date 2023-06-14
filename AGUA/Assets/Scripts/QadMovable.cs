using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QadMovable : MonoBehaviour
{

    protected QadManager qadManager;

    public float jumpHeight = 1;
    public float moveSpeed = 2;
    public int move = 5;
    public bool moving = false;
    List<Qad> selectableQads = new List<Qad>();

    protected GameObject[] QadList;

    public Stack<Qad> path = new Stack<Qad>();
    public Qad currentQad;

    Vector3 velocity = new Vector3();
    Vector3 heading = new Vector3();

    float halfHeight = 0;

    public enum CharacterT
    {
        ENEMY_VERTICAL,
        ENEMY_HORIZONTAL,
        ENEMY_HORSE
    }

    public CharacterT characterType;


    public void Init()
    {
        qadManager = FindObjectOfType<QadManager>();
        QadList = qadManager.QadList;
    }

    //Pathfinding Movement
    public void GetCurrentQad()
    {
        currentQad = GetTargetQad(gameObject);
        currentQad.current = true;
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

    public void ComputeAdjacencyLists() //Computes all adjacent Qads to each Qad
    {
        foreach (GameObject qad in QadList)
        {
            Qad q = qad.GetComponent<Qad>();
            q.FindNeighbors(jumpHeight);
        }
    }
    public void ComputeCustomAdjacencyLists()
    {
        switch (characterType)
        {
            case CharacterT.ENEMY_VERTICAL:
                break;
            case CharacterT.ENEMY_HORIZONTAL:
                for (int i = 0; i < QadList.Length; i++)
                {
                    QadList[i].GetComponent<Qad>().FindDesiredNeighbors("Front", jumpHeight);
                    QadList[i].GetComponent<Qad>().FindDesiredNeighbors("Back", jumpHeight);
                }
                break;
            case CharacterT.ENEMY_HORSE:
                break;
            default:
                break;
        }
    }

    //BFS algorithm


    /* There are quite a bunch of posibilites when trying to reach selectable Qads for each type of movement.
     * There are 4 possible cases: A Qad can have from 1 to 4 adjacent quads.
     * A Qad cannot select as adjacent the currentQad Qad.
     * Those adjacent Qads will not always be reachable, so they have to be processed in different ways. 
     */

    public void FindSelectableQads()
    {
        ComputeAdjacencyLists();
        GetCurrentQad();
        Queue<Qad> process = new Queue<Qad>();

        process.Enqueue(currentQad);
        currentQad.visited = true;

        while (process.Count > 0)
        {
            Qad q = process.Dequeue();

            selectableQads.Add(q);
            q.selectable = true;

            if (q.distance < move)
            {
                foreach (Qad qad in q.adjacencyList)
                {
                    switch (characterType)
                    {
                        case CharacterT.ENEMY_VERTICAL:

                            if (!qad.visited && q.adjacencyList.Count == 4)
                            {
                                if (q.adjacencyList[2] == qad || q.adjacencyList[3] == qad)
                                {
                                    qad.qParent = q;
                                    qad.visited = true; //it is processed
                                    qad.distance = 1 + q.distance;
                                    process.Enqueue(qad);
                                }
                            }

                            if (!qad.visited && q.adjacencyList.Count == 3)
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

                        case CharacterT.ENEMY_HORIZONTAL:

                            if (q.adjacencyList.Count == 4)
                            {
                                if (!qad.visited && (q.adjacencyList[0] == qad || q.adjacencyList[1] == qad))
                                {
                                    qad.qParent = q;
                                    qad.visited = true; //it is processed
                                    qad.distance = 1 + q.distance;
                                    process.Enqueue(qad);
                                }
                            }

                            if (!qad.visited && q.adjacencyList.Count == 3)
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

                            if (!qad.visited && q.adjacencyList.Count == 2)
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

                            if (!qad.visited && q.adjacencyList.Count == 4)
                            {
                                qad.qParent = q;
                                qad.visited = true; //it is processed
                                qad.distance = 1 + q.distance;
                                process.Enqueue(qad);

                            }

                            if (!qad.visited && q.adjacencyList.Count == 3)
                            {
                                if (currentQad.col || currentQad.row)
                                {
                                    if (currentQad.col || currentQad.row)
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

                            if (!qad.visited && q.adjacencyList.Count == 2)
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
                                            Debug.Log("Row");
                                            qad.qParent = q;
                                            qad.visited = true; //it is processed
                                            qad.distance = 1 + q.distance;
                                            process.Enqueue(qad);
                                        }
                                    }
                                }

                                if ((currentQad.col || currentQad.row) && q.adjacencyList[0] == qad)
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

                            if (q.adjacencyList.Count == 1 && !qad.visited && !qad.current)
                            {
                                qad.qParent = q;
                                qad.visited = true; //it is processed
                                qad.distance = 1 + q.distance;
                                process.Enqueue(qad);
                            }

                            break;
                    }
                }
            }
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

    public void FindCustomSelectableQads()
    {
        ComputeAdjacencyLists();
        GetCurrentQad();

        Queue<Qad> process = new Queue<Qad>();

        process.Enqueue(currentQad);
        currentQad.visited = true;

        while (process.Count > 0)
        {
            Qad q = process.Dequeue();

            selectableQads.Add(q);
            q.selectable = true;

            if (q.distance < move)
            {
                foreach (Qad qad in q.adjacencyList)
                {
                    if (!qad.visited)
                    {
                        qad.qParent = q;
                        qad.visited = true;
                        qad.distance = 1 + q.distance;
                        process.Enqueue(qad);
                    }
                }
            }
        }
    }


    public void MoveToQad(Qad qad)
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
            RemoveSelectableTiles();
            FindSelectableQads();
            moving = false;
        }
    }
    public void CustomMove() //Moves objects to desired qad
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
            RemoveSelectableTiles();
            FindCustomSelectableQads();
            moving = false;
        }
    }

    protected void RemoveSelectableTiles()
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

    //Moving the object
    void CalculateHeading(Vector3 targetPos)
    {
        heading = targetPos - transform.position;
        heading.Normalize();

    }

    void SetHorizontalVelocity()
    {
        velocity = heading * moveSpeed;
    }

}
