using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPieceControler : QadMovable
{
    public PlayerPieceControler thisPPlayer;

    // Start is called before the first frame update
    void Start()
    {
        Init();
        thisPPlayer = gameObject.GetComponent<PlayerPieceControler>();
        //FindSelectableQads();
        //FindAttackingQads();
    }

    // Update is called once per frame
    void Update()
    {
        if (moving) Move();
    }

    public GameObject Spawn(Vector3 spawnPos)
    {
        return Instantiate(gameObject, spawnPos, Quaternion.identity, null);
    }

}
