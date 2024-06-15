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
  }

  // Update is called once per frame
  void Update()
  {
    FollowTarget();
  }

  private void FollowTarget()
  {
    transform.position = target.position + offset;
  }
}
