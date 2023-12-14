using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;
using System.ComponentModel;

public class Inventario : MonoBehaviour
{
    public GraphicRaycaster graphRay;
    public Database db;
    public int slotCount = 7;
    private bool isOpen;
    [SerializeField] private Player player;// referencia al jugador
    //[SerializeField] private GameObject inventoryToggle; referencia si se quiere poner boton de cerrado
    [SerializeField] private Transform slotPrebab;
    [SerializeField] private Transform itemPreb;

    public DeletionPrompt deletionPrompt;

    public DescriptionUI descriptionUI; //esto es para referenciar un pequeño cartel, no creo que lo usemos pero lo dejo aqui por si acaso

    [SerializeField] private List<ItemUI> items = new List<ItemUI>();
    bool itemsDeleteModeEnabled;

    [SerializeField] Transform slotContainer;

    private List<Transform> slots = new List<Transform>();

    public static Inventario Instance { get; private set; }
    public bool IsOpen { get => isOpen; set => isOpen = value; }
    public bool IsOpen1 { get => isOpen; set => isOpen = value; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

    }
    // Start is called before the first frame update

    private void Start()
    {
        for (int i = 0; i < slotCount; i++)
        {
            Transform newSlot = Instantiate(slotPrebab, slotContainer);
            slots.Add(newSlot);
        }

        IsOpen = true;

    }

    public void UpdateParent(ItemUI item, Transform newParent)
    {
        item.exParent = newParent;
        item.transform.SetParent(newParent);
        item.transform.parent.GetComponent<Image>().fillCenter = true;
        item.transform.localPosition = Vector3.zero;

    }

    public void AddItem(int id, int quantity)
    {
        ItemUI preexitentValidItem = items.Find(item => item.itemData.ID == id); //si queremos staquear añadir && item.itemData.maxStack >= item.quantity + quantity
        if (preexitentValidItem != null)
        {
            preexitentValidItem.quantity += quantity;
            return;
        }

        for (int i = 0; i <= slots.Count; i++)
        {
            ItemUI itemInSlot = slots[i].childCount == 0 ? null : slots[i].GetChild(0).GetComponent<ItemUI>();

            if (itemInSlot == null)
            {
                ItemUI itemCopy = Instantiate(itemPreb, transform).GetComponent<ItemUI>();

                itemCopy.InitializeItem(id, quantity);
                items.Add(itemCopy);

                UpdateParent(itemCopy, slots[i]);
                break;
            }
            else 
            {
                itemInSlot.quantity += quantity; //cambiar luego por objeto ya ocupado
                break;

            }
        }

    }
    // en principio esto es para borrar objetos, pero no se como lo vamos a usar, lo dejo por si acaso 
     
    public void DeleteItem(ItemUI item, int quantity, bool byUse)
    {
        ItemUI itemToDelete = items.Find(it => it == item);

        itemToDelete.quantity -= quantity;

        if (!byUse)
        {
            BaseItem spawnedItem = Instantiate(item.itemData.item);
            //spawnedItem.transform.position = Player.itemSpawn.position;
            spawnedItem.SetDataById(item.id, quantity);
        }

        if (itemToDelete.quantity <= 0)
        {
            itemToDelete.exParent.GetComponent<Image>().fillCenter = false;
            items.Remove(itemToDelete);
            Destroy(itemToDelete.gameObject);
        }
    }
    
    // esto habilita el boton de borrado, que tampoco tenemos
    public void ToggleDeleteMode()
    {
        itemsDeleteModeEnabled = !itemsDeleteModeEnabled;
        foreach (ItemUI item in items)
        {
            item.EnableDeletion(itemsDeleteModeEnabled);
        }
    }
    

    // para abrr y cerrar el inventario... creo
    public void ToggleInventary()
    {
        if (isOpen && itemsDeleteModeEnabled) ToggleDeleteMode();

        GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        //inventoryToggle.SetActive(!isOpen);
        isOpen = !isOpen;
    }
    
    // hace un popout de un a descripcion
    public void ShowDescripction(ItemUI item)
    {
        descriptionUI.gameObject.SetActive(true);
        descriptionUI.Show(item);
    }
    //oculta la descriocion
    public void HideDescription()
    {
        descriptionUI.gameObject.SetActive(false);

    }
    //actica la seleccion para borrar en masa
    public void ShowDeletePropmt(ItemUI item)
    {
        deletionPrompt.gameObject.SetActive(true);
        deletionPrompt.SetSliderData(item);
    }
    //añade mas slots al inventario
    public void AddMoreSpace (int slotsToAdd)
    {
        for (int i = 0; i < slotsToAdd;i++)
        {
            Transform newSlot = Instantiate(slotPrebab, slotContainer);
            slots.Add(newSlot);
        }
    }
    
}
