using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorNodeController : MonoBehaviour
{
    Player player;
    // Start is called before the first frame update
    void Start()
    {
        player = GetComponentInParent<Player>();
    }

    public void AfterAttack()
    {
        player.AfterAttack();
    }

    public void PlayAtkSound()
    {
        player.PlayAtkSound();
    }
}
