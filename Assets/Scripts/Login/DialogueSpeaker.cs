using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DialogueSpeaker : MonoBehaviour
{
    public List<Conversacion> conversacionesDisponibles = new ();
    [SerializeField] private int indexDeConversaciones = 0; // recorre cada conversacion dentro de la lista 
    public int dialLocalIn = 0; //recorre cada dialogo dentro de la conversacion actual


    // Start is called before the first frame update
    void Start()
    {
        indexDeConversaciones = 0;
        dialLocalIn = 0;

        //////////DEBUG ONLY/// ///////
        foreach (var conv in conversacionesDisponibles)
        {
            conv.finalizado = false;
            var preg = conv.pregunta;
            if (preg != null)
            {
                foreach (var opcion in preg.opciones)
                {
                    opcion.convResultante.finalizado = false;
                }

            }
        }

        //////////DEBUG ONLY///////////
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Conversar()
    {
        if (indexDeConversaciones <= conversacionesDisponibles.Count - 1)
        {
            if (conversacionesDisponibles[indexDeConversaciones].desbloqueada)
            {
                if (conversacionesDisponibles[indexDeConversaciones].finalizado)
                {
                    if (ActualizarConversacion())
                    {
                        DialogueManager.instance.MostrarUI(true);
                        DialogueManager.instance.SetConversacion(conversacionesDisponibles[indexDeConversaciones], this);
                    }
                    DialogueManager.instance.SetConversacion(conversacionesDisponibles[indexDeConversaciones], this);
                    return;
                }
                DialogueManager.instance.MostrarUI(true);
                DialogueManager.instance.SetConversacion(conversacionesDisponibles[indexDeConversaciones], this);
            }
            else
            {
                Debug.LogWarning("La convesacion esta bloqueada");
                DialogueManager.instance.MostrarUI(false);

            }
        }
        else
        {
            //ya use todas las conversaciones disponibles 
            print("Fin del dialogo");
            DialogueManager.instance.MostrarUI(false);
        }

        bool ActualizarConversacion()
        {
            if (!conversacionesDisponibles[indexDeConversaciones].reUsar)
            {
                if (indexDeConversaciones < conversacionesDisponibles.Count - 1)
                {
                    indexDeConversaciones++;
                    return true;
                }
                else
                {
                    return false;
                }


            }
            else
            {
                return true;
            }
        }


    }






}


