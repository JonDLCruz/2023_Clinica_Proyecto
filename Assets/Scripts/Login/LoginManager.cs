using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using System.Linq;

public class LoginManager : MonoBehaviour
{
    public Usuario usuario;
    [SerializeField] private GameObject error;
    [SerializeField] private GameObject m_loginUI = null;
    [SerializeField] private GameObject m_introUI = null;
    public TMP_Text textuser;
    public TMP_Text textpassword;
    public TMP_Text mensajeError;
    public string logUser;
    public string logPassword;
    public string logEmail;
    public int logID;
    public bool logNewPlayer;
    [SerializeField] private bool passlogExist = false;


    //metodo para comprobar si el user puede entrar o no al juego, vamos un login de toda la vida
    public void LoginInGame()
    {
        error.SetActive(false);
        mensajeError.text = "Error. Usuario no existe o la contraseña es incorrecta";
        logUser = textuser.text;
        logPassword = textpassword.text;


        //verificamos si el usuario y el correo ya esten en la lista
        bool userlogExist = false;
        bool passlogExist = false;


        foreach (Registro registro in usuario.registro)
        {
            if (registro.user == logUser)
            {
                userlogExist = true;
                logID = registro.ID;
                logEmail = registro.email;
                logNewPlayer = registro.newPlayer;
                if (String.Equals(registro.password, logPassword))
                {
                    passlogExist = true;
                }
                else
                {
                    passlogExist = false;
                }

                break;
            }

        }


        //si el usuario esta en la lista

        if (userlogExist && passlogExist)
        {
            
            if (logNewPlayer)
            {
                Debug.Log("Aparece Oak");
                m_loginUI.SetActive(false);
                m_introUI.SetActive(true);
                for (int i = 0; i < usuario.registro.Length; i++)
                {
                    if (usuario.registro[i].user == logUser)
                    {
                        Debug.Log("se ha cambiado el booleano");
                        usuario.registro[i].newPlayer = false;
                    }
                    else
                    {
                        Debug.Log("te jodiste, va mal");
                    }
                }

            }
            else
            {
                Debug.Log("cambio de escena");
            }

        }
        else
        {
            error.SetActive(true);
        }
    }

}

