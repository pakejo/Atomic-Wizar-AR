using UnityEngine;

public class Animaciones
{
    private GameObject objetoParaAnimar;

    public void setTarget(GameObject obj)
    {
        objetoParaAnimar = obj;
    }

    public void activarRotacionAutomatica()
    {
        objetoParaAnimar.transform.Rotate(new Vector3(0, 1, 0), 70 * Time.deltaTime);
    }
}