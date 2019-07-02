using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecretRoomPartitionControl : MonoBehaviour
{
    public GameObject lightSecret;

    // Update is called once per frame
    void Update()
    {   
        if(GameController.Instance.players[0].transform.position.z < transform.position.z || GameController.Instance.players[1].transform.position.z < transform.position.z)
        {
            if(!lightSecret.activeSelf)
                lightSecret.SetActive(true);
        }
        else if(GameController.Instance.players[0].transform.position.z > transform.position.z + 2.0f && GameController.Instance.players[1].transform.position.z > transform.position.z + 2.0f)
        {
            if (lightSecret.activeSelf)
                lightSecret.SetActive(false);
        }
    }
}
