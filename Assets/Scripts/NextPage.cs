using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextPage : MonoBehaviour {

    public int page;
    private int currentPage;
    public GameObject[] pages;

	// Use this for initialization
	void Start () {
        if (pages.Length < 1) {
            GameController.Instance.StartGame();
            gameObject.SetActive(false);
            return;
        }

        currentPage = 0;
        page = currentPage;
        for(int i = 1; i < pages.Length; i++)
        {
            pages[i].SetActive(false);
        }
        pages[currentPage].SetActive(true);
	}
	
	// Update is called once per frame
	void Update () {
		if((Input.GetKeyDown(KeyCode.Return) || Input.GetMouseButtonDown(0)))
        {
            if (currentPage < pages.Length - 1)
            {
                pages[currentPage].SetActive(false);
                currentPage += 1;
                pages[currentPage].SetActive(true);
            }
            else
            {
                for (int i = 1; i < pages.Length; i++)
                {
                    pages[i].SetActive(false);
                }
                gameObject.SetActive(false);
                GameController.Instance.StartGame();
            }
            page = currentPage;
        }
	}
}
