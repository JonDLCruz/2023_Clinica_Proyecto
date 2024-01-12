
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.UI;
using UnityEngine.UIElements;
using UnityEngine.Video;

public class PlayerController : MonoBehaviour
{
    //GrandMAnager 
    GameManager _GM;
    //Vamos a crear una maquina de estados para las animaciones y sonidos de el player
    public enum PlayerState { WALK, RUN, INTERACT, IDLE, CROUCH, JUMP }
    public PlayerState pState;
    //Audio 
    AudioInstance _AS;
    //Variables de movimiento y Camera
    public Vector2 sensibility;
    public new Transform camera;
    private new Rigidbody rigidbody;
    public float movmentSpeed;
    public float jumpForce = 20f;
    //Variables de restriccion de movimiento
    public bool canMove, canMoveCamera;
    private bool isTalking = false;
    //Menus Chat NPC
    public GameObject panelInfo;
    public TextMeshProUGUI _subtitles, _interactInfo;
    public GameObject _panelSubtitles, _panelActividades;
    //Menus Objetos
    public GameObject panelObjeto;
    public TextMeshProUGUI _tituloObj, _descObj;
    public VideoPlayer _vid;
    public GameObject MenuObjeto;
    //Variables control de Dialgo
    private string[] dialogueText;
    private int currentIndex = 0;
    private float dialogueTimer = 0f;
    //Rayos
    public float rayDistance;
    GameObject objInteract;

    GameObject lastHit;

    string nameDB = "", descr = "", path = "";
    bool shaderSwitch = false;
    bool rayCastObject = false;

    //Mano
    [SerializeField]
    private GameObject _mano;
    public bool isGrabing;
    public bool timeToReset;
    //Animaci�n
    private Animator animator;
    //Callcular si esta en el suelo
    public float raycastDistance = 0.1f;
    public LayerMask groundLayer;
    private Rigidbody rb;
    bool grounded;

