using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public void Actividad1()
    {
        SceneManager.LoadScene("Actividad_01");
    }
    public void Actividad2()
    {
        SceneManager.LoadScene("Actividad_02");
    }
    public void Actividad3()
    {
        SceneManager.LoadScene("Actividad_03");
    }
    public void LoadSceneClinica()
    {
        SceneManager.LoadScene("Clinica_01");
    }
    
}
