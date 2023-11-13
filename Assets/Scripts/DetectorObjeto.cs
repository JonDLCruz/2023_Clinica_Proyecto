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
    private Transform camera;
    private Rigidbody rigidbody;
    private bool menuAbierto = false;

    // Start is called before the first frame update
    void Start()
    {
        mask = LayerMask.GetMask("DetecObject"); //he llamado a la mascara que detectara si es un objeto asi.
        rigidbody = GetComponent<Rigidbody>();
        camera = transform.Find("Main Camera");
        if (fenChat == null)
        {
            fenChat = Instantiate(texDectect, instanciaPadre.transform); //instanciamos F en el chat para que se encienda y se apage
        }
        fenChat.SetActive(false);
        video = GetComponent<VideoPlayer>();

    }

    // Update is called once per frame
    void Update()
    {

        //Lanza un rayo desde la posicion del raton
        Debug.DrawRay(camera.position, camera.forward * distancia, Color.red);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); //para mi prueba usaremos el raton lo quito por que no me funciona con la camara principal
        RaycastHit hit;

        bool objetoDetectado = Physics.Raycast(ray, out hit, distancia, mask);


        if (objetoDetectado)
        {
            //verificamos si el rayo intercepta con el objeto en la escena
            ResaltarObjeto(hit.transform);
            nombreObjeto = hit.collider.gameObject.name;
        }
        else
        {
            Deseleccionado();
        }

        //F en el chat
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (menuAbierto)
            {
                CerrarMenuFlotante();

            }
            else if (objetoDetectado)
            {
                AbrirMenuFlotante();
            }

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
            if (fenChat != null && !menuAbierto) //si el cartel de F esta deactivado y menu abierto quitado al tocar un objeto se activa
            {
                fenChat.SetActive(true);
            }
            else
            {
                if (fenChat.activeSelf)
                { //activeSelf se comprueba si esta activado, si algunas de las dos condiciones que la activaron ya no se cumple, se desactiva
                    fenChat.SetActive(false);
                }
            }

        }
        else
        {
            if (fenChat.activeSelf)
            {
                fenChat.SetActive(false);
            }

        }
    }

    void AbrirMenuFlotante()
    {

        Transform transP = instanciaPadre.transform; //accedemos al transforn del padre
        menuAbierto = true;
        float xPro = Screen.width * 0.280f;
        float yPro = Screen.height * -0.1f;
        Vector3 posPant = new Vector3(xPro, yPro, 0f); //
        Vector3 posicionMundo = Camera.main.ScreenToViewportPoint(posPant);
        textoInstanciado = Instantiate(menu, posicionMundo, Quaternion.identity); //instanciamos el texto flotante
        textoInstanciado.transform.SetParent(transP); // Establece el padre del objeto instanciado como transP
        textoInstanciado.transform.localPosition = posPant;
        textoInstanciado.transform.localScale = Vector3.one; //Ajusta la escala si es necesario
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
            menuAbierto = false;
        }
    }

    void InfoObjeto(string nombreObjeto)
    {
        //aqui va la ruta a la base de datos de los objetos 
        string textFilePath = "Assets/Resources/dataB/" + nombreObjeto + ".txt"; //sustituir null por ruta
        string videoFilePath = "Assets/Resources/Videos/" + nombreObjeto + ".mp4"; //sustituir null por ruta

        if (File.Exists(textFilePath) && File.Exists(videoFilePath))
        {
            //carga la descrcion del archivo de la base de datos
            string a = File.ReadAllText(textFilePath); //lee todo el contenido dentro de la base de datos del objeto
            descripcion.text = a;

            //carga el video
            video.url = videoFilePath;
            video.Play();
        }
        else
        {
            Debug.LogError("No se encontraro archivos de texto o video para el objeto: " + titulo.text);
        }
    }
}




