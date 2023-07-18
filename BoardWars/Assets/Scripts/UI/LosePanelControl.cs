using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LosePanelControl : MonoBehaviour
{
     
    public TextMeshProUGUI YOULOSE;
     public TextMeshProUGUI RESTARTWAR;

    public float capDilate;

    private float startDilate;

    public float actualDilate;

    public float speedDilate;

    // Start is called before the first frame update
    void Start()
    {
        startDilate = -0.5f;
        actualDilate = startDilate;
    }

    // Update is called once per frame
    void Update()
    {
        if (actualDilate < capDilate)
        {
            actualDilate += Time.deltaTime * speedDilate;
        }
        else
        {
            actualDilate = capDilate;
        }

        YOULOSE.fontMaterial.SetFloat(ShaderUtilities.ID_FaceDilate, actualDilate);
        RESTARTWAR.fontMaterial.SetFloat(ShaderUtilities.ID_FaceDilate, actualDilate);
    }

}
