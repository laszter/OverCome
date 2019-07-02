using UnityEngine;
using UnityEngine.AI;

public class AnimalEvil : MonoBehaviour
{
    public Transform currentNode;
    int scary;
    float speed;
    NavMeshAgent navMeshAgent;
    Animator anim;
    public bool move = true;
    public bool afterMove;
    public bool action;
    public bool walking;
    public bool inScene;
    public bool reachObj;
    public bool finishedWork;
    public float speedDestruct;
    Transform targetLookAt;
    int mask;
    public GameObject raycastOrigin;

    private float turningTimer;
    private float turningTime = 2f;

    private float sceneTime;

    public BreakableController breakable;

    [Header("Node")]
    [SerializeField] private Transform[] spawnNodes;

    // Start is called before the first frame update
    void Start()
    {
        sceneTime = 0;
        reachObj = false;
        inScene = false;
        mask = ~(1 << LayerMask.NameToLayer("Ignore Raycast") | 1 << LayerMask.NameToLayer("HitBox"));
        navMeshAgent = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (ReachDestination())
        {
            AfterMove();
            if (action && breakable != null && breakable.finished)
            {
                BackToHide();
                finishedWork = true;
                action = false;
            }
            else if (reachObj && breakable == null)
            {
                Hidden();
            }
        }

        if (inScene)
        {
            sceneTime += Time.deltaTime;
            if (sceneTime > 15f)
            {
                sceneTime = 15f;
                BackToHide();
            }
        }
    }

    void MoveToPosition(Transform movePos)
    {
        if (movePos == null) return;

        RaycastHit hit;
        if (Physics.Raycast(movePos.transform.position, -movePos.transform.up, out hit, 10f, mask))
        {
            currentNode = movePos;
            navMeshAgent.SetDestination(hit.point);
        }
        //Debug.DrawRay(movePos.transform.position, -movePos.transform.up, Color.red);
        walking = true;
        //anim.SetBool("walk", true);
        afterMove = true;
        turningTimer = 0;
        navMeshAgent.isStopped = false;
    }

    private void TurnToTarget()
    {
        if (targetLookAt == null) return;

        turningTimer += Time.deltaTime;
        if (turningTimer >= turningTime)
        {
            turningTimer = turningTime;
        }

        Vector3 direction = transform.position - targetLookAt.position;

        transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, direction, turningTimer / turningTime);
    }

    private void AfterMove()
    {
        if (currentNode == null) return;

        turningTimer += Time.deltaTime;
        if (turningTimer >= turningTime * 0.5f)
        {
            turningTimer = turningTime;
            afterMove = false;
            if (breakable != null && !action)
            {
                action = true;
                anim.SetBool("atk", true);
                breakable.BeginDestruction(speedDestruct);
            }
        }

        if (afterMove)
        {
            transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, currentNode.GetComponent<PointController>().GetSide(), turningTimer / turningTime);
        }
    }

    private bool ReachDestination()
    {
        if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance < 0.1f)
        {
            walking = false;
            //anim.SetBool("walk", false);
            navMeshAgent.isStopped = true;
            return true;
        }
        return false;
    }

    public void BackToHide()
    {
        if (breakable != null)
        {
            anim.SetBool("atk", false);
            breakable.StopDestruction();
            breakable = null;
            MoveToPosition(RandomSpawnPoint());
        }
    }

    private Transform RandomSpawnPoint()
    {
        return spawnNodes[Random.Range(0, spawnNodes.Length)];
    }

    public void StartAttack(Transform target)
    {
        if (target == null) return;
        finishedWork = false;
        inScene = true;
        reachObj = false;
        action = false;
        gameObject.SetActive(true);
        Transform startNode = RandomSpawnPoint();
        transform.position = startNode.position;
        transform.eulerAngles = startNode.GetComponent<PointController>().GetSide();
        currentNode = startNode;
        MoveToPosition(target);
    }

    public void Hidden()
    {
        inScene = false;
        gameObject.SetActive(false);
        sceneTime = 0;
    }

    public bool IsInScene()
    {
        return inScene;
    }

    public void AddNavMeshSpeed(float speed)
    {
        navMeshAgent.speed += speed;
    }
}
