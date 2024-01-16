using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class ChangeColorText : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public TMP_Text textBotton;
   
      

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Cambiar el color del texto al detectar el ratón encima
        textBotton.color = Color.black;
        Debug.Log("seleccionado");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Restaurar el color original del texto cuando el ratón sale
        textBotton.color = Color.white;
    }
}
