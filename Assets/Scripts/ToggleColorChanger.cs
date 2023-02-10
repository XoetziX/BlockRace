using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleColorChanger : MonoBehaviour
{
    //public Color toggleOnColor;
    //private Color toggleOnColor = new Color(207,175,245,255);
    private Color toggleOnColor = new Color(207f / 255f, 175f / 255f, 245f / 255f);
    private Color toggleOffColor = new Color(240f / 255f, 240f / 255f, 240f / 255f);

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
