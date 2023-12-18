using Palmmedia.ReportGenerator.Core.Common;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public static class SessionController
{
    public static int LoggedInUserID { get; set; }
}
public class UserData
{


    public List<UserDataEntry> Users;
    public class UserDataEntry
    {
        public int ID;
        public string User;
        public string Password;
        public string Email;
        public int Achievements;
        public bool Act1;
        public bool Act2;
        public bool Act3;
    }
}

public class LogginManager : MonoBehaviour
{
    private int userCounter = 0;
    public GameObject _userLog, _userRegister, _passwordLog, _passwordRegister, _email;
    private UserData userData;
    private void Start()
    {
        CreateJSON();
        LoadData();
    }
    public void Loging()
    {
        string inputUser = _userLog.GetComponent<TextMeshProUGUI>().text;
        string inputPassword = _passwordLog.GetComponent<TextMeshProUGUI>().text;
        if (string.IsNullOrEmpty(inputUser) || string.IsNullOrEmpty(inputPassword))
        {
            Debug.Log("Ingresa un nombre de usuario y una contraseña.");
            return;
        }
        UserData.UserDataEntry user = userData.Users.Find(u => u.User == inputUser && u.Password == inputPassword);
        if (user != null)
        {
            SessionController.LoggedInUserID = user.ID;
        }
    }
    public void Registro()
    {
        string inputUser = _userRegister.GetComponent<TextMeshProUGUI>().text;
        string inputPassword = _passwordRegister.GetComponent<TextMeshProUGUI>().text;
        string inputEmail = _email.GetComponent<TextMeshProUGUI>().text;

        // Verificar si el usuario ya existe
        if (string.IsNullOrEmpty(inputUser) || string.IsNullOrEmpty(inputPassword) || string.IsNullOrEmpty(inputEmail))
        {
            Debug.Log("Completa todos los campos para registrarte.");
            return;
        }

        if (userData.Users.Exists(u => u.User == inputUser))
        {
            Debug.Log("El usuario ya existe. Por favor, elige otro nombre de usuario.");
        }
        else
        {
            // Crear un nuevo usuario
            UserData.UserDataEntry newUser = new UserData.UserDataEntry
            {
                ID = GenerateID(),
                User = inputUser,
                Password = inputPassword,
                Email = inputEmail,
                Achievements = 0,
                Act1 = false,
                Act2 = false,
                Act3 = false,
            };

            userData.Users.Add(newUser);
            SaveUserData(newUser);

            Debug.Log("Usuario creado: " + newUser.User);
        }
    }
    void CreateJSON()
    {
        string directoryPath = Path.Combine(Application.dataPath, "DB");
        string filePath = Path.Combine(directoryPath, "UserData.json");

        // Verificar si el archivo ya existe
        if (!File.Exists(filePath))
        {
            UserData data = new UserData();
            string jsonString = JsonUtility.ToJson(data);

            // Crear el archivo solo si no existe
            File.WriteAllText(filePath, jsonString);
            Debug.Log("JSON creado en: " + filePath);
        }
        else
        {
            Debug.Log("El archivo JSON ya existe en: " + filePath);
        }
    }
    void LoadData()
    {
        string path = Path.Combine(Application.dataPath, "DB");
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            userData = JsonUtility.FromJson<UserData>(json);
        }
        else //Creamos una lista de usuarios para poder trabajar sobre ella una vez emepzado la app
        {
            userData = new UserData
            {
                Users = new List<UserData.UserDataEntry>()
            };
         
        }
    }
    void SaveUserData(UserData.UserDataEntry newUser)
    {
        string directoryPath = Path.Combine(Application.dataPath, "DB");
        string filePath = Path.Combine(directoryPath, "UserData.json");

        // Verificar si la carpeta existe, si no, crearla
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }

        // Paso 1: Leer el JSON existente
        string jsonContent = File.ReadAllText(filePath);
        UserData existingData = JsonUtility.FromJson<UserData>(jsonContent);

        // Paso 2: Actualizar la lista
        existingData.Users.Add(newUser);

        // Paso 3: Guardar la lista actualizada
        string jsonActualizado = JsonUtility.ToJson(existingData);
        File.WriteAllText(filePath, jsonActualizado);

        print("Usuario guardado en JSON: " + newUser.User);
    }
    int GenerateID()
    {
        return userData.Users.Count + 1;
    }
}
