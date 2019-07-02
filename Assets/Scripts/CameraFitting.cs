using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFitting : MonoBehaviour
{
    [SerializeField] private Transform[] camNode;
    [SerializeField] private Transform partition;
    [SerializeField] private Transform partitiony;
    [SerializeField] private Transform partitiony2;
    private GameObject[] players;
    private Camera cam;
    int mask;

    private readonly string Node_Room1 = "Room1";
    private readonly string Node_Room2 = "Room2";
    private readonly string Node_BothRoom = "BothRoom";
    private readonly string Node_SecretRoom = "Secret";
    private readonly string Node_SecretRoom2 = "Secret2";

    private float moveTimer;
    [SerializeField]
    private float moveTime;

    bool moving;
    readonly bool zoomout;
    private Transform currentNode;
    private Transform nextNode;

    private readonly float inertia = 20f;
    private readonly float av = 8f;
    private readonly float maxSpeed = 8f;
    private float speed = 0;

    // Start is called before the first frame update
    void Start()
    {
        currentNode = transform;
        cam = GetComponent<Camera>();
        players = GameObject.FindGameObjectsWithTag("Player");
        mask = ~(1 << LayerMask.NameToLayer("Ignore Raycast") | 1 << LayerMask.NameToLayer("HitBox"));
        speed = 0;
        CamMoveToNode(Node_Room1);
    }

    // Update is called once per frame
    void Update()
    {
        if (moving)
        {
            moveTimer += Time.deltaTime;
            if (moveTimer >= moveTime*0.4f)
            {
                moveTimer = moveTime;
                moving = false;
            }
            
            transform.position = Vector3.Lerp(currentNode.position, nextNode.position, moveTimer / moveTime);
            transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, nextNode.GetComponent<PointController>().GetSide() + new Vector3(34f,0,0), moveTimer / moveTime);
            //transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, nextNode.GetComponent<PointController>().GetSide() + new Vector3(transform.eulerAngles.x, 0, 0), moveTimer / moveTime);
        }
        else
        {
            if (players.Length > 1)
            {
                if (players[0].transform.position.z < partitiony2.position.z || players[1].transform.position.z < partitiony2.position.z)
                {
                    CamMoveToNode(Node_SecretRoom2);
                }
                else if (players[0].transform.position.z < partitiony.position.z || players[1].transform.position.z < partitiony.position.z)
                {
                    CamMoveToNode(Node_SecretRoom);
                }
                else if (players[0].transform.position.x > partition.position.x && players[1].transform.position.x > partition.position.x)
                {
                    CamMoveToNode(Node_Room1);
                }
                else if (players[0].transform.position.x < partition.position.x && players[1].transform.position.x < partition.position.x)
                {
                    CamMoveToNode(Node_Room2);
                }
                else
                {
                    CamMoveToNode(Node_BothRoom);
                }
            }
            else
            {
                if (players[0].transform.position.z < partitiony2.position.z)
                {
                    CamMoveToNode(Node_SecretRoom2);
                }
                else if (players[0].transform.position.z < partitiony.position.z)
                {
                    CamMoveToNode(Node_SecretRoom);
                }
                else if (players[0].transform.position.x > partition.position.x)
                {
                    CamMoveToNode(Node_Room1);
                }
                else if (players[0].transform.position.x < partition.position.x)
                {
                    CamMoveToNode(Node_Room2);
                }
            }
        }
    }

    /*private bool IsOutRange(GameObject toCheck)
    {
        float distance = Vector3.Distance(currentNode.position, toCheck.transform.position);
        Debug.Log(distance + " " + toCheck.name);
        if (distance > 2f && !zoomout)
        {
            zoomout = true;
            return true;
        }
        else if(distance > 0 && distance < 2f && zoomout)
        {
            zoomout = false;
            return true;
        }
        else
        {
            return false;
        }
    }*/

    public void CamMoveToNode(string node)
    {
        switch (node)
        {
            case "Room1":
                nextNode = camNode[0];
                break;
            case "BothRoom":
                nextNode = camNode[1];
                break;
            case "Room2":
                nextNode = camNode[2];
                break;
            case "Secret":
                nextNode = camNode[3];
                break;
            case "Secret2":
                nextNode = camNode[4];
                break;
        }

        currentNode = transform;
        moveTimer = 0;
        moving = true;
    }
}
