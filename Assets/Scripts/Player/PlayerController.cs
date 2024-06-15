using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
  public float moveSpeed;
  public float jumpForce, gravityScale;
  private float yStore;
  public float rotateSpeed = 10f;
  private Vector3 moveAmount;
  public CharacterController characterController;
  private CameraController cameraController;
  public Animator anim;

  public GameObject jumpParticle, landingParticle;
  private bool lastGrounded;

  void Awake()
  {
    cameraController = FindObjectOfType<CameraController>();
    lastGrounded = true;
  }

  // Start is called before the first frame update
  void Start()
  {
    characterController.Move(new Vector3(0f, Physics.gravity.y * gravityScale * Time.deltaTime, 0f));
  }

  // Update is called once per frame
  void Update()
  {
    HandleMovement();
    HandleAnimation();
  }

  // doesn't run every frame, runs every physics frame
  void FixedUpdate()
  {
    HandleGravity();
  }



  void HandleMovement()
  {
    // transform.position = new Vector3(
    //   transform.position.x + (Input.GetAxisRaw("Horizontal") * moveSpeed * Time.deltaTime),
    //   transform.position.y,
    //   transform.position.z + (Input.GetAxisRaw("Vertical") * moveSpeed * Time.deltaTime)
    // );


    // using character controller instead of transform.position
    // characterController.Move(
    //   new Vector3(
    //     Input.GetAxisRaw("Horizontal") * moveSpeed,
    //     0f,
    //     Input.GetAxisRaw("Vertical") * moveSpeed
    //   ) * Time.deltaTime
    // );

    yStore = moveAmount.y; // store the y value so we don't lose it when we reset moveAmount

    // move in the direction the camera is facing
    moveAmount = (cameraController.transform.forward * Input.GetAxisRaw("Vertical")) + (cameraController.transform.right * Input.GetAxisRaw("Horizontal"));
    moveAmount.y = 0f; // don't move up or down 
    moveAmount = moveAmount.normalized; // normalize so diagonal movement isn't faster

    if (moveAmount.magnitude > .1f)
    {

      if (moveAmount != Vector3.zero)
      {
        // quaternion is a way to represent rotation in 3D space in Unity
        Quaternion newRotation = Quaternion.LookRotation(moveAmount);

        transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, rotateSpeed * Time.deltaTime);
      }
    }

    moveAmount.y = yStore; // reset the y value


    HandleJump();

    lastGrounded = characterController.isGrounded;

    characterController.Move(new Vector3(moveAmount.x * moveSpeed, moveAmount.y, moveAmount.z * moveSpeed) * Time.deltaTime);
  }

  private void HandleGravity()
  {
    if (!characterController.isGrounded)
    {
      // fixedDeltaTime how much time has passed since the last time fixedUpdate was called
      moveAmount.y += Physics.gravity.y * gravityScale * Time.fixedDeltaTime;
    }
    else
    {
      moveAmount.y = Physics.gravity.y * gravityScale * Time.deltaTime;
    }
  }

  private void HandleAnimation()
  {
    float moveVelocity = new Vector3(moveAmount.x, 0f, moveAmount.z).magnitude * moveSpeed;
    anim.SetFloat("speed", moveVelocity);
    anim.SetBool("isGrounded", characterController.isGrounded);
    anim.SetFloat("yVel", moveAmount.y);
  }

  private void HandleJump()
  {
    if (!characterController.isGrounded) return;

    jumpParticle.SetActive(false);

    if (!lastGrounded)
    {
      landingParticle.SetActive(true);
    }

    if (Input.GetButtonDown("Jump"))
    {
      landingParticle.SetActive(false);
      moveAmount.y = jumpForce;
      jumpParticle.SetActive(true);
    }
  }
}
