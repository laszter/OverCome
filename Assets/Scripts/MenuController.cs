using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MenuController : MonoBehaviour
{
    [SerializeField] GameObject[] menuPage;
    private int currentMenuPage;

    [Serializable]
    public class Charactor
    {
        public GameObject charactorObj;
        public bool selected;

        public void DimMaterials()
        {
            if (charactorObj != null)
            {
                MeshRenderer[] meshRenderers = charactorObj.GetComponentsInChildren<MeshRenderer>();
                for(int i = 0; i < meshRenderers.Length; i++)
                {
                    meshRenderers[i].material.color = Color.gray;
                }
            }
        }

        public void LightMaterials()
        {
            if (charactorObj != null)
            {
                MeshRenderer[] meshRenderers = charactorObj.GetComponentsInChildren<MeshRenderer>();
                for (int i = 0; i < meshRenderers.Length; i++)
                {
                    meshRenderers[i].material.color = Color.white;
                }
            }
        }

        public void HideObject()
        {
            charactorObj.SetActive(false);
        }
    }

    public Charactor[] charactors;

    // Start is called before the first frame update
    void Start()
    {
        currentMenuPage = 0;
        for (int i = 0; i < menuPage.Length; i++)
        {
            if(i > 0)
            {
                menuPage[i].SetActive(false);
            }
            else
            {
                menuPage[i].SetActive(true);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SelectCharactor(int charactorNumber)
    {
        if (charactors[charactorNumber].selected)
        {
            //GameStart
            for(int i = 0; i < charactors.Length; i++)
            {
                if (i != charactorNumber)
                {
                    charactors[i].charactorObj.SetActive(false);
                }
            }

            GameController.Instance.ChooseCharactor(charactorNumber);
            menuPage[1].SetActive(false);
            return;
        }

        foreach(Charactor ch in charactors)
        {
            ch.selected = false;
            ch.DimMaterials();
        }
        charactors[charactorNumber].selected = true;
        charactors[charactorNumber].LightMaterials();
    }

    public void SingleButton_Click()
    {
        foreach(Charactor ch in charactors)
        {
            ch.selected = false;
            ch.DimMaterials();
        }
        OpenMenuPage(1);
    }

    public void BackToMainMenu()
    {
        foreach (Charactor ch in charactors)
        {
            ch.selected = false;
            ch.LightMaterials();
        }
        OpenMenuPage(0);
    }

    private void OpenMenuPage(int target)
    {
        menuPage[currentMenuPage].SetActive(false);
        menuPage[target].SetActive(true);
        currentMenuPage = target;
    }
}
