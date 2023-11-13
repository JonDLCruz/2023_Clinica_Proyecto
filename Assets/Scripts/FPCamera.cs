using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.AffordanceSystem.Receiver.Primitives;

public class FPCamera : MonoBehaviour
{
    //Variables 
    public Vector2 sensibility;
    private new Transform camera;
    private new Rigidbody rigidbody;
    public float movmentSpeed;
    public float rayDistance;

    //Variables para GUI
    public Texture2D puntero;

    //Variables para la interaccion de obejetos
    GameObject objInteract;
    GameObject _mano;
    private bool isGrabing;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();

        camera = transform.Find("Camera");
        Cursor.lockState = CursorLockMode.Locked;

        _mano = GameObject.Find("Hand");

        isGrabing = false;
    }

    // Update is called once per frame
    void Update()
    {
        CameraControl();
        Movmetcharacter();

        if (!isGrabing)
        {
            RaycastObjectInteract();
        }
        else
        {
            StopGrabing(objInteract);
        }
    }

    //Control de la camara con el raton
    public void CameraControl()
    {
        //Ejes para la camara
        float hor = Input.GetAxis("Mouse X");
        float ver = Input.GetAxis("Mouse Y");
        if (hor != 0)
        {
            transform.Rotate(Vector3.up * hor * sensibility.x);
        }

        if (ver != 0)
        {
            float angle = (camera.localEulerAngles.x - ver * sensibility.y + 360) % 360;
            if (angle > 180)
            {
                angle -= 360;
            }
            angle = Mathf.Clamp(angle, -80, 80);
            camera.localEulerAngles = Vector3.right * angle;
        }
    }

    //Aqui tenemos para el movimiento del personage
    public void Movmetcharacter()
    {
        //Ejes del movimiento
        float hor = Input.GetAxisRaw("Horizontal");
        float ver = Input.GetAxisRaw("Vertical");

        Vector3 velocity = Vector3.zero;

        if (hor != 0 || ver != 0)
        {
            Vector3 direction = (transform.forward * ver + transform.right * hor).normalized;
            velocity = direction * movmentSpeed;

        }

        velocity.y = rigidbody.velocity.y;
        rigidbody.velocity = velocity;
    }

    //Funcionalidades del Rayo

    public void RaycastObjectInteract()
    {
        //Dibujamos el rayo para verlo en play
        Debug.DrawRay(camera.position, camera.forward * rayDistance, Color.blue);
        //Creamos el hit donde sacaremos toda la insformación
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
        //Creamos el hit donde sacaremos toda la insformación
        RaycastHit hit;

        if (Physics.Raycast(camera.position, camera.forward, out hit, rayDistance))//casteamos el rayo desde camara y comprobamos los objetos en la mascara NPC_Checker
        {

            if (hit.collider.tag != "Interactable_Obj" && hit.collider.tag != "NPC" && Input.GetMouseButton(1))//Condicion para activar el npc
            {
                
                gameObject.GetComponent<Collider>().enabled = false;
                _obj.transform.SetParent(null);
                _obj.transform.position = hit.point + new Vector3(0, 0.5f, 0);
                _obj.GetComponent<Rigidbody>().useGravity = true;
                objInteract.GetComponent<Rigidbody>().isKinematic = false;
                _obj.gameObject.GetComponent<Collider>().enabled = true;
                gameObject.GetComponent<Collider>().enabled = true;
                isGrabing = false;

                objInteract.GetComponent<ReturnInitial>().ResetPosition();
            }
        }
    }

    private void OnGUI()
    {
        //Para que le podamos poner puntero
        Rect rect = new Rect(Screen.width / 2, Screen.height / 2, puntero.width, puntero.height);
        GUI.DrawTexture(rect, puntero);
    }
}