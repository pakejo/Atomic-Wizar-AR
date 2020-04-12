using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class Controlador : MonoBehaviour
{
    // Objetos de la escena
    private GameObject object_A;
    private GameObject object_B;


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
}
