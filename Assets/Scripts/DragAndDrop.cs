using UnityEngine;
using UnityEngine.EventSystems;

public class DragAndDrop : MonoBehaviour, IBeginDragHandler, IDragHandler
{
    Vector3 dragOffset;

    public void OnBeginDrag( PointerEventData eventData)
    {
        dragOffset = transform.position - Input.mousePosition;
    }

    public void OnDrag( PointerEventData eventData )
    {
        transform.position = Input.mousePosition + dragOffset;
    }





}
