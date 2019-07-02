using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowScore()
    {
        GameController.Instance.EndGame();
    }

    public void SetPos()
    {
        GameController.Instance.SetEndPos();
    }
}
