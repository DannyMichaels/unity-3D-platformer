using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
  private Transform target;
  private Vector3 offset;

  public float moveSpeed = 15f;

  // Start is called before the first frame update
  void Start()
  {
    target = FindObjectOfType<PlayerController>().transform;

    offset = transform.position;

    HideComputerMouseCursor();
  }

  // Update is called once per frame
  void Update()
  {
    FollowTarget();
  }

  private void FollowTarget()
  {
    transform.position = Vector3.Lerp(transform.position, target.position + offset, moveSpeed * Time.deltaTime);

    // if for some reason, the player is falling down and the camera is going below the player, we want to stop the camera from going below the player
    if (transform.position.y < offset.y)
    {
      transform.position = new Vector3(transform.position.x, offset.y, transform.position.z);
    }
  }

  private void HideComputerMouseCursor()
  {
    Cursor.lockState = CursorLockMode.Locked;
    // Cursor.visible = false;
  }
}