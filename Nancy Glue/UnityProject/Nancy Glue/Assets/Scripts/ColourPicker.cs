using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ColourPicker : MonoBehaviour
{
    public UnityEvent<Color> ColourPickerEvent;

    [SerializeField] Texture2D colourChart;
    [SerializeField] GameObject chart;

    [SerializeField] RectTransform Cursor;
    [SerializeField] Image Button;
    [SerializeField] Image cursorColor;

    public void PickColour(BaseEventData data)
    {
        PointerEventData pointer = data as PointerEventData;

        Cursor.position = pointer.position;

        Color pickedColor = colourChart.GetPixel((int)(Cursor.localPosition.x * (colourChart.width / transform.GetChild(0).GetComponent<RectTransform>().rect.width)), (int)(Cursor.localPosition.y * (colourChart.height / transform.GetChild(0).GetComponent<RectTransform>().rect.height)));
        Debug.Log(pickedColor);
        Button.color = pickedColor;
        cursorColor.color = pickedColor;
        ColourPickerEvent.Invoke(pickedColor);
    }
}
