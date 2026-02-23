using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public void RestartGame()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }
    public void MainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }
    public void PlayGame()
    {
        SceneManager.LoadScene("PauseMenu");
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("App is Quitting");
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
