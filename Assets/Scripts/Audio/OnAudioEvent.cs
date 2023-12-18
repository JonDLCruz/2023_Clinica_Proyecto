using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnAudioEvent : MonoBehaviour
{
    private AudioInstance audioMNG;
    public string clipName;
    [SerializeField]
    private bool isPlaying;
    private float fadeD = 2f;


    void Start()
    {
        audioMNG = AudioInstance.instance;
        isPlaying = false;
    }
    //Vamos a crear dos IENUMERATOR para controlar la ganancia de sonido a la hora de entrar y salir
    private IEnumerator SonidoEntrada(float _d)
    {
        float startV = audioMNG.GetVolume();//SI algo falla puede ser esto
        float targetV = 1.0f;//queremos alcanzar este volumnen que es el normal
        float currentTime = 0f;

        while (currentTime < _d)
        {
            currentTime += Time.deltaTime;
            float t = currentTime / _d;
            audioMNG.SetVolume(Mathf.Lerp(startV, targetV, t));
            yield return null;
        }

    }
    private IEnumerator SonidoSalida(float _d)
    {
        float startV = audioMNG.GetVolume();//SI algo falla puede ser esto
        float currentTime = 0f;

        while (currentTime < _d)
        {
            currentTime += Time.deltaTime;
            float t = currentTime / _d;
            audioMNG.SetVolume(Mathf.Lerp(startV, 0f, t));//No ponemos targetV porque total es 0
            yield return null;
        }

        audioMNG.Stop(clipName);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("AudioBroadcaster"))
        {
            print("ENTRA");
            if (audioMNG != null)
            {
                isPlaying = true;
                audioMNG.SelectAudio(clipName);
                StartCoroutine(SonidoEntrada(fadeD));
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("AudioBroadcaster"))
        {
            if (audioMNG != null)
            {
                isPlaying = false;
                StartCoroutine(SonidoSalida(fadeD));
            }
        }
    }
}
