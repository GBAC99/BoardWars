using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverControl : MonoBehaviour
{

    public Renderer rend;

    public GameLoopControler gC;


    [ColorUsage(true, true)]
    public Color hoverColor;

    public Color initColor;

    public bool hover;
    public bool showInfo;
    public string characterType;
    float timeToShowUI = 1.2f;
    float timeToHideInfo = 0.5f;

    public bool selectable;

    // Start is called before the first frame update
    void Start()
    {
        gC = FindObjectOfType<GameLoopControler>();

        if (rend == null)
        {
            rend = GetComponentInChildren<Renderer>();
            rend.material.color = Color.white;
        }
        initColor = rend.material.color;


        selectable = true;
    }

    private void Update()
    {
        if (!selectable)
        {
            rend.material.color = Color.gray;
        }

        if (!showInfo)
        {
            if (timeToHideInfo >= 0)
            {
                timeToHideInfo -= Time.deltaTime;
            }
            else
            {
                showInfo = true;
                HighLight(false);
            }
        }
    }

    private void OnMouseEnter()
    {
        timeToHideInfo = 1.5f;
        rend.material.color = hoverColor;
        hover = true;
        showInfo = true;
        
    }

    private void OnMouseOver()
    {
        if (timeToShowUI >= 0)
        { 
            timeToShowUI -= Time.deltaTime;
        }
        else
        {
            HighLight(true);
        }
    }

    

    private void OnMouseExit()
    {
        timeToShowUI = 1.5f;
        rend.material.color = initColor;
        hover = false;
        showInfo = false;
    }

    //Enables the panel with the information of the piece
    void HighLight(bool on)
    {
        if (on)
        {
            gC.ShowInfo(characterType);
        }
        else
        {
            gC.HideInfo(characterType);
        }
    }


}
