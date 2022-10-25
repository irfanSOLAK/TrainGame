using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SpawnPointScript : MonoBehaviour
{

    [Header("GameObjects")]
    public GameObject[] trainsPrefabs;
    public GameObject[] stations;

    [Header("Game Statistics")]
    public int totalDestroyedTrainNumber;
    public int correctMatchedStationNumber;
    private int levelNumber;
    private int totalSpawnedTrainNumber;

    [Header("Time")]
    [SerializeField] private float trainSpawnTimeInSeconds;
    [SerializeField] private float remainingPlayTimeInSecond;
    private float spawnTimeCounter;

    [Header("Spawn")]
    private bool isSpawning;
    private int randomTrainInstantiateNumber;
    private int tempInstantiateNumber;
    private Color stationsColor;
     

    private void Awake()
    {
        totalDestroyedTrainNumber = 0;
        totalSpawnedTrainNumber = 0;
        correctMatchedStationNumber = 0;
        tempInstantiateNumber = 0;
        SetIsSpawningTo(false);
        randomTrainInstantiateNumber = 2;
        levelNumber = 1;
        spawnTimeCounter = 0;
    }

    private void SetIsSpawningTo(bool spawn)
    {
        isSpawning = spawn;
    }

    // Start is called before the first frame update
    void Start()
    {
        WriteRemainingTimeInCorrectFormat();
        CanvasTextNamedWrite("StartCountDownText", "level:  " + levelNumber.ToString());
        ActivateStationsWithInstantiateNumber();
        StartCoroutine(CountDownToPlay());
    }

    private void WriteRemainingTimeInCorrectFormat()
    {
        var ts = TimeSpan.FromSeconds(remainingPlayTimeInSecond);
        CanvasTextNamedWrite("RemainingTimeText", "Time " + string.Format("{0:00}:{1:00}", ts.Minutes, ts.Seconds));
    }

    public void CanvasTextNamedWrite(string textNameInCanvas, string textValue)
    {
        GameObject.FindGameObjectWithTag("Canvas").transform.Find(textNameInCanvas).GetComponent<Text>().text = textValue;
    }

    private void ActivateStationsWithInstantiateNumber()
    {
        for (int i = 0; i < randomTrainInstantiateNumber; i++)
        {
            stationsColor = stations[i].GetComponent<SpriteRenderer>().color;
            stations[i].GetComponent<SpriteRenderer>().color = new Color(stationsColor.r, stationsColor.g, stationsColor.b, 255);
        }
    }

    // Update is called once per frame
    void Update()
    {
        SpawnTrainInTime();
        CheckIfGameEnd();
        CheckLevelUp();
    }

    private void CheckIfGameEnd()
    {
        if (totalDestroyedTrainNumber == totalSpawnedTrainNumber && remainingPlayTimeInSecond <= 0)
        {
            StartCoroutine(ShowStatistics());
        }
    }

    private void SpawnTrainInTime()
    {
        if (isSpawning)
        {
            spawnTimeCounter += Time.deltaTime;

            if (spawnTimeCounter >= trainSpawnTimeInSeconds)
            {
                SpawnTrain();
                spawnTimeCounter = 0;
            }
        }
    }

    private void SpawnTrain()
    {
        Instantiate(trainsPrefabs[UnityEngine.Random.Range(0, randomTrainInstantiateNumber)], transform.position, Quaternion.identity);
        totalSpawnedTrainNumber++;
    }

    private void CheckLevelUp()
    {
        if (correctMatchedStationNumber % 2 == 0)
        {
            LevelUp();
        }
    }

    void LevelUp()
    {
        if (randomTrainInstantiateNumber < 10 && tempInstantiateNumber < correctMatchedStationNumber)  
        {
            randomTrainInstantiateNumber += 2;
            tempInstantiateNumber = randomTrainInstantiateNumber;
            levelNumber++;

            CanvasTextNamedWrite("LevelText", "Level : " + levelNumber.ToString());
            ActivateStationsWithInstantiateNumber();
        }
    }

    IEnumerator CountDownToPlay()
    {
        CanvasTextNamedWrite("StartCountDownText", "3");
        yield return new WaitForSeconds(1f);
        CanvasTextNamedWrite("StartCountDownText", "2");
        yield return new WaitForSeconds(1f);
        CanvasTextNamedWrite("StartCountDownText", "1");
        yield return new WaitForSeconds(1f);
        CanvasTextNamedWrite("StartCountDownText", "");

        SetIsSpawningTo(true);
        StartCoroutine(ControlRemainingPlayTime());
    }

    IEnumerator ControlRemainingPlayTime()
    {
        while (remainingPlayTimeInSecond > 0)
        {
            CalculateRemainingPlayTime();
            yield return new WaitForSeconds(1f);
        }

        SetIsSpawningTo(false);
        SetRemainingPlayTimeInSecond(0);
    }

    private void CalculateRemainingPlayTime()
    {
        SetRemainingPlayTimeInSecond(remainingPlayTimeInSecond - 1);
        WriteRemainingTimeInCorrectFormat();
    }

    private void SetRemainingPlayTimeInSecond(float remaininig)
    {
        remainingPlayTimeInSecond = remaininig;
    }

    IEnumerator ShowStatistics()
    {
        SaveStatistics();
        yield return new WaitForSeconds(1f);
        LoadResultsScene();
    }

    private static void LoadResultsScene()
    {
        SceneManager.LoadScene("Results");
    }

    private void SaveStatistics()
    {
        PlayerPrefs.SetInt("CorrectMatchedStationNumber", correctMatchedStationNumber);
        PlayerPrefs.SetInt("TotalDestroyedTrainNumber", totalDestroyedTrainNumber);
        PlayerPrefs.SetInt("LevelNumber", levelNumber);
    }
}