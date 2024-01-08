using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataSaveManager : MonoBehaviour
{
    //datos que vamos a guardar son : actividades totales realizadas, booleanas de actividades, logros obtenidos
    private const int Logros = 0;
    private const int ActividadesRealizadas = 0;
    
    public const string Act01Key = "Act01";
    public const string Act02Key = "Act02";
    public const string Act03Key = "Act03";

    // Función para guardar el estado de una actividad
    public void SaveActivityState(string key, bool value)
    {
        SaveBoolean(key, value);
    }

    // Función para cargar el estado de una actividad
    public bool LoadActivityState(string key)
    {
        return LoadBoolean(key);
    }

    // Función genérica para guardar un valor booleano
    private void SaveBoolean(string key, bool value)
    {
        // Convierte el booleano a un entero (0 o 1) antes de guardarlo
        int intValue = value ? 1 : 0;
        PlayerPrefs.SetInt(key, intValue);
    }

    // Función genérica para cargar un valor booleano
    private bool LoadBoolean(string key)
    {
        // Carga el entero y lo convierte de nuevo a un booleano
        int intValue = PlayerPrefs.GetInt(key, 0);
        return intValue == 1;
    }

    public void SaveLogros(int _logros)
    {
        PlayerPrefs.SetInt("Logros", _logros);
    }
    public int LoadAchivements()
    {
        return PlayerPrefs.GetInt("Logros",0);
    }
    public void SaveActividades(int _actividades)
    {
        PlayerPrefs.SetInt("Actividades", _actividades);
    }
    public int LoadActividades()
    {
        return PlayerPrefs.GetInt("Actividades", 0);
    }
   
}

