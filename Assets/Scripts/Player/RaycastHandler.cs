using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Video;

public class RaycastHandler : MonoBehaviour, RayCastInterface
{
    GameManager _GM;
    public float rayDistance;
    private GameObject objInteract;
    private GameObject lastHit;
    private bool rayCastObject = false;
    private bool shaderSwitch = false;
    //Mano
    [SerializeField]
    private GameObject _mano;
    public bool isGrabing;
    //Menus Objetos
    public GameObject MenuObjeto;
    public TextMeshProUGUI _tituloObj, _descObj;
    public VideoPlayer _vid;
    //DBObject
    string nameDB = "", descr = "", path = "";
    private void Start()
    {
        //Set GM
        _GM = GameObject.Find("GameManager").GetComponent<GameManager>();
        _GM.CrearListadeObjetos();

        //Set Mano
        _mano = GameObject.Find("Mano");
        MenuObjeto.SetActive(false);
        isGrabing = false;
    }
    void actualizarInfoObject()
    {
        //Actualizamos la info del objeto cuando lo tengamos aqui,
        _tituloObj.text = nameDB;
        _descObj.text = descr;
        //para el video es diferente tenemos que cambiar la textura lo dejaremos para cuando tengamos los videos
        //Vid.path = path; Solo es una referencia no es como se programa.
    }
    public void RaycastObjectInteract()
    {
        Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * rayDistance, Color.blue);
        RaycastHit hit;

        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, rayDistance, LayerMask.GetMask("OBJ_Checker")))
        {
            rayCastObject = true;
            shaderSwitch = false;
            lastHit = hit.collider.gameObject;
            SwitchShader(lastHit);
            MenuObjeto.SetActive(true);
            actualizarInfoObject();
            if (hit.collider.tag == "Interactable_Obj" && Input.GetMouseButton(0))
            {
                objInteract = hit.collider.gameObject;
                MenuObjeto.SetActive(false);
                Debug.Log("Detected");
                isGrabing = true;
                if (isGrabing)
                {
                    objInteract.transform.SetParent(_mano.transform, false); //0,0,0
                    objInteract.GetComponent<Rigidbody>().useGravity = false;
                    objInteract.GetComponent<Rigidbody>().isKinematic = true;
                    print("Hijo Cambiado" + hit.transform.position);
                    objInteract.transform.position = _mano.transform.position;
                    print("Hijo Junto a padre " + hit.transform.position);
                    shaderSwitch = true;
                    SwitchShader(objInteract);
                    objInteract.GetComponent<OBJ>().CancelResetPosition();
                }
            }
        }
        else
        {
            print("No muestro objeto");
            //MenuObjeto.SetActive(false);
            if (rayCastObject)
            {
                shaderSwitch = true;
                SwitchShader(lastHit);
                rayCastObject = false;
            }
        }
    }


    private void SwitchShader(GameObject obj)
    {
        if (!shaderSwitch)
        {
            obj.GetComponent<Renderer>().material.color = Color.red;
        }
        else
        {
            obj.GetComponent<Renderer>().material.color = Color.white;
        }
    }

    public void StopGrabbing()
    {
        if (objInteract != null)
        {

            objInteract.gameObject.GetComponent<Collider>().enabled = false;
            Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * rayDistance, Color.green);
            //Creamos el hit donde sacaremos toda la insformación
            RaycastHit hit;

            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, rayDistance))//casteamos el rayo desde camara y comprobamos los objetos en la mascara NPC_Checker
            {

                if (hit.collider.tag != "Interactable_Obj" && hit.collider.tag != "NPC" && Input.GetMouseButton(1))//Condicion para activar el npc
                {
                    isGrabing = false;
                    gameObject.GetComponent<Collider>().enabled = false;
                    objInteract.transform.SetParent(null);
                    objInteract.transform.position = hit.point + new Vector3(0, 0.5f, 0);
                    objInteract.GetComponent<Rigidbody>().useGravity = true;
                    objInteract.GetComponent<Rigidbody>().isKinematic = false;
                    objInteract.gameObject.GetComponent<Collider>().enabled = true;
                    gameObject.GetComponent<Collider>().enabled = true;
                    objInteract.GetComponent<OBJ>().ResetPosition();
                    MenuObjeto.SetActive(false);
                }
            }
        }
    }
    public void RaycastNPC()
    {
        Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * rayDistance, Color.red);
        RaycastHit hit;

        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, rayDistance, LayerMask.GetMask("NPC_Checker")))
        {
            //panelInfo.SetActive(true);
            //_interactInfo.text = "Pulsa E para Hablar";
            if (hit.collider.tag == "NPC" && Input.GetKeyDown(KeyCode.E))
            {
                print("Detecatado");
                //panelInfo.SetActive(false);
                //StartDialog(hit.collider.gameObject.GetComponent<NPCText>());
                //Le pasamos a la funcion el NPCText del NPC por ahora ser un dialogo
            }
        }
        else
        {
            //panelInfo.SetActive(false);
        }
    }
    void ObjetosInteractuar(string _nombreObjeto)//Hit.Collider.gameObject.name
    {
        (nameDB, descr, path) = _GM.AccederObjetoLista(_nombreObjeto);//extraemos el nombre descr y Path de animacion
        print(nameDB);
        print(descr);
        print(path);
    }
}
