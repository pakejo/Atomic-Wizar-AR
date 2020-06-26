using UnityEngine;
using UnityEngine.EventSystems;

public class CreateElement : MonoBehaviour
{
    // Objetos de la escena
    private GameObject objetoDeEscenaA;

    private GameObject objetoDeEscenaB;
    private GameObject nuevoObjetoEnEscena;
    private SQLiteHelper conexionConLaBaseDeDatos;

    public CreateElement(GameObject object_A, GameObject object_B)
    {
        this.objetoDeEscenaA = object_A;
        this.objetoDeEscenaB = object_B;
    }

    public void Create()
    {
        objetoDeEscenaA = transform.GetChild(0).gameObject;
        objetoDeEscenaB = transform.GetChild(1).gameObject;

        string name = EventSystem.current.currentSelectedGameObject.name;
        crearNuevoAtomoDe(name);
    }

    private Material buscarMaterialDelElemento(string elemento)
    {
        conexionConLaBaseDeDatos = new SQLiteHelper();
        string material_objeto = conexionConLaBaseDeDatos.ObtenerMaterialDelElemento(elemento);
        string ruta_material = "Materials/" + material_objeto;
        Material material = Resources.Load<Material>(ruta_material);
        return material;
    }

    public void crearNuevoAtomoDe(string elemento)
    {
        nuevoObjetoEnEscena = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        Material materialNuevoAtomo = buscarMaterialDelElemento(elemento);
        nuevoObjetoEnEscena.transform.GetComponent<Renderer>().material = materialNuevoAtomo;
        nuevoObjetoEnEscena.transform.parent = objetoDeEscenaB.transform;
        nuevoObjetoEnEscena.transform.localScale = new Vector3(0.1325715f, 0.1325715f, 0.1325715f);
        nuevoObjetoEnEscena.transform.position = new Vector3(objetoDeEscenaB.transform.position.x, objetoDeEscenaB.transform.position.y + 0.4f, objetoDeEscenaB.transform.position.z);
    }
}