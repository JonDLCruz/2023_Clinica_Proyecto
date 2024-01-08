using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ControladorPreguntas : MonoBehaviour
{
    [SerializeField] private GameObject buttonPref;
    [SerializeField] private TextMeshProUGUI pregText;
    [SerializeField] private Transform opcionesContainer;
    private List<Button> poolButtons = new List<Button>();

    // Start is called before the first frame update
    void Start()
    {

    }

    public void ActivarBotones(int cantidad, string title, Opciones[] opciones)
    {
        pregText.text = title;

        if (poolButtons.Count >= cantidad)
        {
            for (int i = 0; i < poolButtons.Count; i++)
            {
                if (i < cantidad)
                {
                    poolButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = opciones[i].opcion;
                    poolButtons[i].onClick.RemoveAllListeners();
                    Conversacion co = opciones[i].convResultante;
                    poolButtons[i].onClick.AddListener(() => DarFuncionABotones(co));
                    poolButtons[i].gameObject.SetActive(true);
                }
                else
                {
                    poolButtons[i].gameObject.SetActive(false);
                }
            }
        } 
        else
        {
            int cantidadRestante = (cantidad - poolButtons.Count); 
            for (int i = 0; i < cantidadRestante; i++)
            {
                var newButton = Instantiate(buttonPref, opcionesContainer).GetComponent<Button>();
               newButton.gameObject.SetActive(true);
                poolButtons.Add(newButton);
            }
            ActivarBotones(cantidad, title, opciones);
        }
    }

    public void DarFuncionABotones(Conversacion conv)
    {
        DialogueManager.instance.SetConversacion(conv, null);
    }





    // Update is called once per frame
    void Update()
    {

    }
}
