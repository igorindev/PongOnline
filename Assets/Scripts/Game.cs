using UnityEngine;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{
    public string IP { get; set; }

    public void Load(string name)
    {
        SceneManager.LoadScene(name);
    }

    public void LoadClient(string name)
    {
        PlayerPrefs.SetString("ip", IP);

        SceneManager.LoadScene(name);
    }
}
