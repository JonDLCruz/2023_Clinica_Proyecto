using System.Collections;
using UnityEngine;
using TMPro;

public class Dialogo : MonoBehaviour
{
    [SerializeField] private GameObject dialogoPanel;
    [SerializeField] private GameObject m_introUI;
    [SerializeField] private TMP_Text dialogo;
    [SerializeField, TextArea(3,3)] private string[] dialogueLines;

    private bool didDialogueStart;
    private int lineIndex;
    private float tipingTime = 0.05f;


    // Update is called once per frame
    void Update()
    {
        if (m_introUI.activeSelf && !didDialogueStart)
        {
            StartDialogue();
        }
        else if (dialogo.text == dialogueLines[lineIndex])
        {
            NextDialogueLine();
        }
        else
        {
            StopAllCoroutines();
            dialogo.text = dialogueLines[lineIndex];
        }

    }

    private void StartDialogue()
    {
        didDialogueStart = true;
        //aqui iria activar panel y desactivar marcas si las hubiere. 
        lineIndex = 0;
        StartCoroutine(Showline());
    }

    private void NextDialogueLine()
    {
        lineIndex++;
        if (lineIndex >= dialogueLines.Length)
        {
            StartCoroutine(Showline());
        }
        else
        {
            didDialogueStart = false;
            //aqui iria desactivar panel y activar marcas si las hubiere.
        } 
       
    }

    private IEnumerator Showline()
    {
        dialogo.text = string.Empty;

        foreach (char ch in dialogueLines[lineIndex])
        {
            dialogo.text += ch;
            yield return new WaitForSeconds(tipingTime);
                }

    }


}
