using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class DialogoManager : MonoBehaviour
{
    [SerializeField] private GameObject dialogoPanel;
    [SerializeField] private GameObject m_introUI;
    [SerializeField] private GameObject m_loginUI;
    [SerializeField] private TMP_Text dialogo;
    //[SerializeField, TextArea(3,3)] private string[] dialogueLines;

    [SerializeField] private bool didDialogueStart = false;
    private int lineIndex = 0;
    [SerializeField] private Introduccion dialogoIntro;
    [SerializeField] private Dialogo dialogoOak;
    /////////////////////////////////////////////////////////////
   

   

    public void StarDialog(Dialogo dialogo)
    {
        dialogoIntro = dialogo.intro;
        lineIndex = 0;
        didDialogueStart = true;
        dialogoPanel.SetActive(true);
        ShowDialog();
        m_loginUI.SetActive(false);

    }

    public void ShowDialog()
    {
        if (lineIndex < dialogoIntro.lines.Length)
        {
            dialogo.text = dialogoIntro.lines[lineIndex];
            lineIndex++;
        }

    }

    public void EndDialg()
    {
        didDialogueStart = false;
        dialogoPanel.SetActive(false);
    }

    private void Start()
    {
      
    }
    // Update is called once per frame
    void Update()
    {
        if (!didDialogueStart)
        {
            Debug.Log("deberia comenzar a hablar Oak");
            StarDialog(dialogoOak);
        }
        if (didDialogueStart && Input.GetKeyDown(KeyCode.Space))
        {
            ShowDialog();

            if (lineIndex == dialogoIntro.lines.Length)
            {
                EndDialg();
                SceneManager.LoadScene(1);
            }
        }
    }

   


}
