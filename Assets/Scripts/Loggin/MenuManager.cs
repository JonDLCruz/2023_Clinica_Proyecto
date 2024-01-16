using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public GameObject usuario, perfil, opciones, ayuda, salir, estadisticas, progresion, logros, general, graficos, controles, ayudaOpciones;
    public GameObject redes; // estos se pueden quitar por enlaces directos.
    [SerializeField] private Image image;  // estos se pueden quitar por enlaces directos.
    public Sprite iMedac, iTwitter, iTiktok; // estos se pueden quitar por enlaces directos.
    public void BackBotton()
    {
        perfil.SetActive(false);
        opciones.SetActive(false);
        ayuda.SetActive(false);
        usuario.SetActive(true);
    }
    public void NewGame()
    {
        SceneManager.LoadScene(1);
    }
    public void PerfilBotton()
    {
        usuario.SetActive(false);
        opciones.SetActive(false);
        ayuda.SetActive(false);
        salir.SetActive(false);
        progresion.SetActive(false);
        logros.SetActive(false);
        perfil.SetActive(true);
        estadisticas.SetActive(true);
    }

    public void OpcionBoton()
    {
        usuario.SetActive(false);
        perfil.SetActive(false);
        ayuda.SetActive(false);
        salir.SetActive(false);
        graficos.SetActive(false);
        controles.SetActive(false);
        ayudaOpciones.SetActive(false);
        opciones.SetActive(true);
        general.SetActive(true);
    }

    public void AyudaBoton()
    {
        usuario.SetActive(false);
        perfil.SetActive(false);
        ayuda.SetActive(true);
        salir.SetActive(false);
        graficos.SetActive(false);
        controles.SetActive(false);
        ayudaOpciones.SetActive(false);
        opciones.SetActive(false);
        
    }
    public void SalirBoton()
    {
        salir.SetActive(true);
    }

    public void BotonMedac()
    {
        image.sprite = iMedac; 
        redes.SetActive(true);
    }

    public void BotonTwitter()
    {
        image.sprite = iTwitter;
        redes.SetActive(true);
    }

    public void BotonTikTok()
    {
        image.sprite = iTiktok; 
        redes.SetActive(true);
    }

    public void CerrarRedes()
    {
        image.sprite = null;
        redes.SetActive(false);
    }


    public void EstaditicasBotton()
    {
        progresion.SetActive(false);
        logros.SetActive(false);
        estadisticas.SetActive(true);
    }

    public void ProgresionBotton()
    {
        estadisticas.SetActive(false);
        logros.SetActive(false);
        progresion.SetActive(true);
    }

    public void LogrosBotton()
    {
        estadisticas.SetActive(false);
        progresion.SetActive(false);
        logros.SetActive(true);
    }

    public void GeneralBotton()
    {   
        graficos.SetActive(false);
        controles.SetActive(false);
        ayudaOpciones.SetActive(false);
        general.SetActive(true);        
    }

    public void GraficoBotton()
    {        
        controles.SetActive(false);
        ayudaOpciones.SetActive(false);
        general.SetActive(false);
        graficos.SetActive(true);
    }
   
    public void ControlesBotton()
    {
        graficos.SetActive(false);        
        ayudaOpciones.SetActive(false);
        general.SetActive(false);
        controles.SetActive(true);
    }

    public void AyudaOpBotton()
    {
        graficos.SetActive(false);
        controles.SetActive(false);
        ayudaOpciones.SetActive(true);
        general.SetActive(false);
    }
    public void AceptarBoton()
    {

        Application.Quit();
        
    }

    public void CancelarBotton()
    {
        salir.SetActive(false);
    }
}
