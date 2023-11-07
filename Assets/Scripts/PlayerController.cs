using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    public Vector2 sensibility;

    public new Transform camera;
    public GameObject _actividades, _panelSubtitles, _panelActividades;
    public TextMeshProUGUI _subtitles;
    private new Rigidbody rigidbody;
    public float movmentSpeed;
    public float rayDistance;
    public bool canMove, canMoveCamera;
    private bool isTalking = false;
    public string[] dialogueText;
    private int currentIndex = 0;
    private float dialogueTimer = 0f;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        _actividades.SetActive(false);
        _panelSubtitles.SetActive(false);
        _panelActividades.SetActive(false);
        canMove = true;
        canMoveCamera = true;
    }

    // Update is called once per frame
    void Update()
    {
        RaycastNPC();
        Movmetcharacter();
        CameraControl();
        if(isTalking)
        {
            HandleDialog();
        }


    }

    public void CameraControl()
    {
        float hor = Input.GetAxis("Mouse X");
        float ver = Input.GetAxis("Mouse Y");
        if (hor != 0)
        {
            transform.Rotate(Vector3.up * hor * sensibility.x);
        }

        if (ver != 0)
        {
            //camera.Rotate(Vector3.left * ver * sensibility.y);
            float angle = (camera.localEulerAngles.x - ver * sensibility.y + 360) % 360;
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

    public void RaycastNPC()
    {
        Debug.DrawRay(camera.position, camera.forward * rayDistance, Color.red);

        RaycastHit hit;

        if (Physics.Raycast(camera.position, camera.forward, out hit, rayDistance, LayerMask.GetMask("NPC_Checker")))
        {

            if (hit.collider.tag == "NPC" && Input.GetMouseButton(0) && !isTalking)
            {
                print("Detecatado");
                    StartDialog(hit.collider.gameObject.GetComponent<NPCText>());
            }
        }
        ;
    }

    void StartDialog(NPCText _npc)
    {
        print("Entro");
        isTalking = true;
        canMove = false;
        canMoveCamera = false;
        UnityEngine.Cursor.lockState = CursorLockMode.None;
        dialogueText = _npc.arrayText;
        currentIndex = 0;
        _actividades.SetActive(true);
        _panelSubtitles.SetActive(true);
        _subtitles.text = dialogueText[currentIndex];
        print("Termino");
    }
  
    void HandleDialog()
    {
        dialogueTimer += Time.deltaTime;
        if (dialogueTimer >= 3) {

            dialogueTimer = 0f;
            currentIndex++;
            if (currentIndex <= dialogueText.Length)
            {
                _subtitles.text = dialogueText[currentIndex];
            }
            else
            {
                EndDialogue();
            }
        }
    }
    void EndDialogue()
    {
        isTalking = false;
        canMove = true;
        canMoveCamera = true;
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        _panelSubtitles.SetActive(false);
        _actividades.SetActive(false);
        dialogueText = null;
    }
}
