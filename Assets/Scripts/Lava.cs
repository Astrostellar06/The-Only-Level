using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lava : MonoBehaviour
{
    [SerializeField] private Animator animator;
    void Start()
    {
        LavaDown();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            animator.Play("LavaRise");
            Manager.instance.Death();
            Invoke(nameof(LavaDown), .1f);
        }
    }

    private void LavaDown()
    {
        animator.Play("LavaDown");
    }
}
