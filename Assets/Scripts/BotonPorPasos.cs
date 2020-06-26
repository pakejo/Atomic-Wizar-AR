using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BotonPorPasos : MonoBehaviour
{
    private bool activated = false;
    private GameObject button, probeText;
    public Controlador controlador;


    // Start is called before the first frame update
    void Start()
    {
        controlador = (Controlador)FindObjectOfType(typeof(Controlador));
    }


    // Update is called once per frame
    void Update()
    {

    }

    public void changeDrawMode()
    {
        button = GameObject.Find("PorPasos");
        probeText = GameObject.Find("PorPasosText");

        if (!activated)
        {
            this.controlador.setModoDibujado(ModoDibujado.PASO_A_PASO);
            button.GetComponent<Image>().color = Color.green;
            probeText.GetComponent<Text>().color = new Color32(32, 24, 229, 255);
            activated = true;
        }
        else
        {
            this.controlador.setModoDibujado(ModoDibujado.AUTOMATICO);
            button.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
            probeText.GetComponent<Text>().color = new Color32(0, 0, 0, 255);
            activated = false;

        }
    }
}

