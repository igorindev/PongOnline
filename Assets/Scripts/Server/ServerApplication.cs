using UnityEngine;

public class ServerApplication : MonoBehaviour
{
    public int port = 3001;

    UdpConnection MyConnection;

    public PlayerMovement player;

    public Transform player1;
    public Transform player2;
    public Transform ball;

    // Start is called before the first frame update
    void Start()
    {
        MyConnection = transform.GetChild(0).GetComponent<UdpConnection>();
        MyConnection.CreateSocket(port);

        MyConnection.OnPackageReceived += OnPackageReceived;
    }

    void OnPackageReceived(string message, string address, int port)
    {
         int move = int.Parse(message);
        player.Movement = move;

        Debug.Log(message);

        System.Globalization.CultureInfo culture = new System.Globalization.CultureInfo("en-US");
        string pos = "Position=" + player1.position.ToString("0.00", culture);
        pos += "Position=" + player2.position.ToString("0.00", culture);
        pos += "Position=" + ball.position.ToString("0.00", culture);
        
        MyConnection.SendPackage(pos, address, port);
    }
}