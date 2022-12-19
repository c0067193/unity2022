using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotator : MonoBehaviour
{
    public Vector3 axis;
    public float speed;
    private void Update()
    {
        transform.Rotate(axis, speed * Time.deltaTime);
    }
}
