using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

public class ChangeScene : MonoBehaviour
{

    [SerializeField] private GameObject m_registerUI = null;
    [SerializeField] private GameObject m_loginUI = null;
    [SerializeField] private GameObject error;

    public void ShowLogin()
    {
        m_registerUI.SetActive(false);
        m_loginUI.SetActive(true);
        error.SetActive(false);

    }

    public void ShowRegister()
    {        
        m_registerUI.SetActive(true);
        m_loginUI.SetActive(false);
        error.SetActive(false);
    }


}
