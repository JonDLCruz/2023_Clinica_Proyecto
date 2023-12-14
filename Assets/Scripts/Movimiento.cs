using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using Unity.VisualScripting;
using UnityEngine;

public class Movimiento : MonoBehaviour
{
    private float velocidad = 5.0f;
    public float rayDistance = 5.0f;
    public Vector2 sensibilidad;
    private new Transform camera;
    private new Rigidbody rigidbody;
    //puntero
    public Texture2D puntero;
    private InventarioUI ui;

    // Start is called before the first frame update
    void Start()
    {
        
        rigidbody = GetComponent<Rigidbody>();
        camera = transform.Find("Main Camera");
        Cursor.lockState = CursorLockMode.Locked;
        ui = GameObject.Find("GameManager").GetComponent<InventarioUI>();


    }


    // Update is called once per frame
    void Update()
    {
        CamaraControl();
        MovimientoPersonaje();
        rayController();

    }
    public void CamaraControl()
    {
        float hor = Input.GetAxis("Mouse X");
        float ver = Input.GetAxis("Mouse Y");

        if (hor != 0)
        {
            transform.Rotate(Vector3.up * hor * sensibilidad.x);
        }
        if (ver != 0)
        {
            float angle = (camera.localEulerAngles.x - ver * sensibilidad.y + 360) % 360;
            if (angle > 180)
            {
                angle -= 360;
            }
            angle = Mathf.Clamp(angle, -80, 80);
            camera.localEulerAngles = Vector3.right * angle;
        }
    }

    public void MovimientoPersonaje()
    {
        float hor = Input.GetAxis("Horizontal");
        float ver = Input.GetAxis("Vertical");
        Vector3 velocity = Vector3.zero;

        if (hor != 0 || ver != 0)
        {
            Vector3 direction = (transform.forward * ver + transform.right * hor).normalized;
            velocity = direction * velocidad;

        }

        velocity.y = rigidbody.velocity.y;
        rigidbody.velocity = velocity;
    }

    private void OnGUI()
    {
        Rect rect = new Rect(Screen.width / 2, Screen.height / 2, puntero.width, puntero.height);
        GUI.DrawTexture(rect, puntero);

    }

   public void rayController()
    {
       
        Vector3 origenRayo = camera.transform.position;
        Vector3 direccionRayo = camera.transform.forward;
        Debug.DrawRay(camera.position, camera.forward * rayDistance, Color.red);
        RaycastHit hit;
        if (Physics.Raycast(origenRayo,direccionRayo, out hit, rayDistance, LayerMask.GetMask("ItemUI")))
        {
            if (hit.collider.tag == "ItemUI")
            {
                ui.cogerE.SetActive(true);
                hit.collider.gameObject.GetComponent<BaseItem>().CogerObjeto();
            }
        } else
        {
            ui.cogerE.SetActive(false);
        }
        

    }
        
}
