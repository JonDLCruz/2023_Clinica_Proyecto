using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;

public class OBJ : MonoBehaviour
{
    private Vector3 posInit;
    private Vector3 currentPosit;
    void Start()
    {
        posInit = gameObject.transform.position;
    }

   
    public void ResetPosition()
    {
        //last post
        //que pase el tiempo con un Enumerator
        //igualar la posiciom actual del objeto a la posicion anterior del objeto
        print("Entramos al contador");
        StartCoroutine(posReset());//abajo
        print("Salimos del contador");
    }
    
    IEnumerator posReset()
    {
        yield return new WaitForSeconds(30);
        currentPosit = posInit;
        transform.position = currentPosit;
    }
}
