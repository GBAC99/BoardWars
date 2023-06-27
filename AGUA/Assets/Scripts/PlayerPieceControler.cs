using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPieceControler : QadMovable
{
    public PlayerPieceControler thisPPlayer;
    float currentHealthPoints;

    // Start is called before the first frame update
    void Start()
    {
        Init();
        currentHealthPoints = healthPoints;
        thisPPlayer = gameObject.GetComponent<PlayerPieceControler>();
        //FindSelectableQads(); 
        //FindAttackingQads(); 
    }

    // Update is called once per frame
    void Update()
    {
        if (moving) Move();

        if (currentHealthPoints <= 0 )
        {
            gameObject.SetActive(false);
        }

    }

    public void Spawn(Vector3 spawnPos)
    {
        Instantiate(gameObject, spawnPos, Quaternion.identity, null);
    }

    public void SetAttackQad(Qad q)
    {
        qadManager.AddtoPlayerAttackQads(q, damage);
    }

    public void TakeDamage()
    {
        currentHealthPoints -= currentQad.qadDamage;
    }

}
