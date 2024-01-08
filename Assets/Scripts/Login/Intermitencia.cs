using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Intermitencia : MonoBehaviour
{
    private float escalaMaxima = 0.5f;

    private float tiempo = 0.9f;
    private bool intermitencia = true;
    private Vector3 escalaInicial;

        // Start is called before the first frame update
    void Start()
    {
        escalaInicial = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        tiempo += Time.deltaTime;

        if (tiempo >= 1f)
        {
            tiempo = 0f;
            intermitencia = !intermitencia; 
        }

        if (intermitencia)
        {
            float nuevaEscala = Mathf.PingPong(Time.time,1) + escalaMaxima;
            transform.localScale = escalaInicial * escalaMaxima;
            
        }
        else
        {
            transform.localScale = escalaInicial;
        }


    }
}
