using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QadHoverScript : MonoBehaviour
{
    [ColorUsage(true, true)]
    public Color hoverColor;
    public Material hoveringMat;
    public float scaleHover;

    private Renderer hoverRenderer;

    public  GameObject cloneQad;

    // Start is called before the first frame update
    void Start()
    {
        hoverRenderer = CreateOutline(hoveringMat, scaleHover, hoverColor);
    }
    Renderer CreateOutline(Material outlineMat, float scaleFactor, Color color)
    {

        GameObject outlineObject = Instantiate(cloneQad, transform.position, transform.rotation, transform);
        Renderer rend = outlineObject.GetComponent<Renderer>();
        rend.material = outlineMat;
        rend.material.SetColor("_OutlineColor", color);
        rend.material.SetFloat("_ScaleFactor", scaleFactor);
        rend.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        outlineObject.GetComponent<Collider>().enabled = false;
        outlineObject.GetComponent<QadHoverScript>().enabled = false;
        outlineObject.GetComponent<Qad>().enabled = false;
        outlineObject.GetComponent<Animator>().enabled = false;
        outlineObject.tag = "Untagged";
        outlineObject.layer = 0 ;
        rend.enabled = false;

        return rend;
    }

    private void OnMouseEnter()
    {
        hoverRenderer.enabled = true;
    }

    private void OnMouseOver()
    {

    }

    private void OnMouseExit()
    {
        hoverRenderer.enabled = false;
    }


}
