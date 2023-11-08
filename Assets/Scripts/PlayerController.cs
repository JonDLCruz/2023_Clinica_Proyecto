
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    //Variables de movimiento y Camera
    public Vector2 sensibility;
    public new Transform camera;
    private new Rigidbody rigidbody;
    public float movmentSpeed;
    //Variables de restriccion de movimiento
    public bool canMove, canMoveCamera;
    private bool isTalking = false;
    //Menus Chat NPC
    public TextMeshProUGUI _subtitles;
    public GameObject _actividades, _panelSubtitles, _panelActividades;
    //Variables control de Dialgo
    private string[] dialogueText;
    private int currentIndex = 0;
    private float dialogueTimer = 0f;
    //Rayos
    public float rayDistance;
    //Listas
    List<InteractableObj> lista = new List<InteractableObj>();

    // Start is called before the first frame update
    void Start()
    {
        //Set del rigidBody
        rigidbody = GetComponent<Rigidbody>();
        //Cursor settings
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        //Ocultamos Menu
        _actividades.SetActive(false);
        _panelSubtitles.SetActive(false);
        _panelActividades.SetActive(false);
        //Permitimos Movimiento
        canMove = true;
        canMoveCamera = true;
       //Prueba para interactuar con objetos
        ObjetosInteractuar("Cuchara");
    }

    // Update is called once per frame
    void Update()
    {
        //Rayo que utilizamos para detectar NPCS y hablar con ellos
        RaycastNPC();
        //Controlador para poder moverse o no
        if (canMove)
        Movmetcharacter();
        //Lo mismo que el de moverse pero para camara
        if (canMoveCamera)
        CameraControl();
        //Condicion generada para poder hablar y desahabilitar las funciones anteriores
        if(isTalking)
        {
            HandleDialog();
        }


    }

    public void CameraControl()
    {
        //Inputs
        float hor = Input.GetAxis("Mouse X");
        float ver = Input.GetAxis("Mouse Y");
        //Comprobamos que se mueve en orizontal
        if (hor != 0)
        {
            transform.Rotate(Vector3.up * hor * sensibility.x);//Recordad que el movimiento de camara tiene que ir con el movimiento del jugador por eso usamos el transform del player
        }
        //Comprovamos movimiento Vertical
        if (ver != 0)
        {
            //Formua para calcular el angulo actual de la camara
            float angle = (camera.localEulerAngles.x - ver * sensibility.y + 360) % 360;
            //Condicion y clampeo de la camara.
            if (angle > 180)
            {
                angle -= 360;
            }
            angle = Mathf.Clamp(angle, -80, 80);
            camera.localEulerAngles = Vector3.right * angle;
        }
    }

    public void Movmetcharacter()
    {
        //Input de movimiento 
        float hor = Input.GetAxisRaw("Horizontal");
        float ver = Input.GetAxisRaw("Vertical");

        Vector3 velocity = Vector3.zero;
        //Comprobamos que se esta moviendo para asignarle la direccion
        if (hor != 0 || ver != 0)
        {
            Vector3 direction = (transform.forward * ver + transform.right * hor).normalized;
            //Como el righidbody trabaja con velocity lo vamos a meter en el vector que hemos creado anteriormente
            velocity = direction * movmentSpeed;

        }
        //aplicamos la velocidad a el rigidbody
        velocity.y = rigidbody.velocity.y;
        rigidbody.velocity = velocity;
    }

    
    public void RaycastNPC()
    {
        //Dibujamos el rayo para verlo en play
        Debug.DrawRay(camera.position, camera.forward * rayDistance, Color.red);
        //Creamos el hit donde sacaremos toda la insformación
        RaycastHit hit;

        if (Physics.Raycast(camera.position, camera.forward, out hit, rayDistance, LayerMask.GetMask("NPC_Checker")))//casteamos el rayo desde camara y comprobamos los objetos en la mascara NPC_Checker
        {

            if (hit.collider.tag == "NPC" && Input.GetMouseButton(0) && !isTalking)//Condicion para activar el npc
            {
                print("Detecatado");
                    StartDialog(hit.collider.gameObject.GetComponent<NPCText>());//Le pasamos a la funcion el NPCText del NPC por ahora ser un dialogo
                
            }
        }
        ;
    }
    
    void StartDialog(NPCText _npc)
    {
        //Esta funcion la utilizamo para leer el texto del NPC y mostrarlo por pantalla y Activar y desactivar los componentes que necesitamos
        print("Entro");
        isTalking = true;
        canMove = false;
        canMoveCamera = false;
        UnityEngine.Cursor.lockState = CursorLockMode.None;//Liberamos el raton para que el usuario pueda seleccionar las opciones que vamos a mostrar
        dialogueText = _npc.arrayText;
        currentIndex = 0;
        _actividades.SetActive(true);
        _panelSubtitles.SetActive(true);
        _subtitles.text = dialogueText[currentIndex];
        print("Termino");
    }
  
    void HandleDialog()
    {
        //Ponemos un timer al dialogo
        dialogueTimer += Time.deltaTime;
        if (dialogueTimer >= 5) { //Cada 5 segundos salta a la siguiente linea

            dialogueTimer = 0f;
            currentIndex++;//aumentamos la posiscion de la array para leer la siguiente linea
            if (currentIndex < dialogueText.Length)
            {
                _subtitles.text = dialogueText[currentIndex];//Mostramos el dialogo
                _panelActividades.SetActive(true);//Acticamos el panel de acticidades
            }
            else
            {
                    EndDialogue();//terminamos en caso de que el index sobrepase la longitud del Dialogo
            }
        }
    }
    void EndDialogue()
    {
        //volvemos a dejar todos los menus en oculto y a poder caminar
        isTalking = false;
        canMove = true;
        canMoveCamera = true;
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        _panelActividades.SetActive(false);
        _panelSubtitles.SetActive(false);
        _actividades.SetActive(false);
        dialogueText = null;
    }

    void ObjetosInteractuar(string _nombreObjeto)//Hit.Collider.gameObject.name
    {
        CrearListadeObjetos(); //Funcion que se encarga de llenar la lista y actualizarla si hay cambios
        (string name, string descr, string path) = AccederObjetoLista(_nombreObjeto);//extraemos el nombre descr y Path de animacion
        print(name);
        print(descr);
        print(path);
    }
     void CrearListadeObjetos()//Crea lisa de objetos
    {
        lista.Clear();
        lista.Add(new InteractableObj("Cuchara", "Se usa para la sopa", "Assets\\Resources\\Animations\\Cuchara.anim"));
        lista.Add(new InteractableObj("Tenedor", "El cubierto favorito de Poseidón", "Assets\\Resources\\Animations\\Tenedor.anim"));
        lista.Add(new InteractableObj("Cuchillo", "Se usa para cortar, ¿El que? lo dejo en tu mano", "Assets\\Resources\\Animations\\Cuchillo.anim"));
    }
    public (string, string, string) AccederObjetoLista(string _name)
    {
        InteractableObj objFind = lista.Find(objeto => objeto.name == _name);//Condicion para extraer el objeto con el nombre que le pasemos por parametro
        if (objFind != null)
        {
            return (objFind.name, objFind.descr, objFind.AnimationPath);//Lo devolvemos en tres sstrings diferentes RECUERDA USAR ESTE MISMO ORDEN CUANDO LO PASES A UNA VARIABLE
        }
        else
        {
            throw new System.Exception("No ta");//ERROR: No encuentra objeto
        }
    }
}
