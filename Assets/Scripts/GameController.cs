using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

    private static GameController instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public static GameController Instance
    {
        get
        {
            return instance;
        }
    }

    public bool pause;

    private AudioSource audioSource;
    private AudioSource subAudioSource;
    [SerializeField] private AudioClip bgm;
    [SerializeField] private AudioClip bell;
    [SerializeField] GameObject virtualstickUI;
    [SerializeField] RaccoonController raccoon;
    [SerializeField] RatController rat;
    public float spawnRacTimer;
    public float spawnRacTime = 10f;
    public float spawnRatTimer;
    public float spawnRatTime = 10f;
    public bool spawnRac;
    public bool SpawnRat;

    float tempRacTime;
    float tempRatTime;

    bool desallTrigger;
    bool[] step;
    int rs; //round step

    public float timer;
    bool startTimer;

    public GameObject[] players;
    private bool singleplay;

    public bool SinglePlay
    {
        get
        {
            return singleplay;
        }
    }

    [SerializeField] private CameraFitting camera;
    [SerializeField] private Transform[] objectNodes;
    [SerializeField] private BreakableController[] Objective;
    [SerializeField] GameObject scoreUI;
    [SerializeField] private Transform endpos1;
    [SerializeField] private Transform endpos2;
    [SerializeField] private Animator endAnimation;
    [SerializeField] private Animator introAnimation;
    [SerializeField] private GameObject ttUI;
    [SerializeField] private GameObject pauseUI;

    [SerializeField] GameObject carLight;

    // Use this for initialization
    void Start ()
    {
        camera.enabled = false;
        players = GameObject.FindGameObjectsWithTag("Player");
        players[0].GetComponent<Player>().enabled = false;
        players[1].GetComponent<Player>().enabled = false;
        step = new bool[10];
        for(int i = 0; i < step.Length; i++)
        {
            step[i] = false;
        }
        spawnRacTimer = 0;
        spawnRatTimer = 0;
        timer = 0;
        carLight.SetActive(false);
        rs = 0;
        audioSource = GetComponent<AudioSource>();
        subAudioSource = GetComponentInChildren<AudioSource>();
        virtualstickUI.SetActive(false);

    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Escape) && startTimer)
        {
            if (pause)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }

        if (!raccoon.IsInScene() && spawnRac)
        {
            spawnRacTimer += Time.deltaTime;
            if (spawnRacTimer >= spawnRacTime)
            {
                spawnRacTimer = 0f;
                raccoon.StartAttack(RandomTargetObjectNode());
                Debug.Log("Spawn Raccoon");
            }
        }

        if (!rat.IsInScene() && SpawnRat)
        {
            spawnRatTimer += Time.deltaTime;
            if (spawnRatTimer >= spawnRatTime)
            {
                spawnRatTimer = 0f;
                rat.StartAttack(RandomTargetObjectNode());
                Debug.Log("Spawn Rat");
            }
        }

        if (IsDestroyedAll() && !desallTrigger)
        {
            desallTrigger = true;
            if (raccoon.IsInScene())
            {
                raccoon.BackToHide();
            }

            if (rat.IsInScene())
            {
                rat.BackToHide();
            }

            spawnRac = false;
            SpawnRat = false;
            spawnRacTimer = 0;
            spawnRatTimer = 0;
        }else if (!IsDestroyedAll() && desallTrigger && !raccoon.IsInScene() && !rat.IsInScene())
        {
            spawnRac = true;
            SpawnRat = true;
            desallTrigger = false;
        }

        //------------------------------ Timer -------------------------------------
        if (timer > 0f && !step[0])
        {
            audioSource.clip = bgm;
            audioSource.Play();
            spawnRac = true;
            SpawnRat = true;
            step[0] = true;
        }
        else if (timer > 30f && !step[1])
        {
            raccoon.speedDestruct += 1;
            rat.speedDestruct += 1;
            spawnRacTime -= 1;
            spawnRatTime -= 1;
            step[1] = true;
        }
        else if (timer > 60f && !step[2])
        {
            raccoon.AddNavMeshSpeed(2f);
            rat.AddNavMeshSpeed(2f);
            raccoon.speedDestruct += 1;
            rat.speedDestruct += 1;
            spawnRacTime -= 1;
            spawnRatTime -= 1;
            step[2] = true;
        }
        else if (timer > 90f && !step[3])
        {
            raccoon.speedDestruct += 1;
            rat.speedDestruct += 1;
            spawnRacTime -= 1;
            spawnRatTime -= 1;
            step[3] = true;
        }
        else if (timer > 120f && !step[4])
        {
            raccoon.AddNavMeshSpeed(5f);
            rat.AddNavMeshSpeed(5f);
            raccoon.speedDestruct += 1;
            rat.speedDestruct += 1;
            spawnRacTime -= 1;
            spawnRatTime -= 1;
            step[4] = true;
        }
        else if (timer > 150f && !step[5])
        {
            raccoon.speedDestruct += 1;
            rat.speedDestruct += 1;
            spawnRacTime -= 1;
            spawnRatTime -= 1;
            step[5] = true;
            subAudioSource.PlayOneShot(bell);
        }
        else if (timer > 165f && !step[6])
        {
            carLight.SetActive(true);
            step[6] = true;
        }
        else if (timer > 180f && !step[7])
        {
            if (raccoon.IsInScene())
            {
                raccoon.BackToHide();
            }

            if (rat.IsInScene())
            {
                rat.BackToHide();
            }

            spawnRac = false;
            SpawnRat = false;
            spawnRacTimer = 0;
            spawnRatTimer = 0;
            endAnimation.enabled = true;
            startTimer = false;
            step[7] = true;
        }

        if(startTimer)
            timer += Time.deltaTime;
    }

    private Transform RandomTargetObjectNode()
    {
        OverComeUltility.Shuffle(objectNodes);
        for (int i = 0; i < objectNodes.Length; i++)
        {
            if (!objectNodes[i].GetComponentInParent<BreakableController>().destruted && !objectNodes[i].GetComponentInParent<BreakableController>().locked)
            {
                Debug.Log("Lock " + i + objectNodes[i].parent.name);
                objectNodes[i].GetComponentInParent<BreakableController>().locked = true;
                return objectNodes[i];
            }
        }
        return null;
    }

    private bool IsDestroyedAll()
    {
        bool desall = true;
        for (int i = 0; i < objectNodes.Length; i++)
        {
            if (!objectNodes[i].GetComponentInParent<BreakableController>().destruted)
            {
                desall = false;
            }
        }
        return desall;
    }

    public void EndGame()
    {
        scoreUI.SetActive(true);
        for (int i = 0; i < Objective.Length; i++)
        {
            Score.Instance.setDestroyed(i, !Objective[i].destruted);
        }
        Score.Instance.correctionUI();
    }

    public void SetEndPos()
    {
        players[0].GetComponent<Player>().enabled = false;
        players[1].GetComponent<Player>().enabled = false;
        players[0].transform.position = endpos1.position;
        players[0].transform.rotation = endpos1.rotation;
        players[1].transform.position = endpos2.position;
        players[1].transform.rotation = endpos2.rotation;
        virtualstickUI.SetActive(false);
    }

    public void PrepareStartGame()
    {
        introAnimation.enabled = true;
    }

    public void StartGameSetup()
    {
        camera.enabled = true;
        ttUI.SetActive(true);
    }

    public void StartGame()
    {
        for (int i = 0; i < 3; i++)
        {
            int r = Random.Range(0, 7);
            Objective[r].SetDestroy();
        }
        startTimer = true;
        players[0].GetComponent<Player>().enabled = true;
        players[1].GetComponent<Player>().enabled = true;
        introAnimation.gameObject.SetActive(false);
        virtualstickUI.SetActive(true);
    }

    public void RestartGame()
    {
        if(pause)
            ResumeGame();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void PauseGame()
    {
        audioSource.Pause();
        pause = true;
        pauseUI.SetActive(true);
        virtualstickUI.SetActive(false);
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        audioSource.Play();
        pause = false;
        virtualstickUI.SetActive(true);
        pauseUI.SetActive(false);
        Time.timeScale = 1f;
    }

    public void ChooseCharactor(int player)
    {
        singleplay = true;
        if(player == 1)
        {
            players[1].SetActive(false);
            players[0].GetComponent<Player>().SwapController();
        }
        else if(player == 0)
        {
            players[0].SetActive(false);
            players[1].GetComponent<Player>().SwapController();
        }
        PrepareStartGame();
    }
}
