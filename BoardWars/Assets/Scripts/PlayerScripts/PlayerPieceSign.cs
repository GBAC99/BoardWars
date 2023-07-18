using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPieceSign : MonoBehaviour
{

    public GameObject piece;
    public PlayerPieceControler attachedPpc;
    public int activeNumPiece;

    public float pieceCurrentHealth;

    public enum CharacterT
    {
        PLAYER_ARROW,
        PLAYER_SCHYTE,
        PLAYER_HAMMMER,
        PLAYER_SHIP
    }

    public CharacterT characterType;

    public bool usingPiece;

    public GameObject signInstance;

    public Renderer rend;
    private PlayerPieceSign thisPpSign;

    [ColorUsage(true, true)]
    public Color hoverColor;

    public bool hover;

    public bool signPlaced;

    public bool selectable;

     
    // Start is called before the first frame update
    void Start()
    {
        selectable = true;
    }

    private void Update()
    {
        if (!selectable)
        {
            gameObject.SetActive(false);
        }

        if (usingPiece)
        {
            rend.material.color = Color.gray;
        }

        if (attachedPpc != null)
        {
            pieceCurrentHealth = attachedPpc.currentHealthPoints;
            if (attachedPpc.alive == false)
            {
                selectable = false;
            }
        }

        
    }

    public GameObject getPieceSign()
    {
        return piece;
    }

    public int GetPieceNum()
    {
        return activeNumPiece;
    }

    public PlayerPieceSign Spawn(PlayerPieceSign pps,Transform position)
    {
        Debug.Log("sign");
        return Instantiate(signInstance.GetComponentInChildren<PlayerPieceSign>(), position);
    }

    public void SetCurrentPieceHp(PlayerPieceControler ppC)
    {
        attachedPpc = ppC;
    }
}
