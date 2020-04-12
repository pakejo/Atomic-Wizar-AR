using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class Lector : MonoBehaviour
{
    // Start is called before the first frame update
    private void detectarObjetos()
    {
        IEnumerable<TrackableBehaviour> tbs = TrackerManager.Instance.GetStateManager().GetActiveTrackableBehaviours();
        foreach (TrackableBehaviour tb in tbs)
        {
            Debug.Log("Nombre trackable: " + tb.name);
        }
    }
}
