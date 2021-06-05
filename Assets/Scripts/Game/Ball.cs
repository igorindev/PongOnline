using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] float speed = 1;
    [SerializeField] float accelOnHit = 2;
    [SerializeField] int maxHitCounter = 6;
    int hitCounter = 0;
    Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        Launch();
    }

    public void Restart()
    {
        hitCounter = 0;
        transform.position = Vector3.zero;
        rb.velocity = Vector2.zero;
        Launch();
    }

    void Launch()
    {
        float x = Random.Range(0, 2) == 0 ? -1 : 1; 
        float y = Random.Range(0, 2) == 0 ? -1 : 1;

        rb.velocity = new Vector2(speed * x, speed * y);
    }

    // determine the response to a collision
    private void HandleRacketCollision(Collision2D col, int bounceDirectionX)
    {
        // keep track of how many times the ball hits a racket
        hitCounter++;

        if (hitCounter > maxHitCounter)
        {
            hitCounter = maxHitCounter;
        }

        // Calculate hit Factor
        float y = HitFactor(transform.position,
                            col.transform.position,
                            col.collider.bounds.size.y);

        // Calculate direction, make length=1 via .normalized
        Vector2 dir = new Vector2(bounceDirectionX, y).normalized;

        // Set Velocity with dir * speed, Add extra speed for each hit
        rb.velocity = dir * (speed + (hitCounter * accelOnHit));
    }

    float HitFactor(Vector2 ballPos, Vector2 racketPos, float racketHeight)
    {
        return (ballPos.y - racketPos.y) / racketHeight;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.name == "Player 1")
        { // Hit the left Racket?
            HandleRacketCollision(col, 1); // pass 1 for bouncing direction right
        }
        else if (col.gameObject.name == "Player 2")
        { // Hit the right Racket?
            HandleRacketCollision(col, -1); // pass -1 for bouncing direction left
        }
    }
}
