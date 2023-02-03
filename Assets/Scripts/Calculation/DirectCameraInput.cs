using Calculation;
using UnityEngine;

public class DirectCameraInput : BasicInputBehaviour
{
    public override string Name => "Cursor";

    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 200))
            {
                Room.MoveMovingHeads(hit.point);
            }
        }
    }
}
