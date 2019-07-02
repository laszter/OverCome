using UnityEngine;

public class RatController : AnimalEvil
{

    //--------------------------------------- Collider -----------------------------------------
    private void OnTriggerEnter(Collider coll)
    {
        if (coll.GetComponent<BreakableController>() != null)
        {
            reachObj = true;
            breakable = coll.GetComponent<BreakableController>();
        }

        if (coll.gameObject.CompareTag("PlayerHitBox") && (coll.GetComponent<PlayerMeleeAtk>() != null || GameController.Instance.SinglePlay))
        {
            if (action)
            {
                BackToHide();
            }
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
