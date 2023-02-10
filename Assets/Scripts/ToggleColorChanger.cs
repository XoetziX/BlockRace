using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleColorChanger : MonoBehaviour
{
    public Color toggleOnColor;
    public Color toggleOffColor;

    private Toggle toggle;
    private Image toggleBackground;

    private void Start()
    {
        toggle = GetComponent<Toggle>();
        toggle.onValueChanged.AddListener(UpdateToggleColor);
        toggleBackground = toggle.transform.Find("Background").GetComponent<Image>();
    }

    private void UpdateToggleColor(bool isOn)
    {
        toggleBackground.color = isOn ? toggleOnColor : toggleOffColor;
    }


}
