using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody rb;
    public float movementSpeed = 2f;
    public float jumpForce = 2.2f;
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask ground;
    [SerializeField] GameObject environment;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        float horizontalImput = Input.GetAxis("Horizontal");
        float verticalImput = Input.GetAxis("Vertical");
        float rotationInput = Input.GetAxis("Mouse X");

        rb.velocity = new Vector3(horizontalImput * movementSpeed, rb.velocity.y ,verticalImput * movementSpeed);

        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            Jump();
        }

    }

    public void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
    }

    public bool IsGrounded()
    {
        return Physics.CheckSphere(groundCheck.position, 0.01f, ground);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            environment.GetComponent<FootballPlayer>().AddReward(0.01f);
        }
    }

    public void RestartPlayerPosition()
    {
        // Resets player position randomly
        float randomNumberX = (float)environment.GetComponent<FootballPlayer>().rnd.NextDouble()*3f; // Random.Range(0f, 3f);
        float randomNumberY = (float)environment.GetComponent<FootballPlayer>().rnd.NextDouble()*0.5f; // Random.Range(0f, 0.5f);
        float randomNumberZ = (float)environment.GetComponent<FootballPlayer>().rnd.NextDouble()*4f; // Random.Range(0f, 4f);

        GetComponent<Transform>().localPosition = new Vector3(
            -1.5f + randomNumberX,
            0.3f + randomNumberY,
            -2.5f + randomNumberZ);
        
        GetComponent<Rigidbody>().velocity = new Vector3(0,0,0);
        GetComponent<Rigidbody>().angularVelocity = new Vector3(0,0,0);
    }

}