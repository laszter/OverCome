using UnityEngine;

public class RaccoonController : AnimalEvil
{

    //--------------------------------------- Collider -----------------------------------------
    private void OnTriggerEnter(Collider coll)
    {
        if (coll.GetComponent<BreakableController>() != null)
        {
            reachObj = true;
            breakable = coll.GetComponent<BreakableController>();
        }

        if (coll.gameObject.CompareTag("PlayerHitBox") && (coll.GetComponent<PlayerRangeAtk>() != null || GameController.Instance.SinglePlay))
        {
            if(action)
                BackToHide();
        }
    }

    private void OnTriggerExit(Collider coll)
    {
        if (coll.GetComponent<BreakableController>() != null)
        {
            if (breakable != null && coll.GetComponent<BreakableController>() == breakable)
            {
                breakable.StopDestruction();
                breakable = null;
            }
        }
    }
}
