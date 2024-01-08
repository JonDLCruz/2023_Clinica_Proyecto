using Unity.VisualScripting;
using UnityEngine;




[CreateAssetMenu(fileName = "ListaDeUsuarios", menuName = "Registro/Usuario", order = 1)]

public class Usuario : ScriptableObject
{
    [System.Serializable]
    public struct Registro
    {
        public int ID;
        public string user;
        public string email;
        public string password;
        public string confiPass;
        public bool newPlayer;
        public int activityDone;
        public int logros;
        public bool activity01;
        public bool activity02;
        public bool activity03;
        public bool activity04;
        public bool activity05;
    }
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
                    registro[i].newPlayer = true;
                    registro[i].activityDone = 0;
                    registro[i].logros = 0;
                    registro[i].activity01 = false;
                    registro[i].activity02 = false;
                    registro[i].activity03 = false;
                    registro[i].activity04 = false;
                    registro[i].activity05 = false;
                    

                }
            }
        }
    }


    public Registro GetData(int id)
    {
        for (int i = 0; i < registro.Length; i++)
        {
            if (id == registro[i].ID)
            {
                return registro[i];
            }
        }

        return new Registro();
    }




}
