using System.Collections;
using System.Globalization;
using UnityEngine;

public class ServerApplication : MonoBehaviour
{
    public int port = 3001;
    public string connectedAddress = "";
    public int connectedPort = 0;
    UdpConnection MyConnection;

    public PlayerMovement secondPlayer;
    public Ball ballMain;

    public Goal redPoints;
    public Goal greenPoints;

    public Transform player1;
    public Transform player2;
    public Transform ball;

    public float timer;

    void Awake()
    {
        timer = 0;
        port = int.Parse(PlayerPrefs.GetString("port"));
    }

    void Start()
    {
        MyConnection = transform.GetChild(0).GetComponent<UdpConnection>();
        MyConnection.CreateSocket(port);

        MyConnection.OnPackageReceived += OnPackageReceived;

        StartCoroutine(Up());
    }

    void OnPackageReceived(string message, string address, int port)
    {
        connectedPort = port;
        connectedAddress = address;
        int.TryParse(message, out int move);
        secondPlayer.Movement = move;
    }

    private void Update()
    {
        if (connectedAddress != "")
        {
            timer += Time.deltaTime;
        }
    }

    IEnumerator Up()
    {
        while (true)
        {
            if (connectedAddress != "")
            {
                ballMain.enabled = true;
                System.Globalization.CultureInfo culture = new System.Globalization.CultureInfo("en-US");
                string message = "Position=" + player1.position.ToString("0.00", culture);
                message += ",Position=" + player2.position.ToString("0.00", culture);
                message += ",Position=" + ball.position.ToString("0.00", culture);
                message += "," + timer.ToString("0.00", culture);
                message += "," + redPoints.Points;
                message += "," + greenPoints.Points;

                MyConnection.SendPackage(message, connectedAddress, connectedPort);
            }

            yield return null;
        }
    }
}