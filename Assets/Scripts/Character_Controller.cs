using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Character_Controller : MonoBehaviour
{

    private CharacterController controller;
    public float walkSpeed, runSpeed, rotSpeed;
    public float gravityForce;
    public float rotSmoothTime;
    public Animator animator;
    bool isMoving = false;
    bool isWalking = false;
    bool isRunning = false;

    // Start is called before the first frame update
    void Start()
    {

        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        isWalking = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D);

        isRunning = Input.GetKey(KeyCode.LeftShift);
        PlayerControls();
    }

    public void PlayerControls()
    {

        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0, moveVertical).normalized;

        isMoving = movement.magnitude >= 0.1f;

        if (!controller.isGrounded)
        {
            movement.y -= gravityForce * Time.deltaTime;
        }
        else
        {
            movement.y = 0;
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
            animator.SetBool("IsWalking", true);
            animator.SetBool("IsRunning", false);
            controller.Move(walkSpeed * Time.deltaTime * movement);
            float moveMagnitude = movement.magnitude;
            animator.SetFloat("Speed", moveMagnitude);
        }
        else if (isWalking && isRunning)
        {
            animator.SetBool("IsWalking", true);
            animator.SetBool("IsRunning", true);
            controller.Move(runSpeed * Time.deltaTime * movement);
            float moveMagnitude = movement.magnitude;
            animator.SetFloat("Speed", moveMagnitude);
        }
        else
        {
            animator.SetBool("IsWalking", false);
            animator.SetBool("IsRunning", false);
            animator.SetFloat("Speed", 0);
            controller.Move(walkSpeed * Time.deltaTime * movement);
        }

    }
}
