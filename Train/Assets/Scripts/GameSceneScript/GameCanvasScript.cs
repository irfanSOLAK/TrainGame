using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameCanvasScript : MonoBehaviour
{
    public void MainMenuSceneLoad()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
