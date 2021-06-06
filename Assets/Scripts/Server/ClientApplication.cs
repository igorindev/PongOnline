using System.Collections;
using UnityEngine;

public class ClientApplication : MonoBehaviour
{
    UdpConnection MyConnection;

    public string ServerAddress;

    public int ServerPort = 3001;

    public Transform[] positions;

    float lastPackTime = 0;

    Coroutine current;

    [SerializeField] Goal red;
    [SerializeField] Goal green;

    void Start()
    {
        ServerAddress = PlayerPrefs.GetString("ip");

        MyConnection = transform.GetChild(0).GetComponent<UdpConnection>();
        MyConnection.CreateSocket(UnityEngine.Random.Range(3000, 5000));

        MyConnection.OnPackageReceived += OnPackageReceived;

        StartCoroutine(Up());
    }

    void OnPackageReceived(string message, string address, int port)
    {
        if (current != null)
        {
            StopCoroutine(current);
        }

        message = message.Replace("Position=(", "");
        message = message.Replace(")", "");
        message = message.Replace(" ", "");

        string[] values = message.Split(',');

        System.Globalization.CultureInfo culture = new System.Globalization.CultureInfo("en-US");
        Vector3 player1Pos = Vector3.zero;
        player1Pos.x = System.Convert.ToSingle(values[0], culture);
        player1Pos.y = System.Convert.ToSingle(values[1], culture);
        player1Pos.z = System.Convert.ToSingle(values[2], culture);

        Vector3 player2Pos = Vector3.zero;
        player2Pos.x = System.Convert.ToSingle(values[3], culture);
        player2Pos.y = System.Convert.ToSingle(values[4], culture);
        player2Pos.z = System.Convert.ToSingle(values[5], culture);

        Vector3 ball = Vector3.zero;
        ball.x = System.Convert.ToSingle(values[6], culture);
        ball.y = System.Convert.ToSingle(values[7], culture);
        ball.z = System.Convert.ToSingle(values[8], culture);

        float time = System.Convert.ToSingle(values[9], culture);

        red.Points = int.Parse(values[10], culture);
        red.UpdateValue();
        green.Points = int.Parse(values[11], culture);
        green.UpdateValue();
        current = StartCoroutine(Interpolate(player1Pos, player2Pos, ball, time));
    }

    IEnumerator Up()
    {
        while (true)
        {
            int value = (int)Input.GetAxisRaw("Vertical");

            System.Globalization.CultureInfo culture = new System.Globalization.CultureInfo("en-US");
            string message = value.ToString(culture);

            MyConnection.SendPackage(message, ServerAddress, ServerPort);
            yield return null;
        }
    }

    IEnumerator Interpolate(Vector3 player1, Vector3 player2, Vector3 ball, float time)
    {
        time -= lastPackTime;

        Vector3 player1Current = positions[0].position;
        Vector3 player2Current = positions[1].position;
        Vector3 ballCurrent = positions[2].position;

        float count = 0;
        while (count < 1)
        {
            count += Time.deltaTime * time;

            positions[0].position = Vector3.Lerp(player1Current, player1, count);
            positions[1].position = Vector3.Lerp(player2Current, player2, count);
            positions[2].position = Vector3.Lerp(ballCurrent, ball, count);

            yield return null;
        }
        positions[0].position = player1;
        positions[1].position = player2;
        positions[2].position = ball;

        lastPackTime = time;
    }
}
