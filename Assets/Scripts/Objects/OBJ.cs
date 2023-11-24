using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;

public class OBJ : MonoBehaviour
{
    private Vector3 posInit;
    private Vector3 currentPosit;
    private float timeToReset = 30;
    public Coroutine resetCoroutine;


    void Start()
    {
        posInit = gameObject.transform.position;
    }

    public void ResetPosition()
    {
        print("Entramos al contador");
        // Verificar si ya hay una corrutina en ejecución antes de iniciar una nueva
        if (resetCoroutine != null)
        {
            StopCoroutine(resetCoroutine);
        }
        resetCoroutine = StartCoroutine(PosReset());
        print("Salimos del contador");
    }

    public void CancelResetPosition()
    {
        if (resetCoroutine != null)
        {
            StopCoroutine(resetCoroutine);
            print("Coroutina parada");
        }
    }

    IEnumerator PosReset()
    {
        yield return new WaitForSeconds(timeToReset);
        currentPosit = posInit;
        print("Transformar posicion");
        transform.position = currentPosit;
        print("Posicion Transformada");
    }

}
