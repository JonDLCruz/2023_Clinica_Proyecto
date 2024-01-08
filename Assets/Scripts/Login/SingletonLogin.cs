using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Usuario;

public class SingletonLogin : MonoBehaviour
{
    public static SingletonLogin instance;
    public Usuario usuario;
    public LoginManager loginManager;
    public int iDActiveUser;
    public string nameActiveUser;
    public string emailActiveUser;
    public int activityDoneActiveUser;
    public int logrosActiveUser;
    public bool activity01ActiveUser;
    public bool activity02ActiveUser;
    public bool activity03ActiveUser;
    public bool activity04ActiveUser;
    public bool activity05ActiveUser;


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
        
    }

    private void Update()
    {
        iDActiveUser = loginManager.logID;
        foreach (Registro registro in usuario.registro)
        {
            if (registro.ID == iDActiveUser)
            {
                nameActiveUser = registro.user;
                emailActiveUser = registro.email;
                activityDoneActiveUser = registro.activityDone;
                logrosActiveUser = registro.logros;
                activity01ActiveUser = registro.activity01;
                activity02ActiveUser = registro.activity02;
                activity03ActiveUser = registro.activity03;
                activity03ActiveUser = registro.activity04;
                activity05ActiveUser = registro.activity05;

            }


        }
    }
}
