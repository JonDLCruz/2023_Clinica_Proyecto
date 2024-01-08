using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance { get; private set; }
    public static DialogueSpeaker speakerActual;
    [SerializeField] private DialogoUI dialUI;
    [SerializeField] private GameObject player;


    ////////////
    public ControladorPreguntas controladorPreguntas;
    ///////////
    // Start is called before the first frame update
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        dialUI = FindObjectOfType<DialogoUI>();
        ///
        controladorPreguntas = FindAnyObjectByType<ControladorPreguntas>();
        ///

    }

    void Start()
    {
        MostrarUI(false);
    }

    public void MostrarUI(bool mostrar)
    {
        dialUI.gameObject.SetActive(mostrar);
        if (!mostrar)
        {
            dialUI.localIn = 0;
            //el pj pueda moverse
        }
        else
        {
            //el personaje debe bloquearse aqui
        }
    }

    public void SetConversacion(Conversacion conv, DialogueSpeaker speaker)
    {
        ////////////////////
        if (speaker != null)
        {
            speakerActual = speaker;

        }
        else
        {
            //en caso de ser Speaker null quiere decir que vengo de una pregunta, por lo que reseteo el localIn para recorrer toda la nueva conversacion producto de la respuesta elegida
            dialUI.conversacion = conv;
            dialUI.localIn = 0;
            dialUI.ActualizarTextos(0);
        }
        if (conv.finalizado && !conv.reUsar)
        {
            dialUI.conversacion = conv;
            dialUI.localIn = conv.dialogos.Length;
            dialUI.ActualizarTextos(1);
        }
        else
        {
            dialUI.conversacion = conv;
            dialUI.localIn = speakerActual.dialLocalIn;
            dialUI.ActualizarTextos(0);
        }
    }

    public void CambiarEstadoDeReUsable(Conversacion conv, bool estadoDeseado)
    {
        conv.reUsar = estadoDeseado;
    }

    //funcion a llamar para desbloquear x conversacion
    public void BoqueoYDesbloqueoDeConversacion(Conversacion conv, bool desbloquear)
    {
        conv.desbloqueada = desbloquear;
    }
}
