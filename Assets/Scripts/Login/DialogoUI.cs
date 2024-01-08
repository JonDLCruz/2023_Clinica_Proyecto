using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class DialogoUI : MonoBehaviour
{
    public Conversacion conversacion; // conversacion actual mostrada
    [SerializeField] private float textSpeed = 10;
    [SerializeField] private GameObject convContainer; //objeto de la conversacion
    [SerializeField] private GameObject pregContainer; //objeto de la pregunta
    [SerializeField] private Image speakIm; //imagen de la persona que este hablando
    [SerializeField] private TextMeshProUGUI nombre;  //nombre del speaker
    [SerializeField] private TextMeshProUGUI convText; //texto que dice
    [SerializeField] private Button continuarButton; // para avanzar en el texto
    [SerializeField] Button anteriorButton; //para retroceder en el texto... no se si lo usare

    private AudioSource audioSource;

    public int localIn = 0; //recorre cada dialogo dentro de la conversacion actual (lo mismo que dialLocalIn en DialogueSpeaker, solo que este adopta en valor base al que tenga puesto el DialogueSpeaker al momento de hablar.)

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        convContainer.SetActive(true);
        pregContainer.SetActive(false);

        continuarButton.gameObject.SetActive(true);
        anteriorButton.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ActualizarTextos(int comportamiento)
    {
        convContainer.SetActive(true);
        pregContainer.SetActive(false);

        switch (comportamiento)
        {   //Retroceder texto
            case -1:

                if (localIn > 0)
                {
                    print("dialogo anterior");
                    localIn--;

                    nombre.text = conversacion.dialogos[localIn].personaje.nombre;
                    StopAllCoroutines();
                    StartCoroutine(EscribirTexto()); //efecto de escribir lento. 
                    //convText.text = conversacion.dialogos[localIn].dialogo; //Esto es para no darle efecto de escribir texto
                    speakIm.sprite = conversacion.dialogos[localIn].personaje.imagen;

                    if (conversacion.dialogos[localIn].sonido != null)
                    {
                        audioSource.Stop();
                        audioSource.PlayOneShot(conversacion.dialogos[localIn].sonido);
                    }
                    anteriorButton.gameObject.SetActive(localIn > 0);
                }
                DialogueManager.speakerActual.dialLocalIn = localIn;
                break;
            // no avanzar con el texto 
            case 0:
                print("dialogo actualizado");

                nombre.text = conversacion.dialogos[localIn].personaje.nombre;
                StopAllCoroutines();
                StartCoroutine(EscribirTexto()); //efecto de escribir lento. 
                //convText.text = conversacion.dialogos[localIn].dialogo; //Esto es para no darle efecto de escribir texto
                speakIm.sprite = conversacion.dialogos[localIn].personaje.imagen;

                if (conversacion.dialogos[localIn].sonido != null)
                {
                    audioSource.Stop();
                    audioSource.PlayOneShot(conversacion.dialogos[localIn].sonido);
                }
                anteriorButton.gameObject.SetActive(localIn > 0);


                /* yo realmente no lo uso, soy mas bien de la vieja escuela, boton a y pa alante toa
                                if (localIn >= conversacion.dialogos.Length - 1)
                                {
                                    continuarButton.GetComponent<TextMeshProUGUI>().text = "Finalizar";
                                } else {

                                    continuarButton.GetComponent<TextMeshProUGUI>().text = "Continuar";
                                }
                */


                break;

            //avanzar texto
            case 1:

                // el -1 es para evitar que el index se salga del index de la array
                if (localIn < conversacion.dialogos.Length - 1)
                {
                    print("Dialogo siguiente");
                    localIn++;
                    nombre.text = conversacion.dialogos[localIn].personaje.nombre;
                    StopAllCoroutines();
                    StartCoroutine(EscribirTexto()); //efecto de escribir lento. 
                    //convText.text = conversacion.dialogos[localIn].dialogo; //Esto es para no darle efecto de escribir texto
                    speakIm.sprite = conversacion.dialogos[localIn].personaje.imagen;

                    if (conversacion.dialogos[localIn].sonido != null)
                    {
                        audioSource.Stop();
                        audioSource.PlayOneShot(conversacion.dialogos[localIn].sonido);
                    }
                    anteriorButton.gameObject.SetActive(localIn > 0);


                    /* yo realmente no lo uso, soy mas bien de la vieja escuela, boton a y pa alante toa
                                    if (localIn >= conversacion.dialogos.Length - 1)
                                    {
                                        continuarButton.GetComponent<TextMeshProUGUI>().text = "Finalizar";
                                    } else {

                                        continuarButton.GetComponent<TextMeshProUGUI>().text = "Continuar";
                                    }
                    */

                }
                else
                {
                    print("Dialogo terminado");
                    localIn = 0;
                    DialogueManager.speakerActual.dialLocalIn = 0;
                    conversacion.finalizado = true;

                    if (conversacion.pregunta != null)
                    {
                        convContainer.SetActive(false);
                        pregContainer.SetActive(true);
                        var preg = conversacion.pregunta;
                        DialogueManager.instance.controladorPreguntas.ActivarBotones(preg.opciones.Length, preg.pregunta, preg.opciones);

                        return;
                    }

                    DialogueManager.instance.MostrarUI(false);
                    return;
                }

                DialogueManager.speakerActual.dialLocalIn = localIn;

                break;

            default:
                Debug.LogWarning("estas pasando un valor no permitido");
                break;
        }
    }

    IEnumerator EscribirTexto()
    {
        convText.maxVisibleCharacters = 0;
        convText.text = conversacion.dialogos[localIn].dialogo;
        convText.richText = true;

        for (int i = 0; i < conversacion.dialogos[localIn].dialogo.ToCharArray().Length; i++)
        {
            convText.maxVisibleCharacters++;
            yield return new WaitForSeconds(1f / textSpeed);
        }

    }

    public Image Imagen()
    {
        return speakIm;
    }

    public void CambiarImagen(Image nImagen)
    {
        speakIm = nImagen;
    }

    public void ConversacionUIOn()
    {
        convContainer.SetActive(true);
    }

    public void ConversacionUIOff()
    {
        convContainer.SetActive(false);
    }
    public TextMeshProUGUI Texto()
    {
        return convText;
    }
    public void EscribirDialogo(string texto)
    {
        convText.text = texto;
        StopAllCoroutines();
        StartCoroutine(EscribirTexto()); //efecto de escribir lento. 

    }
}
