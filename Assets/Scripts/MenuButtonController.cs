using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuButtonController : MonoBehaviour , IPointerEnterHandler , IPointerExitHandler , IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        GetComponent<Text>().color = Color.white;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        GetComponent<Text>().color = Color.yellow;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        GetComponent<Text>().color = Color.white;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
