using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneChange : MonoBehaviour
{
    [SerializeField] private GameObject m_registerUI = null;
    [SerializeField] private GameObject m_loginUI = null;

    public void ShowLogin()
    {
        m_registerUI.SetActive(false);
        m_loginUI.SetActive(true);

    }

    public void ShowRegister() 
    {
        m_registerUI.SetActive(true);
        m_loginUI.SetActive(false);
    }




}
