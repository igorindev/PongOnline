using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] string inputPlayer = "Player1Vertical";
    [SerializeField] float moveSpeed = 2;
    Rigidbody2D rb;

    float movement;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        movement = Input.GetAxisRaw(inputPlayer);

        rb.velocity = new Vector2(rb.velocity.x, movement * moveSpeed);
    }
}
