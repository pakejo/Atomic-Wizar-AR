﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class Controlador : MonoBehaviour
{
    // Objetos de la escena
    private GameObject object_A;
    private GameObject object_B;
    public string input;


    private int cont = 0;
    private bool esta_cerca = false;

    // Start is called before the first frame update
    void Start()
    {
        // Obtenemos los objetos que representan a las cartas
        object_A = this.transform.GetChild(0).gameObject;
        object_B = this.transform.GetChild(1).gameObject;

    }

    // Update is called once per frame
    void Update()
    {
        // Si los dos objetos están presentes comprobar la distancias de ambos
        /*if (IsOnScene(object_A) || IsOnScene(object_B))
        {
            DetectObjectsPosition(ref object_A, ref object_B);
        }*/

        if (IsOnScene(object_A))
            CopyObjects();
        

        List<string> formula = readFormula("FeS");
    }

    // Comprueba si un objeto esta en la escena. Devuelve true si se detecta el objeto. False en caso contrario
    private bool IsOnScene(GameObject objeto)
    {
        TrackableBehaviour trackableBehaviour = objeto.GetComponent<TrackableBehaviour>();

        TrackableBehaviour.Status currentStatus = trackableBehaviour.CurrentStatus;

        if (currentStatus == TrackableBehaviour.Status.DETECTED || currentStatus == TrackableBehaviour.Status.TRACKED)
            return true;

        return false;
    }

    // Comprueba la posicion del objeto A resoect del objeto B
    private void DetectObjectsPosition(ref GameObject gameObjectA, ref GameObject gameObjectB)
    {
        float distancia_min = 1.0f;
        float distancia_real = Mathf.Abs(object_A.transform.position.x - object_B.transform.position.x);

        if ((distancia_real <= distancia_min) && esta_cerca)
        {
            cont++;
            cont = cont % 2;
            //Llamar a funcion de copia
            esta_cerca = false;
        }

        if (distancia_real > distancia_min)
            esta_cerca = true;

    }

    private void CopyObjects(/*Vector3 position*/)
    {

        if (object_B.transform.childCount > 0)
        {
            for (int i = 0; i < object_B.transform.childCount; i++)
                Instantiate(object_B.transform.GetChild(i), object_A.transform);

            for (int i = 0; i < object_B.transform.childCount; i++)
                GameObject.Destroy(object_B.transform.GetChild(i).gameObject);
        }
    }



    private List<string> readFormula(string formula)
    {
        List<string> resultado = new List<string>();
        int cont_uppercase = 0;
        string temp = "";

        for (int i = 0; i < formula.Length; i++)
        {

            if (char.IsLetter(formula[i]))  // Comprobamos si el caracter actual es una letra
            {

                if (char.IsUpper(formula[i]))   // Si es mayuscula es porque es el 'comienzo' de un nuevo elemento
                {

                    if (cont_uppercase > 0)     // Este contador se usa para el caso de elementos de dos letras como Fe, Ag o para cuando un elemento sigue a otro Ej. HCl
                    {

                        if (temp.Length > 0)    // Si en nuestra solucion hay algo lo añadimos 
                            resultado.Add(temp);

                        resultado.Add("1");     // Para el segundo caso decimos que el elemento que hemos añadido tiene valencia, ya que esta no aparece en la formula
                        temp = "";

                        cont_uppercase--;       // cont_uppercase solo sera 0 y 1
                    }
                    temp += formula[i].ToString();
                    cont_uppercase++;
                }

                else if (char.IsLower(formula[i]))  // Si hemos encontrado una letra minuscula es porque ese elemento tiene dos letras, asi que lo añadimos a la solucion temporal
                {
                    temp += formula[i];
                    resultado.Add(temp);
                    temp = "";
                }


                if (i == (formula.Length - 1))  // Esto solamente se usa cuando el ultimo elemento de la formula tiene valencia 1
                {
                    if (temp.Length > 0)
                        resultado.Add(temp);
                    resultado.Add("1");
                }

            }

            else // Cuando detecta un nuemro si la solucion temporal tiene algo se añade y a continuacion el proximo valor
            {
                if (temp.Length > 0)
                    resultado.Add(temp);
                temp = formula[i].ToString();
                resultado.Add(temp);
                cont_uppercase = 0;
                temp = "";
            }
        }

        //Ordenamos la formula 
        resultado = SortFormula(resultado);
        return resultado;
    }
    /*
    * Ejemplo de la funcion anterior para H2SO4
    * 
    * Comenzamos con H. 
    *  Pasa el primer if (es una letra)
    *      Pasa el segundo if (es mayuscula)
    *          No pasa el tercer if (cont_uppercase esta a 0)
    *          Lo añadimos a temp e incrementamos cont_uppercase
    * 
    * Seguimos con '2'
    *  No pasa el primer if. Se va al else
    *  Añade lo que lleve en temp a la solucion y el 2 (Solucion: "H","2")
    *  Ponemos cont_uppercase a 0 y vaciamos temp
    *  
    * Seguimos con S
    *  Sucede igual que con H
    * 
    * Seguimos con O
    *  Pasa el primer, segundo y tercer if (cont_uppercase esta a 1)
    *  Añadimos lo que tenemos a la solucion (Solucion: "H","2","S")
    *  Añadimos el valor 1 indicando que es la valencia del S (Solucion: "H","2","S","1")
    *  Vaciamos temp y ponemos cont_uppercase a 0
    *  
    * Seguioms con '4'
    *  Sucede igual que con '2', pero esta vez si pasa el if (Solucion: "H","2","S","1","O")
    *  Añade el ultimo valor
    *  
    * Solucion: "H","2","S","1","O","4"
    *  
    */

    /*
     * Esta funcion ordena la formula de forma que el elemento princiapl es el primero, 
     * luego oxigeno y por ultimo hidrogeno
     */
    private List<string> SortFormula(List<string> f)
    {
        List<string> formula_ordenada = new List<string>();

        //Buscamos elemento central
        for (int i = 0; i < f.Count; i++)
        {

            if ((f[i] != "H") && (f[i] != "O") && (!char.IsDigit(f[i], 0))) // IsDigit comprueba si un char se puede convertir a un valor numerico
            {
                formula_ordenada.Add(f[i]);
                formula_ordenada.Add(f[i + 1]);
            }
        }
        //Añadimos el oxigeno (si hay)
        int index = f.IndexOf("O");
        if (index != -1)    // IndexOf devuelve -1 si no encuentra nada 
        {
            formula_ordenada.Add(f[index]);
            formula_ordenada.Add(f[index + 1]);
        }
        // Añadimos el hidrogeno (si hay)
        index = f.IndexOf("H");
        if (index != -1)
        {
            formula_ordenada.Add(f[index]);
            formula_ordenada.Add(f[index + 1]);
        }
        return formula_ordenada;
    }

    /**
     * El algorimo de dibujado principal
     * Determina el tipo de formula y la dibuja
     */
    private void Algoritmo(string f)
    {
        //Determinar el tipo de formula
        int tipo = GetTipo(f);

        switch (tipo)
        {
            case 1:
                // Crear tipo 1
                break;
            case 2:
                // Crear tipo 2
                break;
            case 3:
                // Crear tipo 3
                break;
            case 4:
                // Crear tipo 4
                break;
            default:
                // No hacer nada (lo más facil no cabe duda)
                break;
        }
    }


    /**
     * Determina el tipo de una formula 
     */
    public int GetTipo(string f)
    {
        bool tiene_dos_elementos = false; // Quiere decir que tiene dos elementos que no sean O o H
        bool tiene_oxigeno = false;
        bool tiene_hidrogeno = false;
        int tipo = 0;
        //Varaibles auxiliares
        int cont = 0;

        //Determinar los elementos
        for (int i = 0; i < f.Length; i += 2)
        {
            if (f[i] != 'H' && f[i] != 'O')
                cont++;
            else if (f[i] != 'H')
                tiene_hidrogeno = true;
            else if (f[i] != 'O')
                tiene_oxigeno = true;
        }

        if (cont >= 2)
            tiene_dos_elementos = true;

        //Tipo 1: hidrogeno + elemento
        if (!tiene_dos_elementos && !tiene_oxigeno && tiene_hidrogeno)
            tipo = 1;

        //Tipo 2: oxigeno + elemento
        if (!tiene_dos_elementos && tiene_oxigeno && !tiene_hidrogeno)
            tipo = 2;

        //Tipo 3: elemento + elemento
        if (tiene_dos_elementos && !tiene_oxigeno && !tiene_hidrogeno)
            tipo = 3;

        //Tipo 4: elemento + hidrogeno + oxigeno
        if (!tiene_dos_elementos && tiene_oxigeno && tiene_hidrogeno)
            tipo = 4;

        return tipo;
    }

    /*Método que se ejecuta cuando se pulsa el botón Introducir
      Cambiar para lo que se desee    
    */
    public void show()
    {
        Debug.Log("Se ha insertado el componente " + input);
    }

    /*Método que se ejecutará cada vez que el valor del campo de texto se modifique en tiempo real
      El parámetro string esta cambiado para que lo vaya cogiendo dinámicamente, en caso de que se quiera utilizar
      o introducir un string manual, cambiar el onValueChanged de Field por la llamada al mismo método pero 
      con el parámetro string que se le quiera poner
      
      IMPORTANTE: el input = s es imprescindible por lo tanto, no cambiar bajo ningun concepto
    */
    public void inputCapture(string s)
    {
        input = s;
        Debug.Log(input);
    }
}

