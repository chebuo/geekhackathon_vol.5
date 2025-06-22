using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test_anim : MonoBehaviour
{
    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = this.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            animator.SetBool("boolD", true);
        }
        else
        {
            animator.SetBool("boolD", false);
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            animator.SetBool("boolL", true);
        }
        else
        {
            animator.SetBool("boolL", false);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            animator.SetBool("boolR", true);
        }
        else
        {
            animator.SetBool("boolR", false);
        }   
    }
}
