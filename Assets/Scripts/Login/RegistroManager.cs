using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static Usuario;

public class RegistroManager : MonoBehaviour
{
    public Usuario usuario;
    [SerializeField] private GameObject error;
    public TMP_Text textuser;
    public TMP_Text textemail;
    public TMP_Text textpassword;
    public TMP_Text textconfirmarPassw;
    public TMP_Text mensajeError;

    //metodo para agregar un nuevo usuario a la lista
    public void AddUser()
    {
        error.SetActive(false);
        mensajeError.text = "Error. Usuario o contraseña ya existente";
        string newUser = textuser.text;
        string newEmail = textemail.text;
        string newPassword = textpassword.text;
        string newConfirmP = textconfirmarPassw.text;


        //verificamos si el usuario y el correo ya esten en la lista
        bool userExist = false;
        bool emailExist = false;
        foreach (Registro registro in usuario.registro)
        {
            if (registro.user == newUser)
            {
                userExist = true;
                break;
            }
        }
        foreach (Registro registro in usuario.registro)
            if (registro.email == newEmail)
            {
                emailExist = true;
                break;
            }


        //si el usuario no esta en la lista

        if (!userExist && newPassword == newConfirmP && !emailExist)
        {
            Registro nuevoRegistro = new Registro
            {
                ID = usuario.registro.Length + 1,
                user = newUser,
                email = newEmail,
                password = newPassword,
                confiPass = newConfirmP,
                newPlayer = true,
            };

            int newLength = usuario.registro.Length + 1;
            Array.Resize(ref usuario.registro, newLength);
            usuario.registro[newLength - 1] = nuevoRegistro;
            mensajeError.text = "Usuario creado Correctamente";

        }
        else
        {
            error.SetActive(true);
        }
    }


}
