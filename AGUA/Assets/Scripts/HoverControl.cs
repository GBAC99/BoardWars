using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverControl : MonoBehaviour
{

    private Renderer rend;
    private PlayerPieceSign thisPpSign;

    [ColorUsage(true, true)]
    public Color hoverColor;

    public bool hover;

    public bool selectable;

    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<Renderer>(); 
        selectable = true;
    }

    private void Update()
    {
        if (!selectable)
        {
            rend.material.color = Color.gray;
        }
        
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
