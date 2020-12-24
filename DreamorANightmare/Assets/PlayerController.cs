using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerController : MonoBehaviour
{
    public ParticleSystem Dust;

    public Rigidbody2D rb;
    private Animator myAnimator;
    [SerializeField]
    private Transform[] groundPoints;
    [SerializeField]
    private float movementspeed;

    [SerializeField]
    private float dashspeed;
    [SerializeField]
    private float groundRad;
    [SerializeField]
    private float jumpForce;
    [SerializeField]
    private LayerMask whatIsGround;

    [SerializeField]
    private bool airControl;
    private bool jumping;
    private bool isGrounded;
    private bool isFacingright=true;
    private bool dash;
   

    private void Start()
    {
        
        rb = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
    }

   
    private void Update()
    {
        HandleInput();
    }
    private void FixedUpdate()
    {
        float horizontal = Input.GetAxis("Horizontal");
        isGrounded = IsGrounded();
        HandleMovement(horizontal);
        ResetValue();
        Flip(horizontal);
    }
    private void HandleMovement(float horizontal)
    {
        if (!myAnimator.GetBool("dash") &&  (isGrounded||airControl))
        {
            
            rb.velocity = new Vector2(horizontal * movementspeed, rb.velocity.y);
        }
        if (isGrounded && jumping)
        {
            isGrounded = false;
            
            rb.AddForce(new Vector2(0,jumpForce));
        }
        

        if (dash&& !this.myAnimator.GetCurrentAnimatorStateInfo(0).IsName("dashAnim"))
        {
            myAnimator.SetBool("dash", true);
            
            rb.velocity = new Vector2(horizontal *dashspeed, rb.velocity.y);
            
        }
        else if( !this.myAnimator.GetCurrentAnimatorStateInfo(0).IsName("dashAnim"))
        {
            myAnimator.SetBool("dash",false);
        }
        myAnimator.SetFloat("speed", Mathf.Abs(horizontal));
    }
    private void HandleInput()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            jumping = true;
        }
        if(Input.GetKeyDown(KeyCode.LeftControl))
        {
            dash = true;
        }
    }
    private void Flip(float horizontal)
    {
        if(horizontal > 0 && !isFacingright || horizontal < 0 && isFacingright)
        {
            isFacingright = !isFacingright;
            Vector3 theScale = transform.localScale;

            theScale.x *= -1;

            transform.localScale = theScale;
        }
    }

    private void ResetValue()
    {
        dash = false;
        jumping = false;
    }
    void CreateDust()
    {
        Dust.Play();
    }
  private bool IsGrounded()
    {
        if(rb.velocity.y<=0)
        {
            foreach(Transform point in groundPoints)
            {
                Collider2D[] colliders=Physics2D.OverlapCircleAll(point.position,groundRad,whatIsGround);
                for(int i=0;i<colliders.Length;i++)
                {
                    if(colliders[i].gameObject !=gameObject)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }
}