    //Save Data
    bool act01State = false;
    bool act02State = false;
    bool act03State = false;
    int logrosObtenidos = 0;
    int actividadesRealizadas = 0;
    //void setPlayerData()
    //{
    //    act01State = DataSaveManager.LoadActivityState("Act01Key");
    //    act02State = DataSaveManager.LoadActivityState("Act02Key");
    //    act03State = DataSaveManager.LoadActivityState("Act03Key");
    //    logrosObtenidos = DataSaveManager.LoadAchivements();
    //    actividadesRealizadas = DataSaveManager.LoadAchivements(); 
    //}
    // Start is called before the first frame update
    void Start()
    {
        _GM = GameObject.Find("GameManager").GetComponent<GameManager>();
        _GM.CrearListadeObjetos(); //Funcion que se encarga de llenar la lista y actualizarla si hay cambios
        _AS = GameObject.Find("GameManager").GetComponent<AudioInstance>();
        //Set del rigidBody
        rigidbody = GetComponent<Rigidbody>();
        //Set Mano
        _mano = GameObject.Find("Mano");
        //Cursor settings
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        //Ocultamos Menu

        _panelSubtitles.SetActive(false);
        _panelActividades.SetActive(false);
        panelInfo.SetActive(false);
        MenuObjeto.SetActive(false);
        //Permitimos Movimiento
        canMove = true;
        canMoveCamera = true;
        //Prueba para interactuar con objetos
        ObjetosInteractuar("Curetas");
        isGrabing = false;
        timeToReset = false;
        pState = PlayerState.IDLE;
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        grounded = IsGrounded();
        rayCheck();
        //Controlador para poder moverse o no
        if (canMove)
            Movmetcharacter();
        //Lo mismo que el de moverse pero para camara
        if (canMoveCamera)
            CameraControl();
        //Condicion generada para poder hablar y desahabilitar las funciones anteriores
        if (isTalking)
        {
            HandleDialog();
        }
        PlayerStateMachine();
        if (isGrabing && Input.GetKeyDown(KeyCode.R))//Esto solo es para las animaciones de los objetos en mano
        {
            //Metemos la animaci�n del objeto
            ObjetosInteractuar(objInteract.name);
            PlayAnim(path);
        }
        print(pState);
    }
    void PlayAnim(string _path)
    {
        AnimationClip clip = Resources.Load<AnimationClip>(_path);
        if (clip != null)
        {
            Debug.LogError("No se pudo cargar la animaci�n desde Resources: " + _path);
            return;
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
    void rayCheck()
    {
        if (!isGrabing)
        {

            RaycastObjectInteract();

        }
        else
        {

            StopGrabing(objInteract);
        }

        RaycastNPC();

    }
    public void Movmetcharacter()
    {
        //Vamos a detectar primero los input
        bool isRunning = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
        bool isJumping = Input.GetButtonDown("Jump");
        //Input de movimiento 
        float hor = Input.GetAxisRaw("Horizontal");
        float ver = Input.GetAxisRaw("Vertical");

        Vector3 velocity = Vector3.zero;
        Vector3 direction = Vector3.zero;
        //Comprobamos que se esta moviendo para asignarle la direccion
        if (hor != 0 || ver != 0)
        {
            direction = (transform.forward * ver + transform.right * hor).normalized;
            //Como el righidbody trabaja con velocity lo vamos a meter en el vector que hemos creado anteriormente
            velocity = direction * movmentSpeed;

        }
        //aplicamos la velocidad a el rigidbody
        velocity.y = rigidbody.velocity.y;
        rigidbody.velocity = velocity;
        if (direction != Vector3.zero)
        {
            if (isRunning)
            {
                pState = PlayerState.RUN;
            }
            else
            {
                pState = PlayerState.WALK;
            }
        }
        else if (isJumping)
        {
            pState = PlayerState.JUMP;
            Jump();
        }
        else if (grounded)
        {

            pState = PlayerState.IDLE;
        }
    }

    bool IsGrounded()
    {
        // Obtener la posici�n del objeto
        Vector3 position = transform.position;

        // Lanzar un rayo hacia abajo desde la posici�n del objeto
        bool hitGround = Physics.Raycast(position, Vector3.down, raycastDistance, groundLayer);

        return hitGround;
    }
    public void Jump()
    {
        rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }
    public void PlayerStateMachine()
    {
        switch (pState)
        {
            case PlayerState.WALK:
                //Audio walk
                //_AS.SelectAudio("walk");
                //todo  lo relacionado con moverse
                break;
            case PlayerState.RUN:
                //Audio Run
                //_AS.SelectAudio("run");
                //Todo lo reacionado con correr
                break;
            case PlayerState.INTERACT:
                //Audio Depende del Objeto

                //todo lo relacionado con interactuar 
                break;
            case PlayerState.IDLE:
                //Audio idle si eso
                //_AS.SelectAudio("Idle");
                //animacion Idle
                //todo lo relacionado con el Idle
                break;
            case PlayerState.CROUCH:
                //Audio Arrastrarse
                //_AS.SelectAudio("Agachar");
                //Animacion si eso
                //todo lo relacionado con Crouch
                break;
            case PlayerState.JUMP:
                //Audio Saltar
                //_AS.SelectAudio("Saltar");
                //Animacion si eso
                // rodo lo de saltar
                break;

        }
    }
    public void RaycastNPC()
    {
        //Dibujamos el rayo para verlo en play
        Debug.DrawRay(camera.position, camera.forward * rayDistance, Color.red);
        //Creamos el hit donde sacaremos toda la insformaci�n
        RaycastHit hit;

        if (Physics.Raycast(camera.position, camera.forward, out hit, rayDistance, LayerMask.GetMask("NPC_Checker")) && !isTalking)//casteamos el rayo desde camara y comprobamos los objetos en la mascara NPC_Checker
        {
            panelInfo.SetActive(true);
            _interactInfo.text = "Pulsa E para Hablar";

            if (hit.collider.tag == "NPC" && Input.GetKeyDown(KeyCode.E) && !isTalking)//Condicion para activar el npc
            {
                print("Detecatado");
                panelInfo.SetActive(false);
                StartDialog(hit.collider.gameObject.GetComponent<NPCText>());//Le pasamos a la funcion el NPCText del NPC por ahora ser un dialogo

            }
        }
        else
        {
            panelInfo.SetActive(false);
        }

    }

    public void RaycastObjectInteract()
    {
        //Dibujamos el rayo para verlo en play
        Debug.DrawRay(camera.position, camera.forward * rayDistance, Color.blue);
        //Creamos el hit donde sacaremos toda la insformaci�n
        RaycastHit hit;

        if (Physics.Raycast(camera.position, camera.forward, out hit, rayDistance, LayerMask.GetMask("OBJ_Checker")))//casteamos el rayo desde camara y comprobamos los objetos en la mascara NPC_Checker
        {
            rayCastObject = true;
            shaderSwitch = false;
            lastHit = hit.collider.gameObject;
            switchShader(lastHit);
            MenuObjeto.SetActive(true);
            //string objName = hit.collider.GetComponent<GameObject>().name;
            //AccederObjetoLista(objName);


            //_tituloObj.text ="" + nameDB;
            //_descObj.text ="" + descr;

            //_vid.url= path;
            //_vid.Play();



            if (hit.collider.tag == "Interactable_Obj" && Input.GetMouseButton(0))//Condicion para activar el npc
            {
                objInteract = hit.collider.gameObject;
                MenuObjeto.SetActive(false);
                print("Detecatado");
                isGrabing = true;
                print("isGrabing: " + isGrabing);
                //ObjetosInteractuar(_obj.gameObject.name);//Le pasamos a la funcion el NPCText del NPC por ahora ser un dialogo
                if (isGrabing)
                {

                    print("Cambiamos el Hijo");
                    objInteract.transform.SetParent(_mano.transform, false); //0,0,0
                    objInteract.GetComponent<Rigidbody>().useGravity = false;
                    objInteract.GetComponent<Rigidbody>().isKinematic = true;
                    print("Hijo Cambiado" + hit.transform.position);
                    objInteract.transform.position = _mano.transform.position;
                    print("Hijo Junto a padre " + hit.transform.position);
                    shaderSwitch = true;
                    switchShader(objInteract);
                    objInteract.GetComponent<OBJ>().CancelResetPosition();

                }

            }



        }
        else
        {
            MenuObjeto.SetActive(false);
            if (rayCastObject)
            {
                shaderSwitch = true;
                switchShader(lastHit);
                rayCastObject = false;
            }
        }

    }
    void switchShader(GameObject _obj)
    {
        if (!shaderSwitch)
        {
            _obj.GetComponent<Renderer>().material.color = Color.red;

        }
        else
        {
            _obj.GetComponent<Renderer>().material.color = Color.white;

        }
    }
    void StopGrabing(GameObject _obj)
    {
        _obj.gameObject.GetComponent<Collider>().enabled = false;
        Debug.DrawRay(camera.position, camera.forward * rayDistance, Color.green);
        //Creamos el hit donde sacaremos toda la insformaci�n
        RaycastHit hit;

        if (Physics.Raycast(camera.position, camera.forward, out hit, rayDistance))//casteamos el rayo desde camara y comprobamos los objetos en la mascara NPC_Checker
        {

            if (hit.collider.tag != "Interactable_Obj" && hit.collider.tag != "NPC" && Input.GetMouseButton(1))//Condicion para activar el npc
            {
                isGrabing = false;
                gameObject.GetComponent<Collider>().enabled = false;
                _obj.transform.SetParent(null);
                _obj.transform.position = hit.point + new Vector3(0, 0.5f, 0);
                _obj.GetComponent<Rigidbody>().useGravity = true;
                _obj.GetComponent<Rigidbody>().isKinematic = false;
                _obj.gameObject.GetComponent<Collider>().enabled = true;
                gameObject.GetComponent<Collider>().enabled = true;
                _obj.GetComponent<OBJ>().ResetPosition();
                MenuObjeto.SetActive(false);
            }
        }
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

        _panelSubtitles.SetActive(true);
        _subtitles.text = dialogueText[currentIndex];
        print("Termino");
    }

    void HandleDialog()
    {
        //Ponemos un timer al dialogo
        dialogueTimer += Time.deltaTime;
        if (dialogueTimer >= 5)
        { //Cada 5 segundos salta a la siguiente linea

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
        dialogueText = null;
    }

    void ObjetosInteractuar(string _nombreObjeto)//Hit.Collider.gameObject.name
    {

        (nameDB, descr, path) = _GM.AccederObjetoLista(_nombreObjeto);//extraemos el nombre descr y Path de animacion
        print(nameDB);
        print(descr);
        print(path);
    }
}
