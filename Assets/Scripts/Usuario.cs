using UnityEngine;

[System.Serializable]
public struct Registro
{
    public int ID;
    public string user;
    public string email;
    public string password;
    public string confiPass;
    //public Baseuser baseuser;
}

[CreateAssetMenu(fileName = "ListaDeUsuarios", menuName = "Registro/Usuario", order = 1)]

public class Usuario : ScriptableObject
{
    public Registro[] registro;

    //se llama cuando se carga el scrip y cuando este se cambia en el inspector
    private void OnValidate()
    {
        if (registro != null)
        {
            for (int i = 0; i < registro.Length; i++)
            {
                if (registro[i].ID != i)
                {
                    registro[i].ID = i;
                }
            }

        }

    }

    public void AddDataBase()
    {
        if (registro != null)
        {
            for (int i = 0; i < registro.Length; i++)
            {
                if (registro[i].user == null)
                {
                    registro[i].user = "linea temporal";
                    registro[i].email = "linea temporal";
                    registro[i].password = "linea temporal";
                    registro[i].confiPass = "linea temporal";

                }
            }
        }
    }
}
