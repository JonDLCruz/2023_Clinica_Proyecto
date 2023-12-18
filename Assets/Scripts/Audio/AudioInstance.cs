using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioInstance : MonoBehaviour
{
    //hacemos esto para que haga una instancia de esta clase y pueda utilizarse en otros codigos.
    public static AudioInstance instance;
    //Esta es la lista donde almacenamos los clips
    public List<AudioClip> _AC;
    //Vamos a usar la siguiente variable para almacenar nuestro audiosource
    private AudioSource _AS;
    //Usamos Awake para que se ejecte antes de cualquier start, haciendo que se ejecute antes de la maypria de scripts
    private void Awake()
    {
        //esto de aqi es un patron de diseño Singleton, hace que solo exista una misma instancia por escena
        if (instance == null)//Significa que no existe
        {
            instance = this;//Crea la instancia
        }
        else//Significa que existe
        {
            Destroy(gameObject);//lo destruye
            return;//volvemos a empezar para que cree una instancia
        }
        //ahora vamos a rellentar las variables de arriba
        _AS = GameObject.Find("Player").GetComponent<AudioSource>(); //Le decimos que coja el componente llamado AudioSource
        _AC = new List<AudioClip>();//Inicializamos la lista
        //Carga todos los clips que hay en la carpeta Audios
        AudioClip[] clips = Resources.LoadAll<AudioClip>("Audios");
        //añadimos los clips todos los Audios
        _AC.AddRange(clips);
        _AS.volume = 0f;
    }
    //ahora para reproducir un audio tenemos que crear el siguiente metodo
    //TENEMOS QUE PONER BIEN EL NOMBRE DEL CLIP PARA QUE SE REPRODUZCA SINO NO FUNCIONA
    public void SelectAudio(string _name)
    {
        /*usamos Find para buscar el clip que queremos en la lista de objeto AudioClip
        Lo que hay dentro del parentesis es una funcion que nos va a comparar el nombre del clip.name con _name
        Si la condicion se cumple se llena Clip sino sera null*/
        AudioClip Clip = _AC.Find(clip => clip.name == _name);
        //ahora solo hacemos una condicion sencilla para reproducirlo
        if (Clip != null) //si no esta vacio
        {
            _AS.clip = Clip;//Le pasamos el clip que vamos a reproducir al AudioSource
            _AS.Play();//Reproduce Audio
        }
        else//si esta vacio, podemos sacar un warning o lo que querais sacar
        {
            print("Clip no encontrado");
        }
    }
    public void Stop(string _name)
    {
        AudioClip Clip = _AC.Find(clip => clip.name == _name);
        if (_AS != null)
        {
            _AS.Stop();
        }
    }
    public float GetVolume()
    {
        if (_AS != null)
        {
            return _AS.volume;
        }
        return 0f;//que el valor predeterminado sea 0 por si no encuentra el AS
    }
    public void SetVolume(float _t)
    {
        if (_AS != null)
        {
            _AS.volume = _t;
        }
    }
}
