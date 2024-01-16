using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InicarAplicacion : MonoBehaviour
{
    public GameObject inicio, menu;
    public Sprite imagen1, imagen2;
    [SerializeField] private Image image;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(IntercalarImagenes());
    }

    //corrutina para intercalar imagenes
    IEnumerator IntercalarImagenes()
    {
        image.sprite = imagen1;
        yield return new WaitForSeconds(5);

        image.sprite = imagen2;
        yield return new WaitForSeconds(5);
        inicio.SetActive(false);
        menu.SetActive(true);

    }
}
