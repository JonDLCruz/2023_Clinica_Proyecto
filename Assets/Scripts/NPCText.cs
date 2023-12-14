using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NPCText : MonoBehaviour
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
        if (singletonLogin.activity01ActiveUser)
        {
            mensajeLogro1 = "Has realizado la actividad 1"; 
        }
        else
        {
            mensajeLogro1 = "Aun no has realizado la actividad 1";
        }

        if (singletonLogin.activity02ActiveUser)
        {
            mensajeLogro2 = "Has realizado la actividad 2";
        }
        else
        {
            mensajeLogro2 = "Aun no has realizado la actividad 2";
        }

        if (singletonLogin.activity03ActiveUser)
        {
            mensajeLogro3 = "Has realizado la actividad 3";
        }
        else
        {
            mensajeLogro3 = "Aun no has realizado la actividad 3";
        }

        if (singletonLogin.activity04ActiveUser)
        {
            mensajeLogro4 = "Has realizado la actividad 4";
        }
        else
        {
            mensajeLogro4 = "Aun no has realizado la actividad 4";
        }

        if (singletonLogin.activity05ActiveUser)
        {
            mensajeLogro5 = "Has realizado la actividad 5";
        }
        else
        {
            mensajeLogro5 = "Aun no has realizado la actividad 5";
        }



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
    }
}
