using UnityEngine;
using TMPro;

public class Goal : MonoBehaviour
{
    [SerializeField] Ball ball;
    [SerializeField] TextMeshProUGUI pointsText;
    [SerializeField] int points;

    public int Points { get => points; set => points = value; }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ball"))
        {
            ball.Restart();
            Points += 1;
            pointsText.text = Points.ToString();
        }
    }

    public void UpdateValue()
    {
        pointsText.text = Points.ToString();
    }
}
