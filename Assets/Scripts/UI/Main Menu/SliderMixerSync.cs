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
    private const float maxVol = 0;
    private const float minVol = -80.0f;
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
        float t = Mathf.Clamp01(Mathf.InverseLerp(minVol, maxVol, mixerParamVal));
        slider.value = t;
    }


    private void OnSliderChanged(float val)
    {
        float t = slider.value;
        float mixerParamVal = Mathf.Lerp(minVol, maxVol, t);
        Assert.IsTrue(mixer.SetFloat(mixerParam, mixerParamVal));
    }
}
