using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using System.Linq;
using UnityEngine.SceneManagement;
using static Usuario;


// log creado en paralelo al log manager. 

public class LoginManager : MonoBehaviour
{
    public Usuario usuario;
    [SerializeField] private GameObject error;
    [SerializeField] private GameObject login;
    [SerializeField] private GameObject usuarioMain;
    public TMP_Text textuser;
    public TMP_Text textpassword;
    public TMP_Text mensajeError;
    public TMP_Text userLogged;
    public string logUser;
    public string logPassword;
    public int logID;
    [SerializeField] private bool userlogExist = false;
    [SerializeField] private bool passlogExist = false;


    //metodo para comprobar si el user puede entrar o no al juego, vamos un login de toda la vida
    //BUG, IMPORTANTE, intentamos entrar a la aplicacion, si el usuario ha sido creado desde el inspector de UNITY no funciona, pero si ha sido creado desde dentro de la aplicacion, si funciona. 
    public void LoginInGame()
    {
        error.SetActive(false);
        usuarioMain.SetActive(false);
        mensajeError.text = "Usuario no existe o la contraseña es incorrecta";
        logUser = textuser.text;
        logPassword = textpassword.text;

        //verificamos si el usuario y el correo ya esten en la lista
        userlogExist = false;
        passlogExist = false;
        print("accedemos a base de datos");
        foreach (Registro registro in usuario.registro)
        {
            print("accedemos a objeto");
            if (registro.user == logUser)
            {
                Debug.Log("encuentro al usuario");
                userlogExist = true;
                logID = registro.ID;
                if (logPassword == registro.password)
                {
                    Debug.Log("encuentra al pass");
                    passlogExist = true;
                }
                else
                {
                    Debug.Log("No encuentra el pass");
                    passlogExist = false;
                }
                break;
            }
            print("accedemos al siguiente usuario");
        }
        //si el usuario esta en la lista
        if (userlogExist && passlogExist)
        {
            Debug.Log("cambio de escena");
            error.SetActive(false);
            login.SetActive(false);
            usuarioMain.SetActive(true);
            userLogged.text = logUser;

        }
        else
        {
            error.SetActive(true);
        }
    }

}