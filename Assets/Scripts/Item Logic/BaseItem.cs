using System.ComponentModel;
using UnityEngine;

[System.Serializable]
public abstract class BaseItem : MonoBehaviour
{
    public int id;
    public int quantity = 1;

    public Database.InventoryItem itemData;
    private InventarioUI ui;

    // Start is called before the first frame update
    void Start()
    {
        SetDataById(id, quantity);
        ui= GameObject.Find("GameManager").GetComponent<InventarioUI>();
    }

    public void SetDataById(int id, int quantity = 1)
    {
        itemData.ID = id;
        itemData.nombre = Inventario.Instance.db.dataBase[id].nombre;
        itemData.acumulable = Inventario.Instance.db.dataBase[id].acumulable;
        itemData.tipo = Inventario.Instance.db.dataBase[id].tipo;
        itemData.icono = Inventario.Instance.db.dataBase[id].icono;
        itemData.descripcion = Inventario.Instance.db.dataBase[id].descripcion;

        this.quantity = quantity;
    }

    public abstract void Use();

    public void CogerObjeto()
    {
       

        if (Input.GetKeyDown(KeyCode.E))
        {
            ui.cogerE.SetActive(false);
            Inventario.Instance.AddItem(id, quantity);
            Destroy(this.gameObject);

        }
    }





}
