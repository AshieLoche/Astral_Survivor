using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Character_Controller : MonoBehaviour
{

    private CharacterController controller;
    public float walkSpeed, runSpeed, rotSpeed, desiredSpeed;
    public float gravityForce;
    public float rotSmoothTime;
    public Animator animator;
    bool isMoving = false;
    bool isWalking = false;
    bool isRunning = false;

    //private Vector3 playerVelocity;
    [SerializeField]
    private float jumpHeight = 1.0f;
    [SerializeField]
    private bool isGrounded = false;
    private InputManager inputManager;
    private Transform cameraTransform;

    // Start is called before the first frame update
    void Start()
    {

        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        cameraTransform = Camera.main.transform;

        inputManager = InputManager.Instance;

    }

    // Update is called once per frame
    void Update()
    {

        desiredSpeed = isRunning ? runSpeed : walkSpeed;

        //isWalking = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D);
        isWalking = inputManager.GetMovementStatus();

        //isRunning = Input.GetKey(KeyCode.LeftShift);
        isRunning = inputManager.GetRunStatus();

        isGrounded = controller.isGrounded;
        PlayerControls();
    }

    public void PlayerControls()
    {

        //float moveHorizontal = Input.GetAxis("Horizontal");
        //float moveVertical = Input.GetAxis("Vertical");

        //Vector3 movement = new Vector3(moveHorizontal, 0, moveVertical).normalized;

        Vector2 movement2d = inputManager.GetMovement();
        Vector3 movement = new Vector3(movement2d.x, 0, movement2d.y).normalized;
        movement = cameraTransform.forward * movement.z + cameraTransform.right * movement.x;
        movement.y = 0f;

        isMoving = movement.magnitude >= 0.1f;

        if (isGrounded)
        {
            movement.y = 0;
        }
        else
        {
            movement.y -= gravityForce * Time.deltaTime;
        }

        if (isMoving)
        {
            float targetAngle = Mathf.Atan2(movement.x, movement.z) * Mathf.Rad2Deg;
            targetAngle = Mathf.Clamp(targetAngle, -180, 180);
            float smoothAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref rotSpeed, rotSmoothTime);

            transform.rotation = Quaternion.Euler(0f, smoothAngle, 0f);
        }

        if (isWalking && !isRunning)
        {
            desiredSpeed = walkSpeed;
            animator.SetBool("IsWalking", true);
            animator.SetBool("IsRunning", false);
        }
        else if (isWalking && isRunning)
        {
            desiredSpeed = runSpeed;
            animator.SetBool("IsWalking", true);
            animator.SetBool("IsRunning", true);
        }
        else
        {
            animator.SetBool("IsWalking", false);
            animator.SetBool("IsRunning", false);
        }

        if (inputManager.GetJumpStatus() && isGrounded)
        {
            movement.y += Mathf.Sqrt(jumpHeight * Time.deltaTime);
        }
        
        controller.Move(desiredSpeed * Time.deltaTime * movement);
    }

}
