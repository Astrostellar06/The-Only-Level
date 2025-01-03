using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnablePlatformAnimation : MonoBehaviour
{
    void Start()
    {
        Animator[] animators = gameObject.GetComponentsInChildren<Animator>();
        if (gameObject.transform.parent.name == "Level 13")
            foreach (Animator animator in animators)
                animator.enabled = true;
    }
}
