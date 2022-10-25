using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RailRoadsScript : MonoBehaviour
{
    [Header("Railroad")]
    [SerializeField] private int railroadZAxisRotateAmount;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RotateRailroadWithTouch();
        }
    }

    private void RotateRailroadWithTouch()
    {
        Ray ray = FindTouchPositionWithRay();

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            RotateRailroad(hit);
        }
    }

    private Ray FindTouchPositionWithRay()
    {
        return Camera.main.ScreenPointToRay(Input.mousePosition);
    }

    private void RotateRailroad(RaycastHit hit)
    {
        GameObject touchedRailroad = FindTouchedRailroad(hit);
        Quaternion quaternion = CalculateRotationAngleOf(touchedRailroad);
        SetRotation(touchedRailroad, quaternion);
    }

    private GameObject FindTouchedRailroad(RaycastHit hit)
    {
        return GameObject.Find(hit.transform.name);
    }

    private Quaternion CalculateRotationAngleOf(GameObject touchedRailroad)
    {
        return Quaternion.Euler(0, 0, touchedRailroad.transform.rotation.eulerAngles.z - railroadZAxisRotateAmount);
    }

    private void SetRotation(GameObject touchedRailroad, Quaternion quaternion)
    {
        touchedRailroad.transform.rotation = quaternion;
    }
}