using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControler : QadMovable
{
    EnemyControler thisEnemy;
    float currentHealthPoints;
    // Start is called before the first frame update
    void Start()
    {
        Init();
        currentHealthPoints = healthPoints;
        thisEnemy = gameObject.GetComponent<EnemyControler>(); 
    }

    // Update is called once per frame
    void Update()
    {
        if (moving) Move();

        if (currentHealthPoints <= 0)
        {
            gameObject.SetActive(false);
            alive = false;
        }


    }

    public void Spawn(Vector3 spawnPos)
    {
        Instantiate(gameObject, spawnPos, Quaternion.identity, null);
    }

    public void MoveEnemy()
    { 

        int targetPosition = 0;

        switch (characterType)
        {
            case CharacterT.ENEMY_HORIZONTAL:

                if (currentQad.corner)
                {
                    targetPosition = 1;

                }
                else if (currentQad.col)
                {
                    targetPosition = 2;
                }
                else
                {
                    if (selectableQads.Count < 4)
                    {
                        targetPosition = 1;
                    }
                    else
                    {
                        targetPosition = Random.Range(2, 4);
                    }
                }
                break;

            case CharacterT.ENEMY_VERTICAL:

                if (currentQad.corner)
                {
                    targetPosition = 1;

                }
                else if (currentQad.row)
                {
                    targetPosition = 1;
                }
                else
                {
                    if (selectableQads.Count < 4 && selectableQads.Count > 0)
                    {
                        targetPosition = 1;
                    }
                    
                    else
                    {
                        targetPosition = Random.Range(2, 4);
                    }
                }
                break;

            case CharacterT.ENEMY_HORSE:

                if (currentQad.corner)
                {
                    targetPosition = selectableQads.Count - 1;
                }
                else if (currentQad.row || currentQad.col)
                {
                    targetPosition = Random.Range(2, 4);
                }
                else
                {
                    targetPosition = Random.Range(4, 6);
                }
                break;

        }
         
        MoveToQad(selectableQads[targetPosition]); 


    }

    public void TakeDamage()
    {
        currentHealthPoints -= currentQad.qadDamage;
    }

}
