using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using Unity.VisualScripting;

public class ItemUI : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler, IEndDragHandler   
{
    [SerializeField] private Database db;
    [SerializeField] private GameObject deleteButton;



    public int id;
    public int quantity;

    [HideInInspector] 
    public Database.InventoryItem itemData;
    [HideInInspector] 
    public Transform exParent;

    Image iconoImagen;
    Vector3 dragOffset;

    void Awake()
    {
        iconoImagen = GetComponentInChildren<Image>();

        exParent = transform.parent;
        if (exParent.GetComponent<Image>()) exParent.GetComponent<Image>().fillCenter = true;

        InitializeItem(id, quantity);
         
    }

    public void InitializeItem(int id, int quantity)
    {
        itemData.ID = id;
        itemData.nombre = db.dataBase[id].nombre;
        itemData.acumulable = db.dataBase[id].acumulable;
        itemData.icono = db.dataBase[id].icono;
        itemData.tipo = db.dataBase[id].tipo;

        
        iconoImagen.sprite = itemData.icono;
        this.quantity = quantity;
    }

    public void EnableDeletion(bool enable)
    {
        deleteButton.SetActive(enable);
    }


    //borrar objetos por medio de un boton delete
    public void Delete()
    {
        Inventario.Instance.HideDescription();
        if (quantity > 1)
            Inventario.Instance.ShowDeletePropmt(this);
       else
            Inventario.Instance.DeleteItem(this, 1, false);
    }

    //si damos doble click para usar el objeto
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.clickCount == 2)
        {
            Inventario.Instance.HideDescription();
            itemData.item.Use();
            Inventario.Instance.DeleteItem(this, 1, true);
        }
    }

    //muestra la descripcion cuando el raton pasa por encima del objeto
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!eventData.dragging)
        {
            Inventario.Instance.ShowDescripction(this);
        }
    }
    //cuando sale del objeto
    public void OnPointerExit(PointerEventData eventData)
    {
        Inventario.Instance.HideDescription();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Inventario.Instance.HideDescription();
        exParent = transform.parent;
        exParent.GetComponent<Image>().fillCenter = false;
        transform.SetParent(Inventario.Instance.transform);
        dragOffset = transform.position - Input.mousePosition;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition + dragOffset;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        List<RaycastResult> raycastResults = new List<RaycastResult>();
        Transform slot = null;

        //casteo un rayo desde la posicion del mouse y guardo todo lo que toca en la variable
        Inventario.Instance.graphRay.Raycast(eventData, raycastResults);

        //integro todos los colliders tocados
        foreach (RaycastResult hit in raycastResults)
        {
            var hitObj = hit.gameObject;

            if (hitObj.CompareTag("ItemUI"))
            {
                //verifico que no tome el hit con el objeto mismo que estoy arrastrando
                if (hitObj != this.gameObject)
                {
                    ItemUI hitObjItemData = hitObj.GetComponent<ItemUI>();
                    if (hitObjItemData.itemData.ID != id) 
                    { 
                        slot = hitObjItemData.transform.parent;
                        Inventario.Instance.UpdateParent(hitObjItemData, exParent);
                        break;
                    }
                }
            }

        }
        Inventario.Instance.UpdateParent(this, slot != null ? slot : exParent);

    }
}
