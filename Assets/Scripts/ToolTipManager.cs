using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class ToolTipManager : MonoBehaviour
{
    public TextMeshProUGUI tipText;
    public RectTransform tipWin;

    public static Action<string, Vector2, int> OnMouseHover;
    public static Action OnMouseLeave;

    private void OnEnable()
    {
        OnMouseHover += ShowToolTip;
        OnMouseLeave += HideToolTip;
    }

    private void OnDisable()
    {
        OnMouseHover -= ShowToolTip;
        OnMouseLeave -= HideToolTip;
    }

    private void Start()
    {
        HideToolTip();
    }

    public void ShowToolTip(string tip, Vector2 pos, int MAX_WIDTH)
    {
        tipText.text = tip;
        tipWin.sizeDelta = new Vector2(MAX_WIDTH, tipText.preferredHeight);
        tipWin.transform.position = new Vector2(pos.x + 50, pos.y);
        tipWin.gameObject.SetActive(true);
    }
    public void HideToolTip()
    {
        tipText.text = default;
        tipWin.gameObject.SetActive(false);
    }

}
