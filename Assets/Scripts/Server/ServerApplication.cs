using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerApplication : MonoBehaviour
{
    public int port = 3000;

    UdpConnection MyConnection;

    public Transform RemotePlayer;

    // Start is called before the first frame update
    void Start()
    {
        MyConnection = transform.GetChild(0).GetComponent<UdpConnection>();
        MyConnection.CreateSocket(port);

        MyConnection.OnPackageReceived += OnPackageReceived;
    }

    void OnPackageReceived(string message, string address, int port)
    {
        Debug.Log(message + " - Address: " + address + ":" + port.ToString());

        message = message.Replace("Position=(", "");
        message = message.Replace(")", "");
        message = message.Replace(" ", "");

        Debug.Log(message);

        string [] values = message.Split(',');

        float x = float.Parse(values[0]);
        float y = float.Parse(values[1]);
        float z = float.Parse(values[2]);

        RemotePlayer.position = new Vector3(x, y, z);

        MyConnection.SendPackage("Recebi seu pacote", address, port);
    }
}
