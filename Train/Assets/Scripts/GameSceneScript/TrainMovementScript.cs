using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainMovementScript : MonoBehaviour
{
    [Header("GameObjects")]
    private GameObject pathsGameObject;
    private Transform[] pathsChidrenArrays;

    [Header("Index")]
    private int pathsChidrenArraysIndex;
    private int dynamicRailroadNumber;

    [Header("Speed")]
    private float trainMovementSpeed;

    [Header("Functions")]
    Func<float, int, bool> equal = (x, y) => x == y;
    Func<float, int, bool> notEqual = (x, y) => x != y;

    private SpawnPointScript spawnPointScript;

    // Start is called before the first frame update
    void Start()
    {
        FindPathsGameObject();
        GetPathsChildrenTransform();
        SetPathsChildrenArraysIndexTo(1);
        SetDynamicRailroadNumberTo(0);
        SetTrainMovementSpeedTo(2);
        GetSpawnPointScript();
    }
    private void FindPathsGameObject()
    {
        pathsGameObject = GameObject.FindGameObjectWithTag("Paths");
    }
    private void GetPathsChildrenTransform()
    {
        pathsChidrenArrays = pathsGameObject.GetComponentsInChildren<Transform>();
    }
    private void SetPathsChildrenArraysIndexTo(int a)
    {
        pathsChidrenArraysIndex = a;
    }
    private void SetDynamicRailroadNumberTo(int a)
    {
        dynamicRailroadNumber = a;
    }
    private void SetTrainMovementSpeedTo(float a)
    {
        trainMovementSpeed = a;
    }
    private void GetSpawnPointScript()
    {
        spawnPointScript = GameObject.FindGameObjectWithTag("SpawnPoint").GetComponent<SpawnPointScript>();
    }


    // Update is called once per frame
    void Update()
    {
        MoveToTarget(pathsChidrenArrays[pathsChidrenArraysIndex]);
        DecideNextIfMoveCompleted();
    }

    private void MoveToTarget(Transform target)
    {
        transform.position = Vector2.MoveTowards(transform.position, target.transform.position, trainMovementSpeed * Time.deltaTime);
    }

    private void DecideNextIfMoveCompleted()
    {
        if (transform.position == pathsChidrenArrays[pathsChidrenArraysIndex].transform.position)
        {
            DecideNext();
        }
    }

    private void DecideNext()
    {
        if (pathsChidrenArrays[pathsChidrenArraysIndex].CompareTag("Untagged"))
        {
            SetPathsChildrenArraysIndexTo(pathsChidrenArraysIndex + 1);
            FindNextPath();
        }
        else
        {
            TrainInsideAStation();
        }
    }

    private void TrainInsideAStation()
    {
        SetTrainMovementSpeedTo(0);
        CheckStation();

        spawnPointScript.totalDestroyedTrainNumber++;

        spawnPointScript.CanvasTextNamedWrite("CorrectText",
            "Correct: " +
            spawnPointScript.correctMatchedStationNumber.ToString() +
            " of " +
            spawnPointScript.totalDestroyedTrainNumber.ToString());

        Destroy(gameObject);
    }

    private void CheckStation()
    {
        if (CompareTag(pathsChidrenArrays[pathsChidrenArraysIndex].tag))
        {
            spawnPointScript.correctMatchedStationNumber++;
        }
    }

    private void FindNextPath()
    {
        if (GameObject.Find(dynamicRailroadNumber.ToString()) != null)
        {
            if (transform.position == GameObject.Find(dynamicRailroadNumber.ToString()).transform.position)
            {
                // The following numbers are next path's indexes
                //  SetRailroadOrPath(firstValueIsDynamicRailRoad, 1, 13) means increase dynamicRailroadNumber 1 if the condition is met
                // increase 13 pathsChidrenArraysIndex otherwise

                if (dynamicRailroadNumber == 0)
                {
                    SetRailroadOrPath(equal, 1, 13,6);
                }
                else if (1 <= dynamicRailroadNumber && dynamicRailroadNumber <= 3)
                {
                    SetRailroadOrPath(equal, 1, 6);
                }

                else if (4 <= dynamicRailroadNumber && dynamicRailroadNumber <= 5)
                {
                    SetRailroadOrPath(notEqual, 1, 6);
                }

                else if (dynamicRailroadNumber == 6)
                {
                    SetRailroadOrPath(notEqual, 1, 5);
                }

                else if (7 <= dynamicRailroadNumber && dynamicRailroadNumber <= 8)
                {
                    SetRailroadOrPath(equal, 1, 4);
                }
            }
        }
    }

    private void SetRailroadOrPath(Func<float, int, bool> equalOrNotEqual, int railRoadNumber, int pathIndex)
    {
        if (equalOrNotEqual(GameObject.Find(dynamicRailroadNumber.ToString()).transform.rotation.eulerAngles.z % 180, 0))
        {
            SetDynamicRailroadNumberTo(dynamicRailroadNumber + railRoadNumber);
        }
        else
        {
            SetPathsChildrenArraysIndexTo(pathsChidrenArraysIndex + pathIndex);
        }
    }

    private void SetRailroadOrPath(Func<float, int, bool> equalOrNotEqual, int railRoadNumber, int pathIndex, int railRoadNumber2)
    {
        if (equalOrNotEqual(GameObject.Find(dynamicRailroadNumber.ToString()).transform.rotation.eulerAngles.z % 180, 0))
        {
            SetDynamicRailroadNumberTo(dynamicRailroadNumber + railRoadNumber);
        }
        else
        {
            SetPathsChildrenArraysIndexTo(pathsChidrenArraysIndex + pathIndex);
            SetDynamicRailroadNumberTo(dynamicRailroadNumber + railRoadNumber2);
        }
    }
}