using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [Header("Movement Stats")]
    public float groundAcceleration;
    public float maxGroundSpeed;
    public float airAcceleration;
    public float maxAirSpeed;
    public float noInputDeccel;
    public float jumpForce;
    public InputMaster inputMaster;
    public BoxTrigger ground;
    public bool isGrounded;
    bool wasGrounded;
    bool isJumping;
    Rigidbody rb;
    Vector3 inputDir;
    Vector3 movDir;

    void OnEnable(){

        inputMaster.Enable();

    }

    void OnDisable(){

        inputMaster.Disable();

    }

    // Start is called before the first frame update
    void Awake()
    {
        
        rb = GetComponent<Rigidbody>();
        inputMaster = new InputMaster();
        inputMaster.Player.Jump.performed += ctx => Jump();

    }

    void Update(){

        inputDir = inputMaster.Player.Movement.ReadValue<Vector2>();
        movDir = inputDir.normalized * groundAcceleration;

    }

    // Update is called once per frame
    void FixedUpdate(){

        GroundCheck();

        rb.AddForce((transform.forward * movDir.y) + (transform.right * movDir.x),ForceMode.Acceleration);

        if(movDir.magnitude == 0){

            rb.AddForce(-rb.velocity * noInputDeccel,ForceMode.Acceleration);

        }

        if(isGrounded && rb.velocity.magnitude > maxGroundSpeed){

            rb.velocity = rb.velocity.normalized * maxGroundSpeed;

        } else if(!isGrounded && rb.velocity.magnitude > maxAirSpeed){

            rb.velocity = rb.velocity.normalized * maxAirSpeed;

        }

    }

    void Jump(){

        if(isGrounded){

            rb.AddForce(Vector3.up * jumpForce,ForceMode.Impulse);

        }

    }

    void GroundCheck(){

        wasGrounded = isGrounded;
        isGrounded = ground.isTrue;
        
        //if they weren't touching the ground before but now they are give the player the ability to jump
        if(!wasGrounded &&  isGrounded){

            isJumping = false;

        }

    }

}
