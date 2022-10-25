using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class MainMenuCanvasScript : MonoBehaviour
{ 
    public Text gameInfoText;
    public void GameSceneLoad()
    {
        SceneManager.LoadScene("Game");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void GameInfo()
    {
        gameInfoText.text = "Rotate highlighted railroads to match trains with the same color station.";
    }

    public void EmptyText()
    {
        gameInfoText.text = "";
    }
}
