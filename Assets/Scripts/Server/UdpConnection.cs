using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;
using System.Text;
using System;
using System.Threading;
using System.Collections.Concurrent;

public class UdpConnection : MonoBehaviour
{
    public struct PackageData
    {
        public string message;

        public string address;

        public int port;
    };

    UdpClient socket;

    ConcurrentQueue<PackageData> PackageQueue;

    public delegate void OnPackageReceived_Delegate(string message, string address, int port);

    public OnPackageReceived_Delegate OnPackageReceived;

    // Use this for initialization
    public void CreateSocket(int port)
    {
        PackageQueue = new ConcurrentQueue<PackageData>();

        IPEndPoint serverIPPort = new IPEndPoint(IPAddress.Parse(GetLocalIP()), port);

        socket = new UdpClient(port);

        socket.BeginReceive(new AsyncCallback(OnReceivePackage), socket);
    }

    public void SendPackage(string message, string dstAddress, int dstPort)
    {
        IPEndPoint serverIPPort = new IPEndPoint(IPAddress.Parse(dstAddress), dstPort);

        byte[] bytes = Encoding.UTF8.GetBytes(message);

        socket.BeginSend(bytes, bytes.Length, serverIPPort, new AsyncCallback(OnPackageSent), socket);
    }

    string GetLocalIP()
    {
        string hostName = Dns.GetHostName();

        IPAddress[] addressList = Dns.GetHostEntry(hostName).AddressList;
        for (int i = 0; i < addressList.Length; i++)
        {
            if (addressList[i].AddressFamily == AddressFamily.InterNetwork) // IPV4
                return addressList[i].ToString();
        }

        return "";
    }

    void OnPackageSent(IAsyncResult result)
    {
        UdpClient socket = result.AsyncState as UdpClient;

        int bytesSent = socket.EndSend(result);

        if (bytesSent == 0)
            Debug.Log("Error");
    }

    void OnReceivePackage(IAsyncResult result)
    {
        UdpClient socket = result.AsyncState as UdpClient;
        IPEndPoint source = new IPEndPoint(0, 0);

        byte[] buffer = socket.EndReceive(result, ref source);

        if (buffer.Length > 0)
        {
            string messageTxt = Encoding.UTF8.GetString(buffer, 0, buffer.Length);

            PackageData package = new PackageData();
            package.message = messageTxt;
            package.address = source.Address.ToString();
            package.port = source.Port;

            PackageQueue.Enqueue(package);
        }

        socket.BeginReceive(new AsyncCallback(OnReceivePackage), socket);
    }

    void Update()
    {
        if (PackageQueue.Count > 0)
        {
            PackageData package;
            if (PackageQueue.TryDequeue(out package))
            {
                if (OnPackageReceived != null)
                    OnPackageReceived(package.message, package.address, package.port);
            }

        }
    }
}