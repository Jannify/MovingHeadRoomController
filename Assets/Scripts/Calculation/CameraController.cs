using System;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            ShootRayAndUpdateMH();
        }
    }

    private void ShootRayAndUpdateMH()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 200))
        {
            Debug.Log(hit.transform.name);
            Debug.Log("hit");

            Room.MoveMovingHeads(hit.point);
        }

    }
}
