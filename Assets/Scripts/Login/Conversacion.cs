using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "Conversacion", menuName = "Sistema de Dialogo/Nueva Conversacion")]
public class Conversacion : ScriptableObject {
    [System.Serializable]
    public struct Linea
    {
        public Personaje personaje;

        public AudioClip sonido;

        [TextArea(3, 5)]
        public string dialogo;
    }

    public bool desbloqueada;
    public bool finalizado;
    public bool reUsar;
   

    public Linea[] dialogos;

    ////////
    public Pregunta pregunta;
    ////////

}
