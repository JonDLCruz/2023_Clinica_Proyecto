using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager.Requests;
using UnityEngine;

public class ReturnInitial : MonoBehaviour
{
    public Vector3 posInit;
    public Vector3 currentPosit;
    private float contador = 2.0f;

    // Start is called before the first frame update
    void Start()
    {
        posInit = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ResetPosition()
    {
        currentPosit = transform.position;
        print("Entramos en el contador");
        StartCoroutine(posReset());
    }

    IEnumerator posReset()
    {
        yield return new WaitForSeconds(contador);
        currentPosit = posInit;
        transform.position = currentPosit;
        print("Salimos del contador");
    }
}