using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * #########################################################################
 * # CLASE PRINCIPAL DE LA APLICACIÓN, ENCARGDA DE TODA LA GESTIÓN DE ÉSTA #
 * #########################################################################
 */

public class Controlador : MonoBehaviour
{
    private GameObject button, inputField, probeText;
    private SQLiteHelper conexion_BD;
    private Animaciones animaciones;
    private ModoDibujado modoDibujado;
    private Boolean activated = false;
    private Dictionary<int, Func<TipoDibujado>> estrategiasDeDibujado;
    public string input;

    public GameObject Object_B { get; set; }

    /*
     * Función de Unity usada para inicializar los datos de la clase
     */

    private void Start()
    {
        // Animaciones sobre los objetos de la escena
        animaciones = new Animaciones();

        // Conexion con la base de datos
        conexion_BD = new SQLiteHelper();

        // Obtenemos los objetos que representan a las cartas
        Object_B = transform.GetChild(1).gameObject;

        // Por defecto, los dibujados se haran de forma automatica
        modoDibujado = ModoDibujado.AUTOMATICO;

        // Estrategias de dibujado (usado por el patron strategy)
        estrategiasDeDibujado = new Dictionary<int, Func<TipoDibujado>>()
        {
            {1, () => new DibujadoTipo1(this) },
            {3, () => new DibujadoTipo3(this) }
        };
    }

    /*
     * Función de Unity llamada durante cada frame para actualizar la escena
     */

    private void Update()
    {
        if (Object_B.transform.childCount > 0)
        {
            animaciones.setTarget(Object_B.transform.GetChild(0).gameObject);
            animaciones.activarRotacionAutomatica();
        }
    }

    /*
     * Funcion que lee y transcribe una formula.
     * Devuelve la formula procesada en una lista.
     *
     * Ejemplo para H2SO4
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
     */

