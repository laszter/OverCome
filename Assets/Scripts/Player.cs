using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    SpriteRenderer spriteRenderer;
    Animator animator;
    [SerializeField] int player = 1;
    [SerializeField] AudioClip atkAudio;
    [SerializeField] AudioClip dashAudio;
    AudioSource audioSource;

    private string BUTTON_XZONTAL = "Xzontal_P";
    private string BUTTON_ZZONTAL = "Zzontal_P";
    private string BUTTON_DASH = "Dash_P";
    private string BUTTON_Action = "Action_P";

    private bool running;
    private bool onAir;
    private bool attacking;
    private bool repairing;

    private float repairRate;
    private float max_RepairRate = 30f;
    private float repairav = 10f;
    private float inertia = 20f;
    private float av = 8f;
    private float max_Speed = 8f;
    private float speed;
    private float vspeed;
    private float dspeed;
    private float gravity = 0.2f;
    
    private Vector3 targetDirection;
    private Vector3 xDirect;
    private Vector3 zDirect;
    public BreakableController breakable;

    // Use this for initialization
    void Start () {
        BUTTON_XZONTAL += player.ToString();
        BUTTON_ZZONTAL += player.ToString();
        BUTTON_DASH += player.ToString();
        BUTTON_Action += player.ToString();

        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        if (animator == null)
            animator = GetComponentInChildren<Animator>();
        onAir = true;
        speed = 0;
        vspeed = 0;
	}
	
	// Update is called once per frame
	void Update () {
        if (Ready()) {
            /*if (VirtualStick.Instance.Horizontal() < -0.3f)
            {
                xDirect = Vector3.left;
            }
            else if (VirtualStick.Instance.Horizontal() > 0.3f)
            {
                xDirect = Vector3.right;
            }
            else
            {
                xDirect = Vector3.zero;
            }

            if (VirtualStick.Instance.Vertical() < -0.3f)
            {
                zDirect = Vector3.back;
            }
            else if (VirtualStick.Instance.Vertical() > 0.3f)
            {
                zDirect = Vector3.forward;
            }
            else
            {
                zDirect = Vector3.zero;
            }*/

            if (VirtualStick.Instance.dash.IsPointerDown() && dspeed == 0)
            {
                audioSource.PlayOneShot(dashAudio);
                dspeed = 20f;
            }

            if (VirtualStick.Instance.IsPointerDown())
            {
                //begin walk
                animator.SetBool("toWalk", true);
            }

            if (VirtualStick.Instance.IsDraging())
            {
                //walking
                Running(true);
                targetDirection = xDirect + zDirect;
                RotateTo(1000f, VirtualStick.Instance.GetDragDirection());
                
            }
            else
            {
                //not walking
                Running(false);
                if (VirtualStick.Instance.IsPointerUp())
                {
                    animator.SetBool("toWalk", false);
                }
            }
        }
        
        if (VirtualStick.Instance.action.IsPointerHold())
        {
            speed = 0;
            running = false;
            animator.SetBool("toWalk", false);
            if (!attacking && breakable != null && breakable.destruted) //Repair
            {
                //Debug.Log("Reparing");
                LookAtTarget(500);
                dspeed = 0;
                breakable.RepairingProcess(repairRate);
                repairing = true;
                repairRate += repairav * Time.deltaTime;
                if (repairRate > max_RepairRate) repairRate = max_RepairRate;
                animator.SetBool("toRap", repairing);

            }
            else if (VirtualStick.Instance.action.IsPointerDown() && Ready()) //Attack
            {
                attacking = true;
                animator.SetTrigger("toAtk");
            }
            else if (breakable != null && !breakable.destruted)
            {
                RepairReset();
            }

        }
        else if (VirtualStick.Instance.action.IsPointerUp())
        {
            if (breakable != null && breakable.destruted)
                breakable.EndRepair();
            if (repairing)
            {
                RepairReset();
            }
        }

        if (running || onAir)
        {
            if (speed > max_Speed)
            {
                speed = max_Speed;
            }
            else if (speed < -max_Speed)
            {
                speed = -max_Speed;
            }
        }
        else
        {
            if(speed > 0.15f)
            {
                speed -= inertia * Time.deltaTime;
            }else if(speed < -0.15f)
            {
                speed += inertia * Time.deltaTime;
            }
            else
            {
                speed = 0;
            }
        }

        if(dspeed > 0)
        {
            dspeed -= inertia * 5 * Time.deltaTime;
        }else if(dspeed < 0)
        {
            dspeed = 0;
        }

        /*if (Input.GetKeyDown(KeyCode.W) && Ready())
        {
            vspeed = 10f;
            jumping = true;
            animator.SetBool("running", false);
            animator.SetBool("jumping", true);
        }*/

        /*if (Input.GetKeyDown(KeyCode.K))
        {
            dspeed = 0f;
            speed = -5f;
        }*/

        if (onAir)
        {
            vspeed -= gravity;
        }

        if (vspeed < -30f)
        {
            vspeed = -30f;
        }

        /*if(Input.GetKeyDown(KeyCode.V) && Ready())
        {
            attacking = true;
            animator.SetTrigger("attack");
            speed = 0;
        }*/

        transform.position += (transform.forward.normalized * (speed + dspeed) + new Vector3(0f, vspeed)) * Time.deltaTime;
    }

    private bool Ready()
    {
        return !attacking && !repairing;
    }

    public void AfterAttack()
    {
        attacking = false;
    }

    private void Running(bool run)
    {
        speed += av * Time.deltaTime;
        if(speed > max_Speed * VirtualStick.Instance.GetDistance())
        {
            speed = max_Speed * VirtualStick.Instance.GetDistance();
        }
        //VirtualStick.Instance.text.text = VirtualStick.Instance.GetDistance().ToString();
        running = run;
        if (!onAir)
        {
            if (animator != null)
                animator.SetBool("toWalk", running);
        }
    }

    private void RotateTo(float rotSpd, Vector3 direction)
    {
        if (direction == Vector3.zero) return;
        Vector3 rot = Vector3.RotateTowards(transform.forward, direction, Mathf.Deg2Rad * rotSpd * Time.deltaTime, 1);
        transform.rotation = Quaternion.LookRotation(rot, Vector3.up);
    }

    protected void LookAtTarget(float rotSpd)
    {
        if (breakable == null) return;

        //Debug.Log("Looking");
        Vector3 targetPos = breakable.transform.position;
        targetPos = new Vector3(targetPos.x, transform.position.y, targetPos.z);

        Vector3 desiredDirection = targetPos - transform.position;
        Vector3 rot = Vector3.RotateTowards(transform.forward, desiredDirection, Mathf.Deg2Rad * rotSpd * Time.deltaTime, 1);
        transform.rotation = Quaternion.LookRotation(rot, Vector3.up);
    }

    public void PlayAtkSound()
    {
        audioSource.PlayOneShot(atkAudio);
    }

    //----------------------------------- Collider -------------------------------------
    private void OnCollisionEnter(Collision coll)
    {
        if (coll.gameObject.CompareTag("Floor"))
        {
            
                vspeed = 0;
            if (onAir)
            {
                vspeed = 0;
                onAir = false;
                //if(animator!=null)
                    //animator.SetBool("jumping", false);
            }
        }

        if (coll.gameObject.GetComponent<CrashObj>() != null)
        {
            if (dspeed + speed > 10f)
            {
                dspeed = 0f;
                speed = -5f;
            }
        }
    }

    private void OnCollisionExit(Collision coll)
    {
        if (coll.gameObject.CompareTag("Floor"))
        {
            if (!onAir)
            {
                onAir = true;
                //if (animator != null)
                    //animator.SetBool("jumping", true);
            }
        }
    }

    private void OnTriggerEnter(Collider coll)
    {
        if (coll.GetComponent<BreakableController>() != null && !attacking && !coll.GetComponent<BreakableController>().destructing)
        {
            breakable = coll.GetComponent<BreakableController>();
        }
    }

    private void OnTriggerStay(Collider coll)
    {
        if (coll.GetComponent<BreakableController>() != null && !attacking && !coll.GetComponent<BreakableController>().destructing)
        {
            breakable = coll.GetComponent<BreakableController>();
        }
    }

    private void OnTriggerExit(Collider coll)
    {
        if (coll.GetComponent<BreakableController>() != null)
        {
            if (breakable != null)
            {
                breakable.EndRepair();
                breakable = null;
            }
        }
    }

    public void SwapController()
    {
        if (GameController.Instance.SinglePlay)
        {
            if (player == 1) player = 2;
            else if (player == 2) player = 1;
            BUTTON_XZONTAL = BUTTON_XZONTAL.Remove(BUTTON_XZONTAL.Length - 1);
            BUTTON_ZZONTAL = BUTTON_ZZONTAL.Remove(BUTTON_ZZONTAL.Length - 1);
            BUTTON_DASH = BUTTON_DASH.Remove(BUTTON_DASH.Length - 1);
            BUTTON_Action = BUTTON_Action.Remove(BUTTON_Action.Length - 1);

            BUTTON_XZONTAL += player.ToString();
            BUTTON_ZZONTAL += player.ToString();
            BUTTON_DASH += player.ToString();
            BUTTON_Action += player.ToString();
        }
    }

    private void RepairReset()
    {
        repairing = false;
        repairRate = 0f;
        animator.SetBool("toRap", repairing);
    }
}