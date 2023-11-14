using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterMove Controller;
    public float runSpeed = 40f;
    float horizontalMove = 0f;
    bool crouch = false;
    public Animator animator;

    void Update()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal") ;
        animator.SetFloat("Speed", Mathf.Abs(horizontalMove));

        /*if (Input.GetButtonDown("Crouch"))
        {
            crouch = true;
        }
        else if (Input.GetButtonUp("Crouch"))
        {
            crouch = false;
        }*/
    }
    public void OnLanding()
    {
        animator.SetBool("Jumping", false);
        animator.SetBool("Grounded", false);
    }

    void FixedUpdate()
    {
        Controller.Move(horizontalMove * runSpeed * Time.fixedDeltaTime, crouch);
    }
}
