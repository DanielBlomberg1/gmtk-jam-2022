using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ToolTipInterfaces : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private int MAX_WIDTH = 400;
    [SerializeField] public string TextToShow;
    private const float growthAmount = 1.1f;
    private const float timeToWait = 0.3f;
    private float x, y, z;

    private void Start()
    {
        z = gameObject.transform.localScale.z;
        x = gameObject.transform.localScale.x;
        y = gameObject.transform.localScale.y;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        StopAllCoroutines();
        StartCoroutine(Delay());
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        StopAllCoroutines();
        gameObject.transform.localScale = new Vector3(x, y, z);
        ToolTipManager.OnMouseLeave();
    }

    private void ShowMessage()
    {
        //gameObject.transform.localScale  = new Vector3( x * growthAmount, y * growthAmount, z * growthAmount);
        ToolTipManager.OnMouseHover(TextToShow, Input.mousePosition, MAX_WIDTH);
    }
    private IEnumerator Delay()
    {
        yield return new WaitForSeconds(timeToWait);

        ShowMessage();
    }
}
