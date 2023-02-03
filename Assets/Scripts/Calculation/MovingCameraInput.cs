using System;
using Calculation;
using UnityEngine;
using UnityEngine.UI;

public class MovingCameraInput : BasicInputBehaviour
{
    public override string Name => "Mouse";

    [SerializeField] private float panMoveSpeed = 2f;
    [SerializeField] private float tiltMoveSpeed = 1f;
    [SerializeField] private float panAngle = 90f;
    [SerializeField] private float tiltAngle = 45f;
    [SerializeField] private Image cursorImage;

    private Camera mainCamera;
   private Transform cameraRotator;
    private float rotationX = 0;
    private float rotationY = 0;

    private void Start()
    {
        mainCamera = Camera.main;
        cameraRotator = mainCamera.transform.parent;
    }

    private void Update()
    {
        rotationX += -Input.GetAxis("Mouse Y") * tiltMoveSpeed;
        rotationX = Mathf.Clamp(rotationX, -tiltAngle, tiltAngle);
        rotationY += Input.GetAxis("Mouse X") * panMoveSpeed;
        rotationY = Mathf.Clamp(rotationY, -panAngle, panAngle);
        cameraRotator.localRotation = Quaternion.Euler(rotationX, rotationY, 0);

        if (Input.GetMouseButton(0))
        {
            Ray ray = mainCamera.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
            if (Physics.Raycast(ray, out RaycastHit hit, 200))
            {
                Room.MoveMovingHeads(hit.point);
            }
        }
    }

    private void OnEnable()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        cursorImage.enabled = true;
    }

    private void OnDisable()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        cursorImage.enabled = false;
        cameraRotator.localRotation = Quaternion.identity;
    }
}
