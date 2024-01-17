using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    //Variables del brillo
    public Slider sliderBri;        // Referencia al Slider de brillo en el menú
    public float sliderBriV;        // Valor actual del brillo
    public Image panelBrillo;       // Panel que representa el brillo en el menú

    //Variables volumen
    public Slider slider;           // Referencia al Slider de volumen en el menú
    public float sliderValue;       // Valor actual del volumen

    //Variable de calidad de opciones
    public TMP_Dropdown dropdown;        // Referencia al Dropdown para la calidad gráfica
    public int calidad;                  // Nivel de calidad seleccionado

    //Variables de Pantalla completa y resolucion
    public Toggle toggle;                    // Referencia al Toggle de pantalla completa

    public TMP_Dropdown resolucionDrop;      // Referencia al Dropdown de resoluciones
    Resolution[] resoluciones;               // Lista de resoluciones disponibles

    //Variables del FOV
    public Slider sliderFOV;                // Referencia al Slider de campo de visión (FOV)
    public float fovValue = 60f;            // Valor actual del FOV
    public Camera mainCamera;               // Referencia a la cámara principal

    private void Start()
    {
        //Codigo para el brillo
        sliderBri.value = PlayerPrefs.GetFloat("brillo", 50f); // Recupera el valor del brillo desde PlayerPrefs
        panelBrillo.color = new Color(panelBrillo.color.r, panelBrillo.color.g, panelBrillo.color.b, sliderBri.value);

        //Codigo Volumen
        slider.value = PlayerPrefs.GetFloat("volumenAudio", 50f);   // Recupera el valor del volumen desde PlayerPrefs
        AudioListener.volume = slider.value;

        //Codigo de calidad de opciones
        calidad = PlayerPrefs.GetInt("numeroCalidad", 3);  // Recupera el nivel de calidad desde PlayerPrefs
        dropdown.value = calidad;
        AjustarCalidad();

        //Codigo de Pantalla completa y resolucion
        if (Screen.fullScreen)
        {
            toggle.isOn = true;
        }
        else
        {
            toggle.isOn = false;
        }
        RevisarResolucion();

        //Codigo de del FOV
        mainCamera = Camera.main;

        if (mainCamera != null)
        {
            sliderFOV.value = PlayerPrefs.GetFloat("FOV", mainCamera.fieldOfView);  // Recupera el valor del FOV desde PlayerPrefs
            UpdateFOV();
        }
    }

    //Funcion brillo
    public void ChangeSliderBri(float valor)
    {
        sliderBriV = valor;
        PlayerPrefs.SetFloat("brillo", sliderBriV); // Guarda el nuevo valor del brillo en PlayerPrefs
        panelBrillo.color = new Color(panelBrillo.color.r, panelBrillo.color.g, panelBrillo.color.b, sliderBri.value);
    }

    //Funcion volumen
    public void ChangeSlider(float valor)
    {
        sliderValue = valor;
        PlayerPrefs.SetFloat("volumenAudio", sliderValue);  // Guarda el nuevo valor del volumen en PlayerPrefs
        AudioListener.volume = slider.value;
    }

    //Funcion calidad dee opciones
    public void AjustarCalidad()
    {
        QualitySettings.SetQualityLevel(dropdown.value);  // Establece el nivel de calidad según la selección del usuario
        PlayerPrefs.SetInt("numeroCalidad", dropdown.value);  // Guarda la selección en PlayerPrefs
        calidad = dropdown.value;
    }

    //Funcion Pantalla completa
    public void ActivateFullScreen(bool fullScreen)
    {
        Screen.fullScreen = fullScreen;  // Establece la pantalla completa según la selección del usuario
    }

    //Funcion para la Resolucion
    public void RevisarResolucion()
    {
        resoluciones = Screen.resolutions;  // Obtiene la lista de resoluciones disponibles
        resolucionDrop.ClearOptions();
        List<string> opciones = new List<string>();
        int resolucionActual = 0;

        for (int i = 0; i < resoluciones.Length; i++)
        {
            string opcion = resoluciones[i].width + " x " + resoluciones[i].height;
            opciones.Add(opcion);

            if (Screen.fullScreen && resoluciones[i].width == Screen.currentResolution.width && resoluciones[i].height == Screen.currentResolution.height)
            {
                resolucionActual = i;
            }
        }
        resolucionDrop.AddOptions(opciones);
        resolucionDrop.value = resolucionActual;
        resolucionDrop.RefreshShownValue();
    }

    public void CambiarResolucion(int indiceResolucion)
    {
        Resolution resolucion = resoluciones[indiceResolucion];
        Screen.SetResolution(resolucion.width, resolucion.height, Screen.fullScreen);  // Cambia la resolución según la selección del usuario
    }

    //Funcion del FOV
    public void ChangeFOV(float value)
    {
        fovValue = value;
        PlayerPrefs.SetFloat("FOV", fovValue);  // Guarda el nuevo valor del FOV en PlayerPrefs
        UpdateFOV();
    }
    private void UpdateFOV()
    {
        if (mainCamera != null)
        {
            mainCamera.fieldOfView = fovValue;  // Actualiza el campo de visión de la cámara principal
        }
    }
}
