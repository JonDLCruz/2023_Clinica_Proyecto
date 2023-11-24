using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DeletionPrompt : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private Button selectAllBtn;
    [SerializeField] private TextMeshProUGUI amountText;

    ItemUI _item;

    // Start is called before the first frame update
    void Start()
    {
        slider.minValue = 1;
            slider.wholeNumbers = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (this.gameObject.activeInHierarchy)
        {
            amountText.text = slider.value.ToString();
        }
    }

    public void SetSliderData(ItemUI item)
    {
        _item = item;
        slider.value = 1;
        slider.maxValue = item.quantity;
        selectAllBtn.onClick.AddListener(() => slider.value = slider.maxValue);
    }

    public void Aceptar()
    {
        Inventario.Instance.DeleteItem(_item, (int)slider.value, false);
        this.gameObject.SetActive(false);
    }

    public void Cancelar()
    {
        this.gameObject.SetActive(false);
    }




}
