using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControler : QadMovable
{
    EnemyControler thisEnemy;

    // Start is called before the first frame update
    void Start()
    {
        Init();
        thisEnemy = gameObject.GetComponent<EnemyControler>();
        FindSelectableQads();
        FindAttackingQads();
    }

    // Update is called once per frame
    void Update()
    {
        if (moving) Move();


        if (Input.GetKeyDown(KeyCode.Space))
        {
            MoveEnemy();
        }

    }

    public void Spawn(Vector3 spawnPos)
    {
        Instantiate(gameObject, spawnPos, Quaternion.identity, null);
    }

    public void MoveEnemy()
    {
        //Definir valor dependiendo de si es horse, vertical o horizontal

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
                        targetPosition = 2;
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
                    targetPosition = 2;
                }
                else
                {
                    if (selectableQads.Count < 4)
                    {
                        targetPosition = 2;
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

}
