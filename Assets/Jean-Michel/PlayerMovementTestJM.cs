using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementTestJM : MonoBehaviour
{
    public float moveSpeed;

    public Rigidbody2D rb;
    private Vector3 velocity = Vector3.zero;

    private float horizontalMovement;


    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        horizontalMovement = Input.GetAxis("Horizontal") * moveSpeed * Time.fixedDeltaTime;
    }

    void FixedUpdate()
    {
        MovePlayer(horizontalMovement);
    }

    void MovePlayer(float _horizontalMovement)
    {
        Vector3 targetVelocity = new Vector2(_horizontalMovement, rb.velocity.y);
        rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref velocity, .05f);
    }
}
