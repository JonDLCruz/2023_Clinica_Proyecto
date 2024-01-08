using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCDialogo : MonoBehaviour
{
    public SingletonLogin singletonLogin;
    public Usuario usuario;
    public Introduccion presentacionNPC;
    string mensajeLogro1;
    string mensajeLogro2;
    string mensajeLogro3;
    string mensajeLogro4;
    string mensajeLogro5;

    private void Update()
    {
        singletonLogin = SingletonLogin.instance;
        mensajeLogro1 = singletonLogin.activity01ActiveUser ? "Has realizado la actividad 1" : "Aun no has realizado la actividad 1";
        mensajeLogro2 = singletonLogin.activity02ActiveUser ? "Has realizado la actividad 2" : "Aun no has realizado la actividad 2";
        mensajeLogro3 = singletonLogin.activity03ActiveUser ? "Has realizado la actividad 3" : "Aun no has realizado la actividad 3";
        mensajeLogro4 = singletonLogin.activity04ActiveUser ? "Has realizado la actividad 4" : "Aun no has realizado la actividad 4";
        mensajeLogro5 = singletonLogin.activity05ActiveUser ? "Has realizado la actividad 5" : "Aun no has realizado la actividad 5";



        presentacionNPC = new Introduccion();
        presentacionNPC.lines = new string[]
        {
            "Buenas alumno, bienvenido a MEDAC, antes que nada dejame comprobar tus datos.",
            "Nombre: " + singletonLogin.nameActiveUser + "\n" + "e-mail: " + singletonLogin.emailActiveUser,
            "Tienes " + singletonLogin.logrosActiveUser + " logros",
            mensajeLogro1 + "\n" + mensajeLogro2,
            mensajeLogro3 + "\n" + mensajeLogro4,
            mensajeLogro5,
            "¿Que actividad quieres hacer?",
        };


        //   usuario.registro[singletonLogin.iDActiveUser].activity01 = true;

        //iniciar conversacion

    }

    public void IniciarConversacion(int numeroConversacion)
    {
        string[] conversacion = (numeroConversacion == 1) ? presentacionNPC.lines : presentacionNPC.lines;

    }
}