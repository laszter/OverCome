using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class VirtualButton : MonoBehaviour , IPointerDownHandler , IPointerUpHandler , IDragHandler
{
    private bool isHold, isPointerUp, isPointerDown;
    private int tabCount;
    private float delayTime = 0.1f;
    private float doubleTabDelay;
    private float pointDownDelay;
    private float pointUpDelay;
    private Image img;

    private void Start()
    {
        img = GetComponent<Image>();
        tabCount = 0;
        doubleTabDelay = 0;
    }

    private void Update()
    {
        DelayPointer();
    }

    public virtual void OnDrag(PointerEventData pointerEventData)
    {
        isHold = true;
    }

    public virtual void OnPointerDown(PointerEventData pointerEventData)
    {
        isPointerUp = false;
        isPointerDown = true;
        OnDrag(pointerEventData);
        img.color = SetRGBColorAlpha(img.color, 0.8f);
        tabCount += 1;
        pointDownDelay = delayTime;
        if (doubleTabDelay <= 0) doubleTabDelay = delayTime;
    }

    public virtual void OnPointerUp(PointerEventData pointerEventData)
    {
        isHold = false;
        isPointerUp = true;
        isPointerDown = false;
        pointUpDelay = delayTime;
        img.color = SetRGBColorAlpha(img.color, 0.5f);
    }

    public bool IsPointerHold()
    {
        return isHold;
    }

    public int GetTabCount()
    {
        return tabCount;
    }

    private void DelayPointer()
    {
        if (doubleTabDelay > 0) doubleTabDelay -= 1f * Time.deltaTime;
        else if (tabCount > 0) ResetTabCount();

        if (pointDownDelay > 0) pointDownDelay -= Time.deltaTime;
        else
        {
            pointDownDelay = 0;
            isPointerDown = false;
        }

        if (pointUpDelay > 0) pointUpDelay -= Time.deltaTime;
        else
        {
            pointUpDelay = 0;
            isPointerUp = false;
        }
    }

    public void ResetTabCount()
    {
        tabCount = 0;
    }

    public bool IsPointerUp()
    {
        return isPointerUp;
    }

    public bool IsPointerDown()
    {
        return isPointerDown;
    }

    private Color SetRGBColorAlpha(Color color,float a)
    {
        if (a > 1f) a = 1f;
        else if (a < 0) a = 0;
        return new Color(color.r, color.g, color.b, a);
    }
}
