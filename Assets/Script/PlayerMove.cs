using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float speed = 6f;            // 이동 속도
    public float rotationSpeed = 400f;  // 회전 속도
    public float jumpForce = 5f;        // 점프 힘
    private bool isGrounded = true;     // 바닥에 닿았는지 여부

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        Move();
        Rotate();

    }

    void Move()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        transform.Translate(movement * speed * Time.deltaTime);
        //transform.Rotate(movement * rotationSpeed * Time.deltaTime);


        rb.MovePosition(transform.position + movement * speed * Time.deltaTime);
    }

    void Rotate()
    {
        float h = rotationSpeed * Input.GetAxis("Horizontal") * Time.deltaTime;
        transform.Rotate(0, h, 0);
    }
}
