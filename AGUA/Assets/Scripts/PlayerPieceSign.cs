using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPieceSign : MonoBehaviour
{

    public GameObject piece;

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


    public Renderer rend;
    private PlayerPieceSign thisPpSign;

    [ColorUsage(true, true)]
    public Color hoverColor;

    public bool hover;

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
            rend.material.color = Color.gray;
        }

    }

    public GameObject getPieceSign()
    {
        return piece;
    }


    private void OnMouseEnter()
    {
        if (selectable)
        {
            rend.material.color = hoverColor;
            hover = true;
        }
        else
        {

            rend.material.color = Color.black;
        }
    }

    private void OnMouseExit()
    {
        if (selectable)
        {
            rend.material.color = Color.white;
            hover = true;
        }
        else
        {
            rend.material.color = Color.gray;
        }
    }

}
