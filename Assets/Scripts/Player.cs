using UnityEngine;

public class Player : MonoBehaviour
{
    public Transform itemSpawn;
    Inventario inventario;
   

    // Start is called before the first frame update
    void Start()
    {
        inventario = Inventario.Instance;
        
    }

    // Update is called once per frame
    void Update()
    {
      //aqui podemos ponerle si damos a i que habra inventario
      /*
      if (Input.GetKeyDown(KeyCode.I)) 
        {
            inventario.ToggleInventary();
        } */
    }
}
