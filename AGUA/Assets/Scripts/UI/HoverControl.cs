using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverControl : MonoBehaviour
{

    public Renderer rend;
    Renderer outlineRend;
    public Material outlineMaterial;
    public float scaleOutline;
    [ColorUsage(true, true)]
    public Color hoverColor;


    public GameLoopControler gC;


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

        outlineRend = CreateOutline(outlineMaterial, scaleOutline, hoverColor);
        outlineRend.enabled = false;

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

    Renderer CreateOutline(Material outlineMat, float scaleFactor, Color color)
    {
        GameObject outlineObject = Instantiate(gameObject, transform.position, transform.rotation, transform);
        outlineObject.transform.localScale = new Vector3(1 * scaleFactor, 1 * scaleFactor, 1 * scaleFactor);
        Renderer rend = outlineObject.GetComponent<HoverControl>().rend;
        rend.material = outlineMat;
        rend.material.SetColor("_OutlineColor", color);
        rend.material.SetFloat("_ScaleFactor", scaleFactor);
        rend.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        outlineObject.GetComponent<Collider>().enabled = false;
        outlineObject.GetComponent<HoverControl>().enabled = false;

        if (characterType[0] == 'p') outlineObject.GetComponent<PlayerPieceSign>().enabled = false;
        else
        {
            outlineObject.GetComponent<EnemyControler>().enabled = false;
            outlineObject.GetComponentInChildren<Canvas>().enabled = false;
        }


        outlineObject.tag = "Untagged";
        outlineObject.layer = 0;
        rend.enabled = false;

        return rend;
    }
    private void OnMouseEnter()
    {
        timeToHideInfo = 1.5f;
        outlineRend.enabled = true; 
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
        outlineRend.enabled = false; 
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