    private List<string> readFormula(string formula)
    {
        List<string> resultado = new List<string>();
        int cont_uppercase = 0;
        string temp = "";

        for (int i = 0; i < formula.Length; i++)
        {
            // Comprobamos si el caracter actual es una letra
            if (char.IsLetter(formula[i]))
            {
                // Si es mayuscula es porque es el 'comienzo' de un nuevo elemento
                if (char.IsUpper(formula[i]))
                {
                    // Este contador se usa para el caso de elementos de dos letras como Fe, Ag o para cuando un elemento sigue a otro Ej. HCl
                    if (cont_uppercase > 0)
                    {
                        // Si en nuestra solucion hay algo lo añadimos
                        if (temp.Length > 0)
                            resultado.Add(temp);
                        // Para el segundo caso decimos que el elemento que hemos añadido tiene valencia, ya que esta no aparece en la formula
                        resultado.Add("1");
                        temp = "";
                        // cont_uppercase solo sera 0 y 1
                        cont_uppercase--;
                    }
                    temp += formula[i].ToString();
                    cont_uppercase++;
                }
                // Si hemos encontrado una letra minuscula es porque ese elemento tiene dos letras, asi que lo añadimos a la solucion temporal
                else if (char.IsLower(formula[i]))
                {
                    temp += formula[i];
                    resultado.Add(temp);
                    temp = "";
                }

                // Esto solamente se usa cuando el ultimo elemento de la formula tiene valencia 1
                if (i == (formula.Length - 1))
                {
                    if (temp.Length > 0)
                        resultado.Add(temp);
                    resultado.Add("1");
                }
            }
            // Cuando detecta un nuemro si la solucion temporal tiene algo se añade y a continuacion el proximo valor
            else
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
     * Esta funcion ordena la formula de forma que el elemento princiapl es el primero,
     * luego oxigeno y por ultimo hidrogeno
     */

    private List<string> SortFormula(List<string> f)
    {
        List<string> formula_ordenada = new List<string>();

        //Buscamos elemento central
        for (int i = 0; i < f.Count; i++)
        {
            // IsDigit comprueba si un char se puede convertir a un valor numerico
            if ((f[i] != "H") && (f[i] != "O") && (!char.IsDigit(f[i], 0)))
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

    private IEnumerator DibujarFormula(List<string> formula)
    {
        TipoDibujado estrategia = null;
        int NumeroDeEstrategia = obtenerElTipoDeFormula(formula);

        try
        {
            estrategia = estrategiasDeDibujado[NumeroDeEstrategia]();
        }
        catch (ArgumentException)
        {
            Debug.LogError("Excepcion en el dibujado. No existe un método que dibuje esa fórmula");
        }

        if (estrategia != null)
            yield return StartCoroutine(estrategia.Dibuja(formula, modoDibujado));
    }

    /**
     * Determina el tipo de una formula segun su estructura
     */

    public int obtenerElTipoDeFormula(List<string> f)
    {
        bool tiene_dos_elementos = false;
        bool tiene_oxigeno = false;
        bool tiene_hidrogeno = false;
        int tipo = 0;

        //Varaibles auxiliares
        int cont = 0;

        //Determinar los elementos
        for (int i = 0; i < f.Count; i += 2)
        {
            if (f[i] != "H" && f[i] != "O")
                cont++;
            else if (f[i] == "H")
                tiene_hidrogeno = true;
            else if (f[i] == "O")
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

    public float crearElemento(string tipo, GameObject padre)
    {
        // Crear la primitiva
        GameObject objeto = GameObject.CreatePrimitive(PrimitiveType.Sphere);

        // Obtener el material
        string material_objeto = conexion_BD.ObtenerMaterialDelElemento(tipo);
        string ruta_material = "Materials/" + material_objeto;
        Material mat = Resources.Load<Material>(ruta_material);

        // Obtener el radio
        int radio;
        List<string> info = conexion_BD.obtenerInformacionDeElementoConID(tipo);
        radio = Convert.ToInt32(info[2]);

        // Transformaciones del objeto
        objeto.transform.GetComponent<Renderer>().material = mat;
        objeto.transform.localScale = new Vector3(radio / 500.0f, radio / 500.0f, radio / 500.0f);
        objeto.transform.parent = padre.transform;
        objeto.transform.position = new Vector3(padre.transform.position.x, padre.transform.position.y + 0.5f, padre.transform.position.z);

        return radio / 500.0f;
    }

    /*
     * Método que se ejecuta cuando se pulsa el botón Introducir.
     * Cambiar para lo que se desee
    */

    public void show()
    {
        Debug.Log("Se ha insertado el componente " + input);
        List<string> formula = readFormula(input);

        if (Object_B.transform.childCount > 0)
        {
            GameObject obj = Object_B.transform.GetChild(0).gameObject;
            obj.transform.parent = null;
            GameObject.Destroy(obj);
        }

        StartCoroutine(DibujarFormula(formula));
    }

    /*
     * Método que se ejecutará cada vez que el valor del campo de texto se modifique en tiempo real
     * El parámetro string esta cambiado para que lo vaya cogiendo dinámicamente, en caso de que se quiera utilizar
     * o introducir un string manual, cambiar el onValueChanged de Field por la llamada al mismo método pero
     * con el parámetro string que se le quiera poner
     *
     * IMPORTANTE: el input = s es imprescindible por lo tanto, no cambiar bajo ningun concepto
     */

    public void inputCapture(string s)
    {
        input = s;

        if (button == null)
        {
            button = GameObject.Find("EnterButton");
            inputField = GameObject.Find("Input");
            probeText = GameObject.Find("ProbeText");
        }
        else
        {
            if (input == "Fe2S3" || input == "NaCl" || input == "Li2S" || input == "PbCl2")
            {
                button.GetComponent<Button>().interactable = true;
                inputField.GetComponent<UnityEngine.UI.Image>().color = new Color32(32, 250, 75, 145);
                probeText.GetComponent<Text>().text = "La fórmula introducida es correcta";
                probeText.GetComponent<Text>().color = Color.green;
            }
            else if (input == "HCl" || input == "H2S" || input == "NH3" || input == "CH4")
            {
                button.GetComponent<Button>().interactable = true;
                inputField.GetComponent<UnityEngine.UI.Image>().color = new Color32(32, 250, 75, 145);
                probeText.GetComponent<Text>().text = "La fórmula introducida es correcta";
                probeText.GetComponent<Text>().color = Color.green;
            }
            else
            {
                button.GetComponent<Button>().interactable = false;
                inputField.GetComponent<UnityEngine.UI.Image>().color = new Color32(250, 31, 85, 145);
                probeText.GetComponent<Text>().text = "La fórmula introducida no es correcta";
                probeText.GetComponent<Text>().color = Color.red;
            }

            Debug.Log(input);
        }
    }

    public void changeDrawMode()
    {
        button = GameObject.Find("PorPasos");
        probeText = GameObject.Find("PorPasosText");

        if (!activated)
        {
            modoDibujado = ModoDibujado.PASO_A_PASO;
            button.GetComponent<Image>().color = Color.green;
            probeText.GetComponent<Text>().color = new Color32(32, 24, 229, 255);
            activated = true;
        }
        else
        {
            modoDibujado = ModoDibujado.AUTOMATICO;
            button.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
            probeText.GetComponent<Text>().color = new Color32(0,0,0,255);
            activated = false;

        }
    }
}