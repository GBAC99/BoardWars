using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverControl : MonoBehaviour
{

    private Renderer rend;

    public bool hover;

    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<Renderer>();
    }

    private void OnMouseEnter()
    {
        rend.material.color = Color.red;
        hover = true;
    }

    private void OnMouseExit()
    {
        rend.material.color = Color.white;
        hover = false;

    }

}
