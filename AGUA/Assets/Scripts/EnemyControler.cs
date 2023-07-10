using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControler : QadMovable
{
    EnemyControler thisEnemy;
    public float currentHealthPoints;
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
            if (alive)
            {
                currentQad.GetComponent<Animator>().SetBool("KILL", true);
                alive = false;
            }
        }


    }

    public override void ManageTags(bool current)
    {
        if (current) gameObject.tag = "CurrentEnemyPiece";
        else gameObject.tag = "EnemyPiece";
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
                    targetPosition = 0;

                }
                else if (currentQad.col)
                {
                    targetPosition = 0;
                }
                else
                {
                    if (selectableQads.Count < 2)
                    {
                        targetPosition = 0;
                    }
                    else
                    {
                        targetPosition = Random.Range(0, 2);
                    }
                }
                break;

            case CharacterT.ENEMY_VERTICAL:

                if (currentQad.corner)
                {
                    targetPosition = 0;

                }
                else if (currentQad.row)
                {
                    targetPosition = 0;
                }
                else
                {
                    if (selectableQads.Count < 2)
                    {
                        targetPosition = 0;
                    }

                    else
                    {
                        targetPosition = Random.Range(0, 2);
                    }
                }
                break;

            case CharacterT.ENEMY_HORSE:

                if (currentQad.corner)
                {
                    targetPosition = 0;
                }
                else
                {
                    targetPosition = Random.Range(0, 2);
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
