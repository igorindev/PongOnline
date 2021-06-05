using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ClientApplication : MonoBehaviour
{
    UdpConnection MyConnection;

    public string ServerAddress;

    public int ServerPort = 3000;

    // Start is called before the first frame update
    void Start()
    {
        MyConnection = transform.GetChild(0).GetComponent<UdpConnection>();
        MyConnection.CreateSocket(UnityEngine.Random.Range(3000,5000));

        MyConnection.OnPackageReceived += OnPackageReceived;
    }

    void OnPackageReceived(string message, string address, int port)
    {
        Debug.Log(message + " - Address: " + address + ":" + port.ToString());

        MyConnection.SendPackage("Recebi seu pacote", address, port);
    }

    // Update is called once per frame
    void OnGUI()
    {
        if (GUI.Button(new Rect(10, 10, 150, 100), "SEND MESSAGE"))
        {
            System.Globalization.CultureInfo culture = new System.Globalization.CultureInfo("en-US");
            string message = "Position=" + transform.position.ToString("0.00", culture);

            Debug.Log(message);

            MyConnection.SendPackage(message, ServerAddress, ServerPort);
        }
    }
}
