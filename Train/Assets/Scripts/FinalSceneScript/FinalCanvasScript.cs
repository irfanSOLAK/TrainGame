using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FinalCanvasScript : MonoBehaviour
{
    private int score;
    private int totalSpawnedTrain;
    private int totalCorrectMatch;
    private int maxReachedLevel;

    [Header("Text")]
    public Text scoreReportText;


    private void Awake()
    {
        GetPlayerStatistics();
    }

    // Start is called before the first frame update
    void Start()
    {
        WriteStatisticsToScoreReportText();
    }

    private void GetPlayerStatistics()
    {
        totalCorrectMatch = GetPlayerPrefsIntOf("CorrectMatchedStationNumber");
        totalSpawnedTrain = GetPlayerPrefsIntOf("TotalDestroyedTrainNumber");
        maxReachedLevel = GetPlayerPrefsIntOf("LevelNumber");
        CalculateScore();
    }

    private int GetPlayerPrefsIntOf(string tag)
    {
        int i;
        i = PlayerPrefs.GetInt(tag);
        return i;
    }
 
    private void CalculateScore()
    {
        score = totalCorrectMatch * 1400;
    }

    private void WriteStatisticsToScoreReportText()
    {
        scoreReportText.text = "Score " + 
            score.ToString() + 
            "\n Correct " + 
            totalCorrectMatch.ToString() + 
            " of " + 
            totalSpawnedTrain.ToString() + 
            "\n Level " + 
            maxReachedLevel.ToString();
    }

    public void MainMenuSceneLoad()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void Restart()
    {
        SceneManager.LoadScene("Game");
    }
}
