using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;


public class InventarioUI : MonoBehaviour
{
    public bool misionActive = false; //stado de la mision
    public GameObject missionUI, misionStart, pressS, cogerE;
    public Toggle mision1, mision2, mision3;
    public AudioClip misionS;
    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        misionStart.SetActive(false);
        missionUI.SetActive(false);
        cogerE.SetActive(false);
        mision1.isOn = false;
        mision1.enabled = false;
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = misionS;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (!misionActive)
            {
                //aqui podras mostrar algun tipo de UI
                pressS.SetActive(false);
                misionStart.SetActive(true);
                audioSource.Play();
                CountBackStartMision();

            }

        }
    }

    private void CountBackStartMision()
    {
        StartCoroutine(TurnOffRoutine());

    }
    private System.Collections.IEnumerator TurnOffRoutine()
    {
        yield return new WaitForSeconds(2);
        misionStart.SetActive(false);
        //activa la mision
        misionActive = true;
        missionUI.SetActive(true);
    }


}
