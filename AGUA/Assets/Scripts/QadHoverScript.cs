using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QadHoverScript : MonoBehaviour
{
    [ColorUsage(true, true)]
    public Color hoverColor;
    public Material hoveringMat;
    public float scaleHover;

    bool hover;

    private Renderer hoverRenderer;

    // Start is called before the first frame update
    void Start()
    { 
        hoverRenderer = CreateOutline(hoveringMat, scaleHover, hoverColor);
    }
    Renderer CreateOutline(Material outlineMat, float scaleFactor, Color color)
    {

        GameObject outlineObject = Instantiate(this.gameObject, transform.position, transform.rotation, transform);
        Renderer rend = outlineObject.GetComponent<Renderer>();
        rend.material = outlineMat;
        rend.material.SetColor("_OutlineColor", color);
        rend.material.SetFloat("_ScaleFactor", scaleFactor);
        rend.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        outlineObject.GetComponent<QadHoverScript>().enabled = false;
        outlineObject.GetComponent<Collider>().enabled = false;
        rend.enabled = false;

        return rend;
    }

    private void OnMouseEnter()
    {
        hoverRenderer.enabled = true;
    }

    private void OnMouseOver()
    {
        hover = true;
    }

    private void OnMouseExit()
    {
        hoverRenderer.enabled = false;
        hover = false;
    }

    public bool IsHover()
    {
        return hover;
    }

}
