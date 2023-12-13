using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveData : MonoBehaviour
{
    public struct DataSave
    {
        public int id;
        public string user;
        public string password;
        public int activityDone;
        public int logros;
        public bool activity_01;
        public bool activity_02;
        public bool activity_03;
        public bool activity_04;
        public bool activity_05;
    }
    public DataSave GetData(int _id, string _user)
    {
        DataSave dataSave = new DataSave();
        //For de la bd que hemos creado y comprobamos la id 
        //extraemos ese objeto
        return dataSave; 
    }
    public SaveData(DataSave _data)
    {
        //Acedemos a la base de datos y mediante el ID del _data cambiamos el item de esa posición.
    }
}
