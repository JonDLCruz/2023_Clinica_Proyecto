using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SliderValue : MonoBehaviour
{
    private Slider slider;
    private TextMeshProUGUI numText;

    private void Awake()
    {
        slider = GetComponentInParent<Slider>();
        numText = GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        UpdateText(slider.value);
        slider.onValueChanged.AddListener(UpdateText);
    }

    void UpdateText(float val)
    {
        numText.text = slider.value.ToString();
    }
}
