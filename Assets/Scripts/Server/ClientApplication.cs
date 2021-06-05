using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ClientApplication : MonoBehaviour
{
    UdpConnection MyConnection;

    public string ServerAddress;

    public int ServerPort = 3001;

    public Transform[] positions;

    void Start()
    {
        MyConnection = transform.GetChild(0).GetComponent<UdpConnection>();
        MyConnection.CreateSocket(UnityEngine.Random.Range(3000, 5000));

        MyConnection.OnPackageReceived += OnPackageReceived;

        StartCoroutine(Up());
    }

    void OnPackageReceived(string message, string address, int port)
    {
        message = message.Replace("Position=(", "");
        message = message.Replace(")", "");
        message = message.Replace(" ", "");

        string[] values = message.Split(',');

        Vector3 player1Pos = Vector3.zero;
        player1Pos.x = System.Convert.ToSingle(values[0]);
        player1Pos.y = System.Convert.ToSingle(values[1]);
        player1Pos.z = System.Convert.ToSingle(values[2]);

        Vector3 player2Pos = Vector3.zero;
        player2Pos.x = System.Convert.ToSingle(values[3]);
        player2Pos.y = System.Convert.ToSingle(values[4]);
        player2Pos.z = System.Convert.ToSingle(values[5]);

        Vector3 ball = Vector3.zero;
        ball.x = System.Convert.ToSingle(values[6]);
        ball.y = System.Convert.ToSingle(values[7]);
        ball.z = System.Convert.ToSingle(values[8]);

        MyConnection.SendPackage("Recebi seu pacote", address, port);
    }

    IEnumerator Up()
    {
        while (true)
        {
            int value = (int)Input.GetAxisRaw("Vertical");

            System.Globalization.CultureInfo culture = new System.Globalization.CultureInfo("en-US");
            string message = value.ToString(culture);

            MyConnection.SendPackage(message, ServerAddress, ServerPort);
            yield return new WaitForSeconds(0.01f);
        }
    }
}
