using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class HPBarControler : MonoBehaviour
{
    public Image hpBar;

    float maxFillAmount;
    float currentFillAmount;

    [Header("Player is 0 | Enemy is 1")]
    public int enemyOrPlayer;

    public GameObject piece;

    private void Start()
    {
        if (enemyOrPlayer == 0) maxFillAmount = piece.GetComponent<PlayerPieceControler>().healthPoints / 100;
        else if (enemyOrPlayer == 1) maxFillAmount = piece.GetComponent<EnemyControler>().healthPoints / 100;

    }

    // Update is called once per frame
    void Update()
    {
        if (enemyOrPlayer == 0) currentFillAmount = piece.GetComponent<PlayerPieceControler>().currentHealthPoints / 100;
        else if (enemyOrPlayer == 1) currentFillAmount = piece.GetComponent<EnemyControler>().currentHealthPoints / 100;

        hpBar.fillAmount = currentFillAmount/maxFillAmount;

    }
}
