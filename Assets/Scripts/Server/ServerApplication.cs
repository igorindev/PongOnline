using System.Collections;
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
                message += "," + Time.time;
                message += "," + redPoints;
                message += "," + greenPoints;

                MyConnection.SendPackage(message, connectedAddress, connectedPort);
            }

            yield return new WaitForSeconds(0.1f);
        }
    }
}