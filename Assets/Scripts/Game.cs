using UnityEngine;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{
    public string IP { get; set; }
    public string Port { get; set; }

    public void Load(string name)
    {
        PlayerPrefs.SetString("port", Port);

        SceneManager.LoadScene(name);
    }

    public void LoadClient(string name)
    {
        PlayerPrefs.SetString("ip", IP);
        PlayerPrefs.SetString("port", Port);

        SceneManager.LoadScene(name);
    }
}