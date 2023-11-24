using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemRotate : MonoBehaviour
{
    private float velocityR = 20.0f;

    // Update is called once per frame
    void Update()
    {
        // Rotar el objeto alrededor del eje Y basado en el tiempo y la velocidad definida
        transform.Rotate(Vector3.up, velocityR * Time.deltaTime);
    }
}
