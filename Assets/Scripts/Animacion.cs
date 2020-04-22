using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animacion : MonoBehaviour
{
    private GameObject Object;

    public void setTarget(GameObject obj)
    {
        Object = obj;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void automaticRotation()
    {
        this.Object.transform.Rotate(new Vector3(0, 1, 0), 70 * Time.deltaTime);
    }
}
