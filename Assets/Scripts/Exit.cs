using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exit : MonoBehaviour
{
    public void ExitApp()
    {
        Application.Quit();

        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
    }
}
