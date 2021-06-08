using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    CharacterController characterController;
    [Header("Opciones de Personaje")]
    public float walkSpeed = 4.0f;
    public float runSpeed = 12.0f;
    public float jumpSpeed = 8.0f;
    public float gravity = 20.0f;
    //float timeRunning = 0.0f;
    float Loop ;
    bool isRunning = false;
    bool isWalking = false;
    float sprintDuration = 2.0f;
    private float sprintTimer = 0;

    [Header("Opciones de Camara")]
    public Camera cam;
    public float mouseHorizontal = 3.0f;
    public float mouseVertical = 2.0f;
    public float minRotation = -65.0f;
    public float maxRotation = 20.0f;
    float h_mouse , v_mouse;

    private Vector3 move = Vector3.zero;
    
    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        h_mouse = mouseHorizontal * Input.GetAxis("Mouse X");
        v_mouse += mouseVertical * Input.GetAxis("Mouse Y");

        v_mouse = Mathf.Clamp(v_mouse, minRotation, maxRotation);
        cam.transform.localEulerAngles = new Vector3(-v_mouse, 0, 0);
        transform.Rotate(0, h_mouse, 0);
        
        if(characterController.isGrounded)
        {

            move = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));
            if ( Input.GetAxis( "Horizontal" ) !=0f || Input.GetAxis( "Vertical" ) != 0f)
                {
                    if ( Input.GetKey( "left shift" ) || Input.GetKey( "right shift" ) )
                    {
                        // Running
                        isWalking = false;
                        isRunning = true;
                    }
                    else
                    {
                        // Walking
                        isWalking = true;
                        isRunning = false;
                    }
                }
            else
            {
                // Stopped
                isWalking = false;
                isRunning = false;
            }
            
            // check the sprint timer
            if ( isRunning )
            {
                sprintTimer += Time.deltaTime;
            
              if ( sprintTimer > sprintDuration )
                {
                    isRunning = false;
                    isWalking = true;
                    if(isWalking){
                        move = transform.TransformDirection(move) * walkSpeed;
                    }
                }
                else if ( characterController.isGrounded )
                {
                    move = transform.TransformDirection(move) * runSpeed;
                }
            }
            else
            {
                sprintTimer -= Time.deltaTime * 0.5f;
                move = transform.TransformDirection(move) * walkSpeed;
            }
            
            sprintTimer = Mathf.Clamp( sprintTimer, 0, sprintDuration );
            


            //move = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));

            //if(Input.GetKey(KeyCode.LeftShift))
            //    move = transform.TransformDirection(move) * runSpeed;
            //else
            //    move = transform.TransformDirection(move) * walkSpeed;
            
            if (Input.GetKey(KeyCode.Space))
                move.y = jumpSpeed;
        }

        move.y -= gravity * Time.deltaTime;

        characterController.Move(move * Time.deltaTime);
    }
}
