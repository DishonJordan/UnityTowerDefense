using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class SliderSetText : MonoBehaviour
{
    private Slider slider;
    public bool round = true;
    public float multiplier = 100f;
    public TextMeshProUGUI text;

    private void Awake()
    {
        slider = GetComponent<Slider>();
        slider.onValueChanged.AddListener(UpdateSliderValueText);
    }

    private void OnEnable()
    {
        UpdateSliderValueText(slider.value);
    }

    void UpdateSliderValueText(float val)
    {
        val *= multiplier;
        text.text = round ? ((int) val).ToString() : val.ToString();
    }
}
