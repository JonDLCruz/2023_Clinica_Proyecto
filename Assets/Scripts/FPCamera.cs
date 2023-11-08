using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPCamera : MonoBehaviour
{
    //Aqui tienes todas las variables que vamos a utilizar
    public Vector2 sensibility;
    private new Transform camera;
    private new Rigidbody rigidbody;
    public float movmentSpeed;
    public float rayDistance;

    public Texture2D puntero;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();

        camera = transform.Find("Camera");
        Cursor.lockState = CursorLockMode.Locked;

        
    }

    // Update is called once per frame
    void Update()
    {
        CameraControl();
        Movmetcharacter();
        Rayocamera();
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
            //Aqui realizamos que la camara con el ege vertical se establezca unas limitaciones
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
    public void Rayocamera()
    {
        Debug.DrawRay(camera.position, camera.forward * rayDistance, Color.red);
    }


    private void OnGUI()
    {
        //Para que le podamos poner puntero
        Rect rect = new Rect(Screen.width / 2, Screen.height / 2, puntero.width, puntero.height);
    }
}
