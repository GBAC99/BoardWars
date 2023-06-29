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

    Vector3 startPosition;
    Vector3 upPosition;

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

        startPosition = transform.position;

        upPosition = new Vector3(startPosition.x, startPosition.y + 0.13f, startPosition.z);

    }

    private void Update()
    {
        if (!selectable)
        {
            gameObject.SetActive(false);
        }

        if (attachedPpc != null)
        {
            pieceCurrentHealth = attachedPpc.currentHealthPoints;
        }


        /* if (hover)
         {
             MoveUp();
         }
         else
         {
             if (transform.position != startPosition)
             {
                 MoveInit();
             }
         }*/
    }

    public GameObject getPieceSign()
    {
        return piece;
    }

    public PlayerPieceSign Spawn(PlayerPieceSign pps,Transform position)
    { 
        return Instantiate(pps, position);
    }

    public void SetCurrentPieceHp(PlayerPieceControler ppC)
    {
        attachedPpc = ppC;
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
            hover = false;
        }
        else
        {
            rend.material.color = Color.gray;
        }
    }

    void MoveUp()
    {
        transform.position = Vector3.MoveTowards(transform.position, upPosition, 2 * Time.deltaTime);
    }

    void MoveInit()
    {
        transform.position = Vector3.MoveTowards(transform.position, startPosition, 1.5f * Time.deltaTime);
    }
}
