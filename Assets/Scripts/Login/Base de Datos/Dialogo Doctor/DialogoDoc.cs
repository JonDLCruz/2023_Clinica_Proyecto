using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DialogoDoc : MonoBehaviour
{
    public Personaje oak;
    
    private Image speaker = null;
    private string[] linesOak;
    private float textSped = 0.1f;
    int index;
    // Start is called before the first frame update
    private void Awake()
    {
        textoOak2();   
    }

    void Start()
    {
        
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void textoOak1()
    {
        
        //speaker.sprite = oak.imagen;
        speaker.name = oak.name;
        index = 0; 
    }

    private void textoOak2()
    {
        linesOak.AddRange("Veo que has llego a la fundación.");
        linesOak.AddRange("Habla con la enfermera para que te diga las pruebas que tienes que hacer");
    }
    
}
