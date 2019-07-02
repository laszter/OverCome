using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BreakableController : MonoBehaviour
{
    [SerializeField] private Sprite repairIcon;
    [SerializeField] private Sprite breakIcon;

    [SerializeField] private GameObject destructForm;
    [SerializeField] private GameObject resructForm;

    [SerializeField] AudioClip breakedAudio;
    [SerializeField] AudioClip repairedAudio;
    AudioSource audioSource;

    private PointController point;
    private ProcessBarController processBar;
    private IconObject icon;
    public bool destruted;
    public bool finished;
    public bool destructing;
    public bool iconShow;
    public bool locked;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        destruted = false;
        point = GetComponentInChildren<PointController>();
        processBar = GetComponentInChildren<ProcessBarController>();
        processBar.gameObject.SetActive(false);
        icon = GetComponentInChildren<IconObject>();
        icon.GetComponent<Image>().sprite = breakIcon;
        icon.transform.parent.parent.gameObject.SetActive(false);
        ResructionForm();
    }

    // Update is called once per frame
    void Update()
    {
        if (destructing)
        {
            if ((processBar.process >= 100f && !destruted) || (processBar.process <= 0 && destruted))
            {
                finished = true;
                processBar.gameObject.SetActive(false);
                if (destruted)
                    HideIcon();
            }
            if (processBar.process >= 50f && !destruted)
            {
                destruted = true;
                DestructedForm();
                ShowIcon();
                audioSource.PlayOneShot(breakedAudio);
            }
            else if (processBar.process <= 0f && destruted)
            {
                destruted = false;
            }
        }
        else if (destruted)
        {
            if(processBar.process < 100)
            {
                processBar.SetDestruct(5f);
                ShowIcon();
            }
            else
            {
                processBar.gameObject.SetActive(false);
            }
        }
    }

    public void BeginDestruction(float speed)
    {
        if (processBar != null)
        {
            processBar.gameObject.SetActive(true);
            finished = false;
            processBar.SetDestruct(speed);
            destructing = true;
        }
    }

    public void StopDestruction()
    {
        if (processBar != null)
        {
            if (processBar.process >= 50f && !destruted)
            {
                destruted = true;
                DestructedForm();
                ShowIcon();
                audioSource.PlayOneShot(breakedAudio);
            }else if(processBar.process < 50f && !destruted)
            {
                processBar.process = 0;
                processBar.gameObject.SetActive(false);
            }
            else if (processBar.process <= 0f && destruted)
            {
                destruted = false;
                processBar.gameObject.SetActive(false);
            }
            destructing = false;
            processBar.StopDestruct();
        }
        locked = false;
    }

    public void SetDestroy()
    {
        StartCoroutine(WaitProcessBarLoad());
        destruted = true;
        DestructedForm();
        ShowIcon();
        audioSource.PlayOneShot(breakedAudio);
        processBar.gameObject.SetActive(true);
        processBar.process = 60f;
    }

    IEnumerator WaitProcessBarLoad()
    {
        yield return new WaitUntil(() => processBar != null);
        processBar.gameObject.SetActive(true);
        processBar.process = 60f;
    }

    public void RepairingProcess(float repairRate)
    {
        if (!destructing && destruted)
        {
            icon.GetComponent<Image>().sprite = repairIcon;
            icon.GetComponent<Animator>().enabled = true;
            processBar.Repairing(repairRate);
            if (processBar.process <= 0f)
            {
                processBar.StopDestruct();
                processBar.process = 0f;
                destruted = false;
                ResructionForm();
                HideIcon();
                audioSource.PlayOneShot(repairedAudio);
            }
            else
            {
                processBar.gameObject.SetActive(true);
            }
        }
    }

    public void EndRepair()
    {
        if (!destructing && destruted)
        {
            icon.GetComponent<Image>().sprite = breakIcon;
            icon.GetComponent<Image>().rectTransform.localEulerAngles = Vector3.zero;
            icon.GetComponent<Animator>().enabled = false;
        }
    }

    private void ShowIcon()
    {
        if (!iconShow)
        {
            icon.transform.parent.parent.gameObject.SetActive(true);
            icon.GetComponent<Animator>().enabled = false;
            iconShow = true;
            if (destruted)
            {
                icon.GetComponent<Image>().sprite = breakIcon;
                icon.GetComponent<Image>().rectTransform.localEulerAngles = Vector3.zero;
                icon.GetComponent<Animator>().enabled = false;
            }
        }
    }

    private void HideIcon()
    {
        if (iconShow)
        {
            iconShow = false;
            icon.transform.parent.parent.gameObject.SetActive(false);
            processBar.gameObject.SetActive(false);
        }
    }

    private void DestructedForm()
    {
        if (resructForm != null)
            resructForm.SetActive(false);
        if (destructForm != null)
            destructForm.SetActive(true);
    }

    private void ResructionForm()
    {
        if (destructForm != null)
            destructForm.SetActive(false);
        if (resructForm != null)
            resructForm.SetActive(true);
    }
}
