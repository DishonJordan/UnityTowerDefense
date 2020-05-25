using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
public class AutoAspect : MonoBehaviour
{
    private Image image;
    private AspectRatioFitter fitter;

    void Update()
    {
        if(!image) image = GetComponent<Image>();
        if(!fitter) fitter = GetComponent<AspectRatioFitter>();
        if (!image || !fitter) return;
        float estimatedAspect = image.sprite.rect.width / image.sprite.rect.height;
        fitter.aspectRatio = estimatedAspect;
    }
}
