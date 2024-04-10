using Framework;
using System.Collections; 
using UnityEngine;
using UnityEngine.SceneManagement;  

public class RestartScene : MonoBehaviour
{

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Backslash) || Input.GetKeyDown(KeyCode.Backspace))
        {
            Restart();
        }
    }

    public static void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1.0f;
    }
}
