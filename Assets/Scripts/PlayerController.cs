
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.HID;
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
    GameObject objInteract;
    //Listas
    List<InteractableObj> lista = new List<InteractableObj>();
    //interactable obj
    string nameDB = "", descr = "", path = "";
    //Mano
    [SerializeField]
    private GameObject _mano;
    private bool isGrabing;

    // Start is called before the first frame update
    void Start()
    {
        //Set del rigidBody
        rigidbody = GetComponent<Rigidbody>();
        //Set Mano
        _mano = GameObject.Find("Mano");
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
        ObjetosInteractuar("Curetas");
        isGrabing = false;
    }

    // Update is called once per frame
    void Update()
    {
        //Rayo que utilizamos para detectar NPCS y hablar con ellos
        RaycastNPC();
        if (!isGrabing)
        {
            RaycastObjectInteract();
        }
        else
        {
            StopGrabing(objInteract);
        }
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
        //Creamos el hit donde sacaremos toda la insformaci�n
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

    public void RaycastObjectInteract()
    {
        //Dibujamos el rayo para verlo en play
        Debug.DrawRay(camera.position, camera.forward * rayDistance, Color.blue);
        //Creamos el hit donde sacaremos toda la insformaci�n
        RaycastHit hit;

        if (Physics.Raycast(camera.position, camera.forward, out hit, rayDistance, LayerMask.GetMask("OBJ_Checker")))//casteamos el rayo desde camara y comprobamos los objetos en la mascara NPC_Checker
        {

            if (hit.collider.tag == "Interactable_Obj" && Input.GetMouseButton(0))//Condicion para activar el npc
            {
                objInteract = hit.collider.gameObject;
                
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
                }
               
            }
            
              

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

            if (hit.collider.tag != "Interactable_Obj" && hit.collider.tag != "NPC" && Input.GetMouseButton(1) )//Condicion para activar el npc
            {
                //_obj.GetComponent<OBJ>().ResetPosition();
                gameObject.GetComponent<Collider>().enabled = false;
                _obj.transform.SetParent(null);
                _obj.transform.position = hit.point + new Vector3(0, 0.5f, 0);
                _obj.GetComponent<Rigidbody>().useGravity = true;
                objInteract.GetComponent<Rigidbody>().isKinematic = false;
                _obj.gameObject.GetComponent<Collider>().enabled = true;
                gameObject.GetComponent<Collider>().enabled = true;
                isGrabing = false;
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
        _actividades.SetActive(true);
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
        _actividades.SetActive(false);
        dialogueText = null;
    }

    void ObjetosInteractuar(string _nombreObjeto)//Hit.Collider.gameObject.name
    {
        CrearListadeObjetos(); //Funcion que se encarga de llenar la lista y actualizarla si hay cambios
        (nameDB, descr, path) = AccederObjetoLista(_nombreObjeto);//extraemos el nombre descr y Path de animacion
        print(nameDB);
        print(descr);
        print(path);
    }
    void CrearListadeObjetos()//Crea lisa de objetos
    {
        lista.Clear();
        lista.Add(new InteractableObj("Carpeta de Historia cl�nica", "documento que recoge toda la informaci�n referente a la salud dental de un paciente", "Assets\\Resources\\Animations\\Cuchara.anim"));
        lista.Add(new InteractableObj("Ficha dental", "es una c�dula que posee un sistema de anotaci�n, un esquema dentario y pautas destinadas para consignar datos de inter�s profesional", "Assets\\Resources\\Animations\\Tenedor.anim"));
        lista.Add(new InteractableObj("Hoja de anamnesis", "Se usa para cortar, �El que? lo dejo en tu mano", "Assets\\Resources\\Animations\\Cuchillo.anim"));
        lista.Add(new InteractableObj("Hoja de exploraci�n f�sica", "examen visual se realiza siempre desde fuera (empezando por la cara) hacia dentro y abarca toda la mucosa oral.", "Assets\\Resources\\Animations\\Cuchillo.anim"));
        lista.Add(new InteractableObj("Hoja de consentimiento informado", "Es la decisi�n libre y voluntaria realizada por un paciente, donde acepta las acciones diagn�sticas y terap�uticas sugeridas por su m�dico.", "Assets\\Resources\\Animations\\Cuchillo.anim"));
        lista.Add(new InteractableObj("Ficha dental", "es uno de los registros m�s confiables tanto para la identificaci�n m�dico-legal como para la judicial, ya que se basa en criterios cient�ficos comprobados para su aplicaci�n en estos contextos.", "Assets\\Resources\\Animations\\Cuchillo.anim"));
        lista.Add(new InteractableObj("Bandeja de instrumental", "bandeja donde se colocan todos los elementos imprescindibles para llevar a cabo una consulta dental y es manejada por los auxiliares de odontolog�a", "Assets\\Resources\\Animations\\Cuchillo.anim"));
        lista.Add(new InteractableObj("Agujas", "e trata de agujas libres de latex indicadas para la anestesia de conducci�n y la anestesia de infiltraci�n", "Assets\\Resources\\Animations\\Cuchillo.anim"));
        lista.Add(new InteractableObj("Carpules de anestesia", "cilindro de cristal que contiene el anest�sico local, entre otros ingredientes", "Assets\\Resources\\Animations\\Cuchillo.anim"));
        lista.Add(new InteractableObj("Anestesia para carpule", "es un instrumento de aplicaci�n de la anestesia local en las intervenciones odontol�gicas", "Assets\\Resources\\Animations\\Cuchillo.anim"));
        lista.Add(new InteractableObj("Jeringas desechables", "Jeringas de un solo uso", "Assets\\Resources\\Animations\\Cuchillo.anim"));
        lista.Add(new InteractableObj("Composites", "resinas compuestas o composites son materiales sint�ticos mezclados heterog�neamente formando un compuesto que en Odontolog�a se utiliza para reparar piezas dentales da�adas por caries o traumatismos,", "Assets\\Resources\\Animations\\Cuchillo.anim"));
        lista.Add(new InteractableObj("Gel de �cido ortofosf�rico", "material de tipo resinoso cuya finalidad es proporcionarle una mejor superficie de adhesi�n a los materiales de restauraci�n en la pieza dental", "Assets\\Resources\\Animations\\Cuchillo.anim"));
        lista.Add(new InteractableObj("Agente de uni�n l�quido", "sustancia qu�mica presentada en forma de Primer y Activador, destinada a la formaci�n de una capa qu�micamente compatible entre porcelanas y cementos resinosos, aumentando su adhesividad.", "Assets\\Resources\\Animations\\Cuchillo.anim"));
        lista.Add(new InteractableObj("Pistola de compsite", "Accesorio indispensable para la aplicaci�n de materiales de restauraci�n como lo es el composite en compules.", "Assets\\Resources\\Animations\\Cuchillo.anim"));
        lista.Add(new InteractableObj("L�mpara de fotopolimerizaci�n", "es un tipo de accesorio dental que utiliza la resina dental que se utiliza en la creaci�n de pr�tesis dentales.", "Assets\\Resources\\Animations\\Cuchillo.anim"));
        lista.Add(new InteractableObj("Gutapercha", "es un material flexible y aislante usado para el relleno del conducto radicular despu�s de ser limpiado", "Assets\\Resources\\Animations\\Cuchillo.anim"));
        lista.Add(new InteractableObj("Cemento sellador", "es un biomaterial compuesto por una mezcla de diferentes componentes que se aplica entre dos superficies hasta adquirir suficiente firmeza", "Assets\\Resources\\Animations\\Cuchillo.anim"));
        lista.Add(new InteractableObj("Kit de exploraci�n", "Kit con instrumental basico para la exploraci�N oral", "Assets\\Resources\\Animations\\Cuchillo.anim"));
        lista.Add(new InteractableObj("Espejo", "es un instrumental utilizado en la exploraci�n de la cavidad bucal del paciente a la vez que separa las paredes de la boca para ampliar el campo de visi�n", "Assets\\Resources\\Animations\\Cuchillo.anim"));
        lista.Add(new InteractableObj("Sonda", "Instrumento que eval�a el nivel de infecci�n que posee la dentadura de una persona", "Assets\\Resources\\Animations\\Cuchillo.anim"));
        lista.Add(new InteractableObj("Pinza", " Instrumento quir�rgico se utiliza para sujetar tejidos pesados. Suele ser de acero inoxidable y tiene varios tama�os diferentes. Es un instrumento vers�til porque puede sujetar tejidos gruesos y densos.", "Assets\\Resources\\Animations\\Cuchillo.anim"));
        lista.Add(new InteractableObj("Posicionador radiogr�fico intraoral", "soporte fundamental que permite la ubicaci�n ideal de la cavidad bucal del paciente durante las radiograf�as intraorales", "Assets\\Resources\\Animations\\Cuchillo.anim"));
        lista.Add(new InteractableObj("Esc�ner dental", "dispositivo que se emplea para escanear el interior de nuestra boca y recrearla en formato digital 3D, para poder trabajar sobre esta.", "Assets\\Resources\\Animations\\Cuchillo.anim"));
        lista.Add(new InteractableObj("Pel�cula radiogr�fica", "pel�cula o l�mina radiogr�fica de acetato que sirve como receptor de imagen de la luz emitida por los rayos X", "Assets\\Resources\\Animations\\Cuchillo.anim"));
        lista.Add(new InteractableObj("contra�ngulo", "pieza de mano que se utiliza acoplada a un micromotor para trabajar en la boca a baja velocidad y gran torque.", "Assets\\Resources\\Animations\\Cuchillo.anim"));
        lista.Add(new InteractableObj("Fresas", "nstrumentos met�licos empleados en odontolog�a para cortar, pulir y tallar las superficies dentales o eliminar la caries.", "Assets\\Resources\\Animations\\Cuchillo.anim"));
        lista.Add(new InteractableObj("Sonda de endodoncia DG16", "es una herramienta esencial en endodoncia para la evaluaci�n y tratamiento de los conductos radiculares", "Assets\\Resources\\Animations\\Cuchillo.anim"));
        lista.Add(new InteractableObj("Hipoclorito de sodio", "Hipoclorito de sodio", "Assets\\Resources\\Animations\\Cuchillo.anim"));
        lista.Add(new InteractableObj("Limas de endodoncia", "serie de instrumentos que se utilizan para tratar el conducto radicular de las piezas dentales.", "Assets\\Resources\\Animations\\Cuchillo.anim"));
        lista.Add(new InteractableObj("Regla de endodoncia", "son un instrumental utilizado por las y los odont�logos para medir la longitud de las limas, puntas de papel y puntas de Gutapercha.", "Assets\\Resources\\Animations\\Cuchillo.anim"));
        lista.Add(new InteractableObj("Topes de goma", "Los topes impedir�n cerrar totalmente la boca para que los dientes no toquen el aparato nuevo.", "Assets\\Resources\\Animations\\Cuchillo.anim"));
        lista.Add(new InteractableObj("Caja de endodoncia", "se utilizan para organizar el instrumental de endodoncia al estar divididos en compartimentos.", "Assets\\Resources\\Animations\\Cuchillo.anim"));
        lista.Add(new InteractableObj("Esponjero", "soporte para apoyo de limas endod�nticas", "Assets\\Resources\\Animations\\Cuchillo.anim"));
        lista.Add(new InteractableObj("Motor de endodoncia", "aparatos destinados a facilitar el proceso de endodoncia, ya que permiten utilizar limas accionadas mec�nicamente", "Assets\\Resources\\Animations\\Cuchillo.anim"));
        lista.Add(new InteractableObj("Ultrasonidos o instrumento subs�nico", "herramienta equipada con una punta vibratoria que permite eliminar el sarro o c�lculo acumulado en el interior de las enc�as y en la superficie dental.", "Assets\\Resources\\Animations\\Cuchillo.anim"));
        lista.Add(new InteractableObj("Curetas", "se utilizan para llevar a cabo las limpiezas de boca y los curetajes, cuya finalidad es eliminar la placa bacteriana y el sarro acumulado en la l�nea de las enc�as y por debajo de la misma.", "Assets\\Resources\\Animations\\Cuchillo.anim"));
        lista.Add(new InteractableObj("Hoces", "tiene un dise�o de secci�n triangular y dos bordes cortantes, con una punta aguda. Por estas razones, no puede utilizarse para eliminar el c�lculo subgingival, ya que lesionar�a la enc�a.", "Assets\\Resources\\Animations\\Cuchillo.anim"));
        lista.Add(new InteractableObj("Sonda periodontal", "El uso de este instrumento es habitual en periodoncia, se trata de una sonda calibrada que permite tomar referencias del estado periodontal, profundidad de la enc�a y bolsas periodontales.", "Assets\\Resources\\Animations\\Cuchillo.anim"));
        lista.Add(new InteractableObj("Gasas", "comprimen la herida y la inmovilizan para una pronta recuperaci�n sin dolor. Tambi�n estas gasas nos ayudan a cicatrizaci�n gracias a la regulaci�n del calor. ", "Assets\\Resources\\Animations\\Cuchillo.anim"));
        lista.Add(new InteractableObj("Gasas est�riles", "Las gasas de algod�n est�riles son compresas de algod�n 100%, utilizadas como absorbentes en hemorragias, para facilitar la aplicaci�n de productos en todo tipo de curas", "Assets\\Resources\\Animations\\Cuchillo.anim"));
        lista.Add(new InteractableObj("Clorhexidina", "desinfectante oral de acci�n antis�ptica", "Assets\\Resources\\Animations\\Cuchillo.anim"));
        lista.Add(new InteractableObj("Pastas de profilaxis", "mezcla de abrasivos blandos de grano fino en base de glicerina que limpia y pule la superficie de los �rganos dentarios sin rayarlos,", "Assets\\Resources\\Animations\\Cuchillo.anim"));
        lista.Add(new InteractableObj("Cepillo de naylon", "Se encargan de eliminaci�n de la placa dental en superficies oclusales, c�spides y pendientes cusp�deas,con pasta de pulido. Las flexibles cerdas de nylon permiten la limplieza incluso bajo las enc�as.", "Assets\\Resources\\Animations\\Cuchillo.anim"));
        lista.Add(new InteractableObj("Tiras de acetato de grano fino", "son un instrumento utilizado en profilaxis dental por los odont�logos para el pulido de las superficies interdentales de contacto estrecho para un acabado perfecto", "Assets\\Resources\\Animations\\Cuchillo.anim"));
        lista.Add(new InteractableObj("Hilo dental", "es un filamento de un grosor muy fino destinado a eliminar los restos de comida y las bacterias que se acumulan all� donde el cepillo no puede llegar", "Assets\\Resources\\Animations\\Cuchillo.anim"));
        lista.Add(new InteractableObj("Cubetas de fl�or", "Cubetas desechables que se utilizan en odontolog�a para la fluorizaci�n", "Assets\\Resources\\Animations\\Cuchillo.anim"));
        lista.Add(new InteractableObj("Sindesmotomo", "instrumento utilizado en cirug�a odontol�gica para separar y cortar el tejido periodontal y fibras desmodontales antes de extraer la pieza dental", "Assets\\Resources\\Animations\\Cuchillo.anim"));
        lista.Add(new InteractableObj("Botadores", "instrumento utilizado en odontolog�a para extraer o movilizar dientes o ra�ces dentarias", "Assets\\Resources\\Animations\\Cuchillo.anim"));
        lista.Add(new InteractableObj("F�rceps", "instrumentos, basados en el principio de la palanca de segundo grado, con forma de tenaza usados en el proceso de exodoncia, es decir, para la extracci�n de piezas dentales.", "Assets\\Resources\\Animations\\Cuchillo.anim"));
        lista.Add(new InteractableObj("Cucharilla de legrado", "Una cucharilla de legrado es un excavador dental. �stos son utilizados, como dice su nombre, para excavar las superficies de los dientes y poder detectar y eliminar caries.", "Assets\\Resources\\Animations\\Cuchillo.anim"));
        lista.Add(new InteractableObj("C�nulas de aspiraci�n", "comprimen la herida y la inmovilizan para una pronta recuperaci�n sin dolor. Tambi�n estas gasas nos ayudan a cicatrizaci�n gracias a la regulaci�n del calor. instrumento dise�ado para aspirar el exceso de sangre, fluidos y desechos de mayor volumen que se acumulan durante las cirug�as en la cavidad oral.", "Assets\\Resources\\Animations\\Cuchillo.anim"));
        lista.Add(new InteractableObj("Espejos intraorales", "es un instrumental utilizado en la exploraci�n de la cavidad bucal del paciente a la vez que separa las paredes de la boca para ampliar el campo de visi�n", "Assets\\Resources\\Animations\\Cuchillo.anim"));
        lista.Add(new InteractableObj("Separadores de Farabeuf", "nstrumento dental para efectuar la separaci�n de las paredes de una cavidad o labios de una herida, debido a su parte fina y parte ancha.", "Assets\\Resources\\Animations\\Cuchillo.anim"));
        lista.Add(new InteractableObj("Separadores de Langenbeck", " se utiliza para hacer visibles los planos profundos del campo operatorio. Este separador es un instrumento con la parte activa plana, formando un �ngulo aproximadamente de 90� respecto a la parte medial del mismo.", "Assets\\Resources\\Animations\\Cuchillo.anim"));
        lista.Add(new InteractableObj("Hoja de bisturi: 11", "presentan un borde recto que culmina en una punta de gran filo para realizar incisiones m�s precisas.", "Assets\\Resources\\Animations\\Cuchillo.anim"));
        lista.Add(new InteractableObj("Hoja de bisturi: 12", "hoja de bistur� peque�a, puntiaguda en forma de media luna afilada a lo largo del filo interior de la curva. ", "Assets\\Resources\\Animations\\Cuchillo.anim"));
        lista.Add(new InteractableObj("Hoja de bisturi: 15", "tiene un peque�o filo cortante curvado y es la hoja de bistur� con la forma m�s popular, perfecta para realizar incisiones cortas y precisas", "Assets\\Resources\\Animations\\Cuchillo.anim"));
        lista.Add(new InteractableObj("Hoja de bisturi: 15C", "instrumento met�lico de filo cortante con forma de cuchillo peque�o. De hoja fina, ligeramente curvada, puntiaguda y de un corte.", "Assets\\Resources\\Animations\\Cuchillo.anim"));
        lista.Add(new InteractableObj("Despegador o periostotomo", "instrumento utilizado en periodoncia odontol�gica para despegar el periostio, sujetando y separando el colgajo de tal manera que el dentista consiga tener un campo visual amplio de trabajo", "Assets\\Resources\\Animations\\Cuchillo.anim"));
        lista.Add(new InteractableObj("Tijeras", "Instrumentos de corte de diferentes tama�os y longitudes para las diferentes especialidades orales", "Assets\\Resources\\Animations\\Cuchillo.anim"));
        lista.Add(new InteractableObj("Pinzas de Adson", "Se usa para cortar, �El que? lo dejo en tu mano", "Assets\\Resources\\Animations\\Cuchillo.anim"));
        lista.Add(new InteractableObj("Pinzas de mosquito recta", "es utilizada para colocar ligaduras de alambre o el�sticas, y para la compresi�n de vasos sangu�neos superficiales y de peque�o tama�o.", "Assets\\Resources\\Animations\\Cuchillo.anim"));
        lista.Add(new InteractableObj("Pinzas de curva", "se coloca en la mano derecha y tiene como funci�n principal el coger la extensi�n del bl�ster de pesta�as, a�adirle una peque�a cantidad de adhesivo y colocarla sobre nuestra pesta�a natural.", "Assets\\Resources\\Animations\\Cuchillo.anim"));
        lista.Add(new InteractableObj("Portaagujas", "nstrumento utilizado en cirug�a para la sujeci�n de la aguja de sutura y realizar los puntos de sutura en el paciente", "Assets\\Resources\\Animations\\Cuchillo.anim"));
        lista.Add(new InteractableObj("Sutura", "maniobra que realiza el dentista con la finalidad de reunir dos tejidos separados por traumatismos varios o debido a una incisi�n.", "Assets\\Resources\\Animations\\Cuchillo.anim"));
        lista.Add(new InteractableObj("Motor de implantes", "aparatos el�ctricos indispensables en el proceso de colocaci�n de los implantes dentales. Est�n compuestos por una consola central, un pedal de control y un contra �ngulo conectado a un micromotor.", "Assets\\Resources\\Animations\\Cuchillo.anim"));
        lista.Add(new InteractableObj("Contra�ngulo de implantes", "es una pieza de mano que se utiliza acoplada a un micromotor para trabajar en la boca a baja velocidad y gran torque", "Assets\\Resources\\Animations\\Cuchillo.anim"));
        lista.Add(new InteractableObj("Cajas quir�rgicas de implantes", "Esta caja contiene todo lo necesario para la colocaci�n de un implante Galimplant y su pr�tesis.", "Assets\\Resources\\Animations\\Cuchillo.anim"));
        lista.Add(new InteractableObj("Cubeta", "�Qu� es una cubeta en odontolog�a?Las cubetas de impresi�n dental son dispositivos que puedes utilizar para controlar y confinar el material requerido durante el proceso de toma de impresiones dentales.", "Assets\\Resources\\Animations\\Cuchillo.anim"));
        lista.Add(new InteractableObj("Escayola", "s un yeso para dados de trabajo sobre los que se har�n trabajos de rehabilitaci�n dental como coronas, implantes, postes o pr�tesis.", "Assets\\Resources\\Animations\\Cuchillo.anim"));
        lista.Add(new InteractableObj("Alginato", "pasta muy usada en los tratamientos dentales que permite obtener una copia fidedigna de la dentadura, ofrece un molde de los dientes. E", "Assets\\Resources\\Animations\\Cuchillo.anim"));
        lista.Add(new InteractableObj("Agua", "El agua m�s indicada para el uso en odontolog�a debe ser esterilizada o destilada", "Assets\\Resources\\Animations\\Cuchillo.anim"));
        lista.Add(new InteractableObj("Dosificadores de polvo y agua", "Un dosificador o m�quina dosificadora es una herramienta �til de trabajo, la cual nos permite agregar un l�quido o solido en cantidades exactas en cada una de sus descargas", "Assets\\Resources\\Animations\\Cuchillo.anim"));
        lista.Add(new InteractableObj("Taza de goma espec�fica", "Taza de goma c�moda y flexible dise�ada para preparar y contener mezclas como alginato o yesos dentales sin que se adhieran a la superficie.", "Assets\\Resources\\Animations\\Cuchillo.anim"));
        lista.Add(new InteractableObj("Elast�meros", "material que se utiliza en el proceso de la impresi�n dental para reproducir los tejidos de la cavidad oral, en negativo", "Assets\\Resources\\Animations\\Cuchillo.anim"));
        lista.Add(new InteractableObj("Pistola para silicona fluida y punta de automezcla", "La Pistola Para Silicona Dental es utilizada para mezclar y distribuir el material Bisacryl o Silicona. ", "Assets\\Resources\\Animations\\Cuchillo.anim"));
        lista.Add(new InteractableObj("Papel de burbujas", "un tipo de bolsa de embalaje con burbujas de aire.", "Assets\\Resources\\Animations\\Cuchillo.anim"));
        lista.Add(new InteractableObj("Cucharas electr�nicas", "instrumental utilizado para excavar las superficies de los dientes y poder detectar y eliminar caries y c�lculos as� como para eliminar tejido desorganizado y extirpar la pulpa coronaria.", "Assets\\Resources\\Animations\\Cuchillo.anim"));
        lista.Add(new InteractableObj("Cuchillete de escayola", "Cuchillo para cortar escayola con abre mufla.", "Assets\\Resources\\Animations\\Cuchillo.anim"));
        lista.Add(new InteractableObj("Desinfectante", "Las soluciones desinfectantes son sustancias que act�an sobre los microorganismos inactiv�ndolos y ofreciendo la posibilidad de mejorar con m�s seguridad los equipos y materiales durante el lavado", "Assets\\Resources\\Animations\\Cuchillo.anim"));
        lista.Add(new InteractableObj("Brackets", "Los brackets son utilizados para corregir malos posicionamientos dentales y alinear correctamente los dientes.", "Assets\\Resources\\Animations\\Cuchillo.anim"));
        lista.Add(new InteractableObj("Arcos", "Son los encargados de unir, a trav�s de ligaduras met�licas o el�sticas, los brackets que van colocados en los dientes con el objetivo de ir aline�ndolos progresivamente.", "Assets\\Resources\\Animations\\Cuchillo.anim"));
        lista.Add(new InteractableObj("Bandas", "Se aplican al alambre para mejorar y mantener la alineaci�n de la mordida y los dientes durante todo el proceso de tratamiento.", "Assets\\Resources\\Animations\\Cuchillo.anim"));
        lista.Add(new InteractableObj("Tubos", "Los tubos son unos dispositivos met�licos que se adhieren de forma directa sobre el esmalte en las caras vestibulares de los molares (aunque tambi�n pueden soldarse en las bandas). Sirven para soportar alambres activos y pasivos.", "Assets\\Resources\\Animations\\Cuchillo.anim"));
        lista.Add(new InteractableObj("Ligaduras", "La ligadura en lazo no es m�s que un alambre com�nmente utilizado de un calibre de . 010 que se trenza alrededor de los brackets con cierta tensi�n con la finalidad de desplazar un diente en un sentido determinado", "Assets\\Resources\\Animations\\Cuchillo.anim"));
        lista.Add(new InteractableObj("Cadenetas", "La cadeneta de ortodoncia es un elemento auxiliar que se usa en el tratamiento con brackets y est� compuesto de un material el�stico. Su uso nos permitir� conseguir una amplia variedad de objetivos durante este tratamiento tan com�n y habitual.", "Assets\\Resources\\Animations\\Cuchillo.anim"));
        lista.Add(new InteractableObj("Muelle o resorte", "Los muelles de ortodoncia son tipos especiales de resortes met�licos que se utilizan para crear espacio o cerrar huecos entre dos dientes.", "Assets\\Resources\\Animations\\Cuchillo.anim"));
        lista.Add(new InteractableObj("Pinza para posicionar brackets", "Pinzas Falcon de acero para Posicionar Brackets", "Assets\\Resources\\Animations\\Cuchillo.anim"));
        lista.Add(new InteractableObj("Alicates", "Alicate para doblar alambres de ortodoncia.", "Assets\\Resources\\Animations\\Cuchillo.anim"));
        lista.Add(new InteractableObj("Cepillo", "Cepillo fino limpieza interproximal con eje de acero inoxidable. Para la limpieza y el pulido de todas las �reas de acceso dif�cil como los espacios interdentales..", "Assets\\Resources\\Animations\\Cuchillo.anim"));
        lista.Add(new InteractableObj("Jab�n", "Desinclor Clorhexidina Jabonosa 500 ml con bomba. Est� recomendado su uso para el lavado de manos del personal en todos los servicios de alto riesgo y, en general, en todos los campos que requieren una m�xima desinfecci�n.", "Assets\\Resources\\Animations\\Cuchillo.anim"));
        lista.Add(new InteractableObj("Cuba de ultrasonidos", "se encargan de limpiar los instrumentos dentales eliminando los restos de sangre y peque�as part�culas que quedan adheridos a los instrumentos antes de la desinfecci�n.", "Assets\\Resources\\Animations\\Cuchillo.anim"));
        lista.Add(new InteractableObj("Glutaraldeh�do al 2%", "Desinfectante de Alto Nivel, Indicado para la desinfecci�n de alto nivel de instrumental, elementos de reprocesamiento y equipos termosensibles por inmersi�n que requieren desinfecci�n de alto nivel.", "Assets\\Resources\\Animations\\Cuchillo.anim"));
        lista.Add(new InteractableObj("Pa�o suave o celulosa", "absorbente elaborado de celulosa. Absorbe hasta 12 veces su peso. Especial para eliminar la suciedad y enjuagar, sin dejar rayas ni pelusas.", "Assets\\Resources\\Animations\\Cuchillo.anim"));
        lista.Add(new InteractableObj("Bolsas para esterilizar", "Las bolsas de esterilizaci�n son un dispositivo m�dico cuya funci�n es ser una barrera, manteniendo libre de virus y bacterias los productos en su interior.", "Assets\\Resources\\Animations\\Cuchillo.anim"));
        lista.Add(new InteractableObj("Autoclave", "Un autoclave es un aparato de uso m�dico que permite esterilizar mediante vapor de agua a alta presi�n y temperatura.", "Assets\\Resources\\Animations\\Cuchillo.anim"));
        lista.Add(new InteractableObj("Retractores de Labios y mejillas", "Los retractores de mejillas son instrumentos dentales dise�ados para mantener las mejillas y los labios alejados de los dientes, proporcionando un f�cil acceso para fotografiar los dientes afectados.", "Assets\\Resources\\Animations\\Cuchillo.anim"));
        lista.Add(new InteractableObj("Hilo de retracci�n", "Consiste en un hilo met�lico que va cementado de forma fija en la parte interior de canino a canino, tanto superior como inferior.", "Assets\\Resources\\Animations\\Cuchillo.anim"));
        lista.Add(new InteractableObj("Dique de goma", "son hojas cuadradas que se usan para aislar el lugar de la operaci�n del resto de la boca. Tienen un hoyo en el centro que le permite a su dentista aislar el �rea del tratamiento con una grapa dental alrededor del diente.", "Assets\\Resources\\Animations\\Cuchillo.anim"));
        lista.Add(new InteractableObj("Clamp", "Los clamps son abrazaderas met�licas que constringen la zona cervical de la corona dental para sujetar el dique de goma.", "Assets\\Resources\\Animations\\Cuchillo.anim"));
        lista.Add(new InteractableObj("Perforador de diques", "El perforador de diques es un instrumento que se emplea para producir un corte circular en la hoja del dique de hule que corresponda con el tama�o del diente que se necesita aislar, as� al colocar un clamps se consigue un aislamiento absoluto.", "Assets\\Resources\\Animations\\Cuchillo.anim"));
        lista.Add(new InteractableObj("Portaclamps", "Los Porta clamps dentales son pinzas acodadas, con un �ngulo en su parte activa que tiene la funci�n de llevar o retirar el clamp.", "Assets\\Resources\\Animations\\Cuchillo.anim"));
        lista.Add(new InteractableObj("Arco de young", "El arco esta dise�ado para su uso en aislamiento dental completo junto al dique de goma que actua como barrera y los clamps posicionados sobre el diente.", "Assets\\Resources\\Animations\\Cuchillo.anim"));


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
