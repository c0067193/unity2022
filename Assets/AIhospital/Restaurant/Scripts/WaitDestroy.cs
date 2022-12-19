using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitDestroy : MonoBehaviour {

    // Set the time of destroy in the inspector of the object that will be destroyed.

    public float time;

    // Destroys object with script attached after an amount of seconds.

    private void Start()
    {
        Destroy(this.gameObject, time);
    }
}
