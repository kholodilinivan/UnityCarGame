using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public string levelToLoad;
    public string mainMenu;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void StartGame()
    {
        SceneManager.LoadScene(levelToLoad);
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene(mainMenu);
        Time.timeScale = 1;
    }

    public void QuitGame()
    {
        Application.Quit();
        print("Game Qiuit");
    }
}
