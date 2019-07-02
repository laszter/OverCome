using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    private static Score instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public static Score Instance
    {
        get
        {
            return instance;
        }
    }

    public Sprite yes;
	public Sprite no;

	bool[] objectDestroyed = new bool[8];

	public Image[] element;

	//0-7 left to right , up to down.
	public void setDestroyed(int arrayLocation,bool status){
		objectDestroyed[arrayLocation] = status;
	}

		
	// return boolean value.
	bool getDestroyed(int arrayLocation){
		return objectDestroyed[arrayLocation];
	}

    // Start is called before the first frame update
    void Start()
    {
    }

	public void correctionUI(){
		for (int i = 0 ; i < 8 ; i++){
			if(objectDestroyed[i]){
				element[i].sprite = yes;
			}
			else
				element[i].sprite = no;
		}

	}

    // Update is called once per frame
    void Update()
    {
        
    }
}
