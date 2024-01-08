using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Status : MonoBehaviour
{
    public SingletonLogin login;
    public Usuario user;
    public GameObject status;
    public GameObject player;
    public TextMeshProUGUI nombre;
    public TextMeshProUGUI email;
    public TextMeshProUGUI actividad1;
    public TextMeshProUGUI actividad2;
    public TextMeshProUGUI actividad3;
    public TextMeshProUGUI actividad4;
    public TextMeshProUGUI actividad5;

    private void Awake()
    {
        PlayerController playerController = player.GetComponent<PlayerController>();
        playerController.enabled = false;
    }
    private void Start()
    {
        Invoke("ApagarStatus", 10);
    }
           
    
    // Update is called once per frame
    void Update()
    {
        login = SingletonLogin.instance;
        string mensajeLogro1 = login.activity01ActiveUser ? "Realizada" : "Sin realizar";
        string mensajeLogro2 = login.activity02ActiveUser ? "Realizada" : "Sin realizar"; 
        string mensajeLogro3 = login.activity03ActiveUser ? "Realizada" : "Sin realizar"; 
        string mensajeLogro4 = login.activity04ActiveUser ? "Realizada" : "Sin realizar";
        string mensajeLogro5 = login.activity05ActiveUser ? "Realizada" : "Sin realizar";

        nombre.text = login.nameActiveUser;
        email.text = login.emailActiveUser;
        actividad1.text = mensajeLogro1;
        actividad2.text = mensajeLogro2;
        actividad3.text = mensajeLogro3;
        actividad4.text = mensajeLogro4;
        actividad5.text = mensajeLogro5;
        

    }

    void ApagarStatus()
    {
        status.SetActive(false);
        PlayerController playerController = player.GetComponent<PlayerController>();
        playerController.enabled = true;
    }
        


}
