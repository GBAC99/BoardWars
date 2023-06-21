using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverControl : MonoBehaviour
{

    private Renderer rend;

    [ColorUsage(true, true)]
    public Color hoverColor;

    public bool hover;

    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<Renderer>();
    }

    private void OnMouseEnter()
    {
        rend.material.color = hoverColor;
        hover = true;
    }

    private void OnMouseExit()
    {
        rend.material.color = Color.white;
        hover = false;

    }

}
