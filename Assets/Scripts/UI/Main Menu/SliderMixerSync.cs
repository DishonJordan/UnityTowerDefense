using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Audio;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class SliderMixerSync : MonoBehaviour
{
    private Slider slider;
    public AudioMixer mixer;
    public string mixerParam;

    private void Awake()
    {
        slider = GetComponent<Slider>();
        slider.onValueChanged.AddListener(OnSliderChanged);
    }

    private void OnEnable()
    {
        float mixerParamVal = 0;
        Assert.IsTrue(mixer.GetFloat(mixerParam, out mixerParamVal));
        float t = Mathf.Pow(10, mixerParamVal / 20);
        slider.value = t;
    }


    private void OnSliderChanged(float val)
    {
        float t = slider.value;
        float mixerParamVal = Mathf.Log10(t) * 20;
        Assert.IsTrue(mixer.SetFloat(mixerParam, mixerParamVal));
    }
}
