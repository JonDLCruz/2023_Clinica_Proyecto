using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class DetectorObjeto : MonoBehaviour
{
    [SerializeField] GameObject instanciaPadre; //donde colocaremos los menus para que se vean en pantalla 
    private GameObject textoInstanciado; //el objeto instanciado
    [SerializeField] private TextMeshProUGUI titulo;
    [SerializeField] private TextMeshProUGUI descripcion;
    [SerializeField] private VideoPlayer video;
    [SerializeField] private GameObject menu;
    private string nombreObjeto; //donde pondremos luego el nombre del objeto que señalamos con el raton
    LayerMask mask; //la mascara para detectar 
    private float distancia = 3f; //ya cambiaremos esto
    public GameObject texDectect; //donde colocaremos F en el chat
    GameObject ultimoReconocido = null;
    GameObject fenChat;
    private new Transform camera;
    private new Rigidbody rigidbody;

    // Start is called before the first frame update
    void Start()
    {
        mask = LayerMask.GetMask("DetecObject"); //he llamado a la mascara que detectara si es un objeto asi.
        fenChat = Instantiate(texDectect, instanciaPadre.transform); //instanciamos F en el chat para que se encienda y se apage
        rigidbody = GetComponent<Rigidbody>(); 
        camera = transform.Find("Main Camera");
        fenChat.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {

        //Lanza un rayo desde la posicion del raton
        Debug.DrawRay(camera.position, camera.forward * distancia, Color.red);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); //para mi prueba usaremos el raton lo quito por que no me funciona con la camara principal
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, distancia, mask))
        {
            //verificamos si el rayo intercepta con el objeto en la escena
            Debug.Log("He detectado algo");
            ResaltarObjeto(hit.transform);


            //F en el chat
            if (Input.GetKeyDown(KeyCode.F))
            {
                if (hit.collider.gameObject != null)
                {
                    nombreObjeto = hit.collider.gameObject.name;

                    if (textoInstanciado == null)
                    {
                        AbrirMenuFlotante();
                    }
                    else
                    {
                        CerrarMenuFlotante();
                    }
                }
            }
        }
        else
        {
            Deseleccionado();
        }

    }

    //esto habria que cambiarlo por el Shader ... cuando se pueda
    private void ResaltarObjeto(Transform transform)
    {
        transform.GetComponent<Renderer>().material.color = Color.red;
        ultimoReconocido = transform.gameObject;
    }

    void Deseleccionado()
    {
        if (ultimoReconocido)
        {
            ultimoReconocido.GetComponent<Renderer>().material.color = Color.black;
            ultimoReconocido = null;
        }
    }


    //activar F en el chat
    private void OnGUI()
    {
        if (ultimoReconocido)
        {
            if (fenChat != null)
            {
                fenChat.SetActive(true);
            }
            else
            {
                Debug.LogWarning("El objeto fenChat no ha sido inicializado correctamente.");
                fenChat.SetActive(false);
            }

        }
        else
        {
            if (fenChat != null)
            {
                fenChat.SetActive(false);
            }
            else
            {
                Debug.LogWarning("El objeto fenChat no ha sido inicializado correctamente.");
            }
        }
    }

    void AbrirMenuFlotante()
    {
        textoInstanciado = Instantiate(menu, instanciaPadre.transform); //instanciamos el texto flotante
        titulo = GameObject.Find("Titulo").GetComponent<TextMeshProUGUI>(); //instanciamos el titulo
        descripcion = GameObject.Find("Descripcion").GetComponent<TextMeshProUGUI>(); //Instanciamos Descripcion
        video = GameObject.Find("Video Player").GetComponent<VideoPlayer>(); //se supone que aqui va el video
        titulo.text = nombreObjeto;
        InfoObjeto(nombreObjeto);
    }

    void CerrarMenuFlotante()
    {
        if (textoInstanciado != null)
        {
            Destroy(textoInstanciado);
            textoInstanciado = null;
        }
    }

    void InfoObjeto(string nombreObjeto)
    {
        //aqui va la ruta a la base de datos de los objetos 
        string textFilePath = "2023_Clinica_Proyecto/Assets/Resources/dataB"; //sustituir null por ruta
        //string videoFilePath = "Assets/Resources/Videos"; //sustituir null por ruta

        if (File.Exists(textFilePath) ) //&& File.Exists(videoFilePath)
        {
            //carga la descrcion del archivo de la base de datos
            string a = File.ReadAllText(textFilePath); //lee todo el contenido dentro de la base de datos del objeto
            descripcion.text = a;

            //carga el video
            //video.url = videoFilePath;
            //video.Play();
        }
        else
        {
            Debug.LogError("No se encontraro archivos de texto o video para el objeto: " + titulo.text);
        }
    }

}
