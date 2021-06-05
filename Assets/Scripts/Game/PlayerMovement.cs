using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] string inputPlayer = "Player1Vertical";
    [SerializeField] float moveSpeed = 2;
    Rigidbody2D rb;

    public bool server;

    float movement;

    public float Movement { get => movement; set => movement = value; }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (server)
        {
            Movement = Input.GetAxisRaw(inputPlayer);

            rb.velocity = new Vector2(rb.velocity.x, Movement * moveSpeed);
        }
    }
}
