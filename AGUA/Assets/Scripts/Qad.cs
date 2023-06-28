using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Qad : MonoBehaviour
{



    public bool walkable = true;
    public bool current = false;
    public bool target = false;
    public bool selectable = false;
    public bool attacked = false;
    public bool diagonalToCurrent = false;
    public bool playerAttack = false;


    public float qadDamage = 0;

    //Stores every adjacent Qad for each Qad
    public List<Qad> adjacencyList = new List<Qad>();
    //Qad in front (0)
    //Qad in back (1)
    //Qad in right (2)
    //Qad in left (3)


    //bfs variables 
    public bool visited = false;
    public Qad qParent = null;
    public int distance = 0;
    public bool corner = false;

    //layer
    public LayerMask layerQad;

    //Position inside Grid
    public bool row = false;
    public bool col = false;

    //hovering
    QadHoverScript hover;


    // Start is called before the first frame update
    void Start()
    {
        hover = GetComponent<QadHoverScript>();

        qadDamage = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (current) GetComponent<Renderer>().material.color = Color.magenta;
        else if (target) GetComponent<Renderer>().material.color = Color.green;
        else if (selectable) GetComponent<Renderer>().material.color = Color.cyan;
        else if (attacked) GetComponent<Renderer>().material.color = Color.red;
        else if (playerAttack) GetComponent<Renderer>().material.color = Color.yellow;
        else GetComponent<Renderer>().material.color = Color.white;
    }

    public void SetDamageInQad(float _damage)
    {
        qadDamage = 0;
        qadDamage = _damage;
    }

    public bool GetHover()
    {
        return hover.IsHover();
    }

    public void Reset()
    {
        walkable = true;
        current = false;
        target = false;
        selectable = false;
        attacked = false;
        diagonalToCurrent = false;

        visited = false;
        qParent = null;
        distance = 0;


        adjacencyList.Clear();
    }

    //Pathfinding 
    public void FindNeighbors(float jumpHeight, bool checkForMoves, PlayerPieceControler currentPiece)
    {
        Reset();

        CheckQads(Vector3.forward, jumpHeight, checkForMoves, currentPiece);
        CheckQads(-Vector3.forward, jumpHeight, checkForMoves, currentPiece);
        CheckQads(Vector3.right, jumpHeight, checkForMoves, currentPiece);
        CheckQads(-Vector3.right, jumpHeight, checkForMoves, currentPiece);
    }


    public void CheckQads(Vector3 direction, float jumpHeight, bool checkForMoves, PlayerPieceControler currentPiece)
    {
        Vector3 halfExtents = new Vector3(0.25f, (1 + jumpHeight) / 2.0f, 0.25f);
        Collider[] colliders = Physics.OverlapBox(transform.position + direction, halfExtents, Quaternion.identity, layerQad);

        foreach (Collider col in colliders)
        {
            Qad qad = col.GetComponent<Qad>();
            if (qad != null)
            {
                RaycastHit hit;
                if (!Physics.Raycast(qad.transform.position, Vector3.up, out hit, 1))
                {
                    adjacencyList.Add(qad);
                }
                else
                {
                    if (hit.collider.gameObject.tag == "EnemyPiece")
                    {
                        adjacencyList.Add(qad);
                        walkable = false;
                    }
                    if (hit.collider.gameObject.tag == "PlayerPiece")
                    {
                        adjacencyList.Add(qad);
                        walkable = false;
                    }
                }
            }
        }
    }



}
