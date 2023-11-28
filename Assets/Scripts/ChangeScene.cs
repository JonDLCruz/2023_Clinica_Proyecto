using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeScene : MonoBehaviour
{

    [SerializeField] private GameObject m_registerUI = null;
    [SerializeField] private GameObject m_loginUI = null;

    public void ShowLogin()
    {
        Debug.Log("Click.");
        m_registerUI.SetActive(false);
        m_loginUI.SetActive(true);

    }

    public void ShowRegister()
    {
        Debug.Log("Click.");
        m_registerUI.SetActive(true);
        m_loginUI.SetActive(false);
    }


}
