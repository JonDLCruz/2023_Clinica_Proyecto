using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DescriptionUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI header;
    [SerializeField] TextMeshProUGUI content;
    public int characterWrapLimit;

    [SerializeField] LayoutElement layoutElement;
    RectTransform rect;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    public void Show(ItemUI item)
    {
        header.text = item.itemData.nombre;
        content.text = item.itemData.descripcion;

        int headerLength = header.text.Length;
        int contentLength = content.text.Length;

        layoutElement.enabled = (headerLength > characterWrapLimit || contentLength > characterWrapLimit);

    }
        
    // Update is called once per frame
    void Update()
    {
        Vector2 position = Input.mousePosition;

        float pivotX = position.x / Screen.width;
        float pivotY = position.y / Screen.height;

        float finalPivotX;
        float finalPivotY;
        
        //si el mouse esta en la parte izquierda de la pantalla mueve el cursor a la derecha y viceversa 
        if ( pivotX < 0.5) 
            finalPivotX = -0.1f; 
        else 
            finalPivotX = 1.01f;

        //si el mouse esta en la parte inferior de la pantalla mueve el cursor a la arriba y viceversa
        if (pivotY < 0.5)
            finalPivotY = 0;
        else
            finalPivotY = 1;

        rect.pivot = new Vector2(finalPivotX, finalPivotY);

        transform.position = position;

    }
}
