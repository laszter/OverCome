using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using UnityEngine.XR;
using UnityEngine.UI;
using System.Collections.Generic;

public class VirtualStick : MonoBehaviour , IPointerDownHandler , IPointerUpHandler , IDragHandler
{
    private static VirtualStick instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public static VirtualStick Instance
    {
        get
        {
            return instance;
        }
    }

    private Vector2 pointDown;
    private Vector3 directionDown, directionDrag;
    private bool isDraging, isPointerUp, isPointerDown;
    private int tabCount;
    private float distanceFloat;
    private float doubleTabDelay;
    [Header("Scripts")]
    public VirtualButton action;
    public VirtualButton dash;
    public GameObject pauseBtn;

    [Header("Image")]
    public Image stickCircle;
    public Image stickPoint;
    private Vector2 inputVector;

    public Vector2 stickDefaultPos;

    private void Start()
    {
        tabCount = 0;
        doubleTabDelay = 0;
        stickDefaultPos = stickCircle.transform.position;
    }

    private void Update()
    {
        DoubleTabTimeOut();
    }

    public virtual void OnDrag(PointerEventData pointerEventData)
    {
        Vector2 distance = pointerEventData.position - pointDown;
        distanceFloat = Vector2.Distance(pointerEventData.position, pointDown);
        if (distanceFloat > 1f)
        {
            isDraging = true;
            directionDown = new Vector3(pointDown.x, 0, pointDown.y);
        }
        else
        {
            isDraging = false;
        }
        directionDrag = new Vector3(pointerEventData.position.x, 0, pointerEventData.position.y);

        //MoveStickImage
        Vector2 pos;
        if(RectTransformUtility.ScreenPointToLocalPointInRectangle(stickCircle.rectTransform,pointerEventData.position,pointerEventData.pressEventCamera,out pos))
        {
            pos.x = (pos.x / stickCircle.rectTransform.sizeDelta.x);
            pos.y = (pos.y / stickCircle.rectTransform.sizeDelta.y);

            inputVector = new Vector2(pos.x * 2, pos.y * 2);
            inputVector = (inputVector.magnitude > 1.0f) ? inputVector.normalized : inputVector;

            stickPoint.rectTransform.anchoredPosition = new Vector3(inputVector.x * (stickCircle.rectTransform.sizeDelta.x / 3), inputVector.y * (stickCircle.rectTransform.sizeDelta.y / 3));
        }
    }

    public virtual void OnPointerDown(PointerEventData pointerEventData)
    {
        isPointerUp = false;
        isPointerDown = true;
        if (pointerEventData.position.x < Screen.width / 2)
        {
            pointDown = pointerEventData.position;
            directionDown = new Vector3(pointDown.x, 0, pointDown.y);
            stickCircle.transform.position = pointDown;
            OnDrag(pointerEventData);
            tabCount += 1;
            if (doubleTabDelay <= 0) doubleTabDelay = 0.3f;
        }
    }

    public virtual void OnPointerUp(PointerEventData pointerEventData)
    {
        pointDown = Vector2.zero;
        isDraging = false;
        isPointerUp = true;
        isPointerDown = false;
        stickCircle.transform.position = stickDefaultPos;
        inputVector = Vector3.zero;
        stickPoint.rectTransform.anchoredPosition = Vector2.zero;
    }

    public float Horizontal()
    {
        if (inputVector.x != 0)
            return inputVector.x;
        else
            return 0;
    }

    public float Vertical()
    {
        if (inputVector.y != 0)
            return inputVector.y;
        else
            return 0;
    }

    public bool IsDraging()
    {
        return isDraging;
    }

    public int GetTabCount()
    {
        return tabCount;
    }

    private void DoubleTabTimeOut()
    {
        if (doubleTabDelay > 0) doubleTabDelay -= 1f * Time.deltaTime;
        else if (tabCount > 0) ResetTabCount();
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

    public float GetDistance()
    {
        if(distanceFloat / (Screen.width / 2) * 4 > 1)
        {
            return 1;
        }
        return distanceFloat / (Screen.width / 2) * 4;
    }

    public Vector3 GetDragDirection()
    {
        return directionDrag - directionDown;
    }
}
