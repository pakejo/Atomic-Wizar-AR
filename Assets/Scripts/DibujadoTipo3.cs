using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DibujadoTipo3 : TipoDibujado
{
    public DibujadoTipo3(Controlador controlador) : base(controlador)
    {
    }

    public override IEnumerator Dibuja(List<string> formula, ModoDibujado modoDibujado)
    {
        //Paso 1: Dibujar elemento de menor numero
        int a = Convert.ToInt32(formula[1]);
        int b = Convert.ToInt32(formula[3]);
        int n_1;
        int n_2;
        int aux;
        string primer_elemento_a_crear;
        string segundo_elemento_a_crear;

        if (a <= b)
        {
            n_1 = a;
            n_2 = b;
            primer_elemento_a_crear = formula[0];
            segundo_elemento_a_crear = formula[2];
        }
        else
        {
            n_1 = b;
            n_2 = a;
            primer_elemento_a_crear = formula[2];
            segundo_elemento_a_crear = formula[0];
        }

        float radio = this.controlador.crearElemento(primer_elemento_a_crear, controlador.Object_B);
        float radio_esfera_1 = radio;

        //Obtenenmos su referencia para trabajar sobre el resto de esferas
        GameObject primera = controlador.Object_B.transform.GetChild(0).gameObject;
        primera.transform.position = new Vector3(controlador.Object_B.transform.position.x, controlador.Object_B.transform.position.y + 0.5f, controlador.Object_B.transform.position.z);

        //Añadimos las esferas restantes del mismo tipo
        for (int i = 1; i < n_1; i++)
        {
            radio = controlador.crearElemento(primer_elemento_a_crear, primera);
            primera.transform.GetChild(i - 1).transform.position = new Vector3(primera.transform.position.x, primera.transform.position.y, primera.transform.position.z);
            primera.transform.GetChild(i - 1).Translate(radio / 2, 0, 0);
        }

        if (modoDibujado == ModoDibujado.PASO_A_PASO)
            yield return new WaitForSecondsRealtime(4.0f);

        // Añadimos el segundo tipo
        aux = n_1 - 1;

        for (int i = 0; i < n_2; i++)
        {
            radio = controlador.crearElemento(segundo_elemento_a_crear, primera);
            primera.transform.GetChild(i + aux).transform.position = new Vector3(primera.transform.position.x, primera.transform.position.y, primera.transform.position.z);
            primera.transform.GetChild(i + aux).Translate(-(radio_esfera_1 / 2) - (radio / 3), 0, 0);
            primera.transform.GetChild(i + aux).transform.RotateAround(primera.transform.position, new Vector3(0, 1, 0), (360.0f / n_2) * i);
        }
    }
}
