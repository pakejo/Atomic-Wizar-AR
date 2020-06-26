using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DibujadoTipo1 : TipoDibujado
{
    public DibujadoTipo1(Controlador controlador) : base(controlador)
    {
    }

    public override IEnumerator Dibuja(List<string> formula, ModoDibujado modoDibujado)
    {
        string elementoCentral = formula[0];
        string hidrogeno = "H";
        int numeroAtomosHidrogeno = Convert.ToInt32(formula[3]);

        // Obtenemos referencia de la carta
        GameObject cartaJuego = this.controlador.Object_B.gameObject;
        Vector3 posicionCartaJuego = new Vector3(cartaJuego.transform.position.x, cartaJuego.transform.position.y, cartaJuego.transform.position.z);

        // Creamos y colocamos el elemento central
        float radioElementoCentral = this.controlador.crearElemento(elementoCentral, cartaJuego);
        GameObject gameObjectElementoCentral = cartaJuego.transform.GetChild(0).gameObject;
        gameObjectElementoCentral.transform.position = new Vector3(posicionCartaJuego.x, posicionCartaJuego.y + 0.5f, posicionCartaJuego.z);

        if (modoDibujado == ModoDibujado.PASO_A_PASO)
            yield return new WaitForSecondsRealtime(4.0f);

        // Ya tenemos el elemento central, ahora colocamos los hidrogenos
        // La forma de colorcalors es: si tiene 3 o menos atomos de hidrogenos se colocan en circulo
        // Si tiene mas, los 3 primeros se colocan igua y los restantes en los extremos
        float radioHidrogeno;
        GameObject gameObjectAux;

        if (numeroAtomosHidrogeno <= 3)
        {
            for(int i = 0; i < numeroAtomosHidrogeno; i++)
            {
                radioHidrogeno = controlador.crearElemento(hidrogeno, gameObjectElementoCentral);
                gameObjectAux = gameObjectElementoCentral.transform.GetChild(i).gameObject;
                gameObjectAux.transform.position = new Vector3(gameObjectElementoCentral.transform.position.x, gameObjectElementoCentral.transform.position.y, gameObjectElementoCentral.transform.position.z);
                gameObjectAux.transform.Translate(radioElementoCentral / 2.5f, 0, 0);
                gameObjectAux.transform.RotateAround(gameObjectElementoCentral.transform.position, new Vector3(0, 0, 1), -20);
                gameObjectAux.transform.RotateAround(gameObjectElementoCentral.transform.position, new Vector3(0, 1, 0), (360 / numeroAtomosHidrogeno) * i);
            }
        }
        else if (numeroAtomosHidrogeno > 3)
        {
            for (int i = 0; i < 3; i++)
            {
                radioHidrogeno = controlador.crearElemento(hidrogeno, gameObjectElementoCentral);
                gameObjectAux = gameObjectElementoCentral.transform.GetChild(i).gameObject;
                gameObjectAux.transform.position = new Vector3(gameObjectElementoCentral.transform.position.x, gameObjectElementoCentral.transform.position.y, gameObjectElementoCentral.transform.position.z);
                gameObjectAux.transform.Translate(radioElementoCentral / 2.5f, 0, 0);
                gameObjectAux.transform.RotateAround(gameObjectElementoCentral.transform.position, new Vector3(0, 0, 1), -20);
                gameObjectAux.transform.RotateAround(gameObjectElementoCentral.transform.position, new Vector3(0, 1, 0), (360 / numeroAtomosHidrogeno) * i);
            }

            radioHidrogeno = controlador.crearElemento(hidrogeno, gameObjectElementoCentral);
            gameObjectAux = gameObjectElementoCentral.transform.GetChild(numeroAtomosHidrogeno - 1).gameObject;
            gameObjectAux.transform.position = new Vector3(gameObjectElementoCentral.transform.position.x, gameObjectElementoCentral.transform.position.y, gameObjectElementoCentral.transform.position.z);
            gameObjectAux.transform.Translate(0, radioElementoCentral / 2.5f, 0);
        }

        if (modoDibujado == ModoDibujado.PASO_A_PASO)
            yield return new WaitForSecondsRealtime(4.0f);
    }
}
