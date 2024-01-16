using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class StartButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public TMP_Text textBotton;
    public Sprite imageOn;
    public Sprite imageOff;
    [SerializeField] private Image image;


    public void OnPointerEnter(PointerEventData eventData)
    {
        // Cambiar el color del texto al detectar el ratón encima
        textBotton.color = Color.black;
        image.sprite = imageOn;
        
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Restaurar el color original del texto cuando el ratón sale
        textBotton.color = Color.white;
        image.sprite = imageOff; 
    }
}