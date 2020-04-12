using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;
using UnityEngine.EventSystems;

public class CreateElement : MonoBehaviour
{
    // Objetos de la escena
    private GameObject object_A;
    private GameObject object_B;
    private GameObject objeto;
    private SQLiteHelper conexion_BD;

    public CreateElement(GameObject object_A, GameObject object_B)
    {
        this.object_A = object_A;
        this.object_B = object_B;
    }

    public void Create()
    {
        object_A = this.transform.GetChild(0).gameObject;
        object_B = this.transform.GetChild(1).gameObject;

        string name = EventSystem.current.currentSelectedGameObject.name;
        CrearNuevoAtomoElemento(name);
    }

    private Material buscarMaterial(string elemento)
    {
        conexion_BD = new SQLiteHelper();
        string material_objeto = this.conexion_BD.GetMaterialOf(elemento);
        string ruta_material = "Materials/" + material_objeto;
        Material material = Resources.Load<Material>(ruta_material);
        return material;
    }

    public void CrearNuevoAtomoElemento(string elemento)
    {
        objeto = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        Material mat = this.buscarMaterial(elemento);
        objeto.transform.GetComponent<Renderer>().material = mat;
        objeto.transform.parent = object_B.transform;
        objeto.transform.localScale = new Vector3(0.1325715f, 0.1325715f, 0.1325715f);
        objeto.transform.position = new Vector3(object_B.transform.position.x, object_B.transform.position.y + 0.4f, object_B.transform.position.z);
        
    }

}
