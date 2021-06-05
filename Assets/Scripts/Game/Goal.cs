using UnityEngine;
using TMPro;

public class Goal : MonoBehaviour
{
    [SerializeField] Ball ball;
    [SerializeField] TextMeshProUGUI pointsText;
    [SerializeField] int points;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ball"))
        {
            ball.Restart();
            points += 1;
            pointsText.text = points.ToString();
        }
    }
}
