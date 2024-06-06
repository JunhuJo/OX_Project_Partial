using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] Animator animator;
    public float speed = 6f;           
    public float rotationSpeed = 400f; 

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

        rb.MovePosition(transform.position + movement * speed * Time.deltaTime);
        if (movement != Vector3.zero)
        {
            animator.SetTrigger("isRun");
        }
    }

    void Rotate()
    {
        float h = rotationSpeed * Input.GetAxis("Horizontal") * Time.deltaTime;
        transform.Rotate(0, h, 0);
        if (h != 0)
        {
            animator.SetTrigger("isRun");
        }
    }
}
