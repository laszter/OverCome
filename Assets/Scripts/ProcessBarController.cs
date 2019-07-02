using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProcessBarController : MonoBehaviour
{
    [SerializeField] public float process;
    [SerializeField] Image bar;

    private float timer;
    private float time = 2f;
    float speed = 10f;

    private Color healthy = Color.green;
    private Color unhealthy = Color.red;
    private Color warning = new Vector4(1f,0.5f,0f,1f);

    bool warn;
    bool unheal;

    bool destructing;
    bool repairing;

    // Start is called before the first frame update
    void Start()
    {
        process = 0;
        bar.rectTransform.localScale = Vector3.zero;
        bar.color = healthy;
    }

    // Update is called once per frame
    void Update()
    {
        if (destructing) Destructing();
        bar.rectTransform.localScale = new Vector3(process * 0.01f, 1f, 1f);
        bar.color = Vector4.Lerp(healthy, unhealthy, process * 0.01f);
    }

    private void Destructing()
    {
        if (process < 100f)
        {
            process += speed * Time.deltaTime;
        }
        else
        {
            process = 100f;
            destructing = false;
        }
    }

    public void SetDestruct()
    {
        destructing = true;
        speed = 10f;
    }

    public void SetDestruct(float av)
    {
        destructing = true;
        speed = av;
    }

    public void StopDestruct()
    {
        destructing = false;
    }

    public void Repairing(float av)
    {
        process -= av * Time.deltaTime;
    }
}
