using System;
using System.Buffers.Binary;
using UnityEngine;
using Utils;

public class Entour_DMXMovingHead : DMXDevice
{
    public override int NumChannels => 15;

    [Header("Components")] [SerializeField]
    private Transform rootRotator;

    [SerializeField] private Transform panRotator;

    [SerializeField] private Transform tiltRotator;
    [SerializeField] private Light mhLight;

    [Header("Settings")] [SerializeField] private float panRange = 540;

    [SerializeField] private float tiltRange = 256;
    [SerializeField] private bool invertPan = false;
    [SerializeField] private float intensityMultiply = 3;

    [Header("Debug-Data")] [SerializeField, ReadOnlyField]
    private float panTarget;

    [SerializeField, ReadOnlyField] private float tiltTarget;
    [SerializeField, ReadOnlyField] private int panTargetDMX;
    [SerializeField, ReadOnlyField] private int tiltTargetDMX;

    private void Update()
    {
        panRotator.localRotation = Quaternion.Euler(0, 0, invertPan ? panTarget * -1 : panTarget);
        tiltRotator.localRotation = Quaternion.Euler(tiltTarget, 0, 0);

        panTargetDMX = (int) (Mathf.Abs((panTarget / panRange) - 0.5f) * 65535f);
        tiltTargetDMX = (int) (Mathf.Abs((tiltTarget / tiltRange) - 0.5f) * 65535f);
    }

    public override void SetData(byte[] newDmxData)
    {
        base.SetData(newDmxData);

        mhLight.intensity = dmxData[1] / 255f * intensityMultiply;
        SetColor(dmxData[2]);
        panTarget = (0.5f - dmxData.ReadExtendedPercentage(9)) * (invertPan ? -1 : 1) * panRange;
        tiltTarget = (0.5f - dmxData.ReadExtendedPercentage(11)) * tiltRange;
    }

    private void SetColor(byte colorIndex)
    {
        mhLight.color = colorIndex switch
        {
            < 11 => Color.white,
            < 22 => RGBToColor(248, 225, 173), // CTC
            < 33 => Color.yellow,
            < 44 => Color.cyan,
            < 55 => RGBToColor(244, 168, 216), // Pink
            < 66 => Color.green,
            < 77 => Color.blue,
            < 88 => Color.red,
            < 99 => Color.magenta,
            < 110 => RGBToColor(30, 173, 230), // Blue
            < 121 => RGBToColor(233, 158, 24), // Orange
            < 132 => RGBToColor(21, 124, 3), //DarkGreen
            < 143 => RGBToColor(146, 104, 214), // Purple
            _ => Color.white
        };
    }

    private static Color RGBToColor(int r, int g, int b) => new(r / 255f, g / 255f, b / 255f);

    public byte[] RotateToPoint(Vector3 point)
    {
        Vector3 delta = point - rootRotator.position;

        Vector3 panFrom = new(delta.x, 0, delta.z);
        panTarget = Vector3.SignedAngle(panFrom, rootRotator.up, Vector3.up);
        panRotator.localRotation = Quaternion.Euler(0, 0, panTarget);

        tiltTarget = Vector3.SignedAngle(delta, panRotator.forward, -panRotator.right);
        tiltRotator.localRotation = Quaternion.Euler(tiltTarget, 0, 0);

        float panPercentage = Mathf.Abs((panTarget / panRange) - 0.5f);
        float tiltPercentage = Mathf.Abs((tiltTarget / tiltRange) - 0.5f);

        dmxData.WriteExtendedPercentage(9, panPercentage);
        dmxData.WriteExtendedPercentage(11, tiltPercentage);

        return dmxData;
    }
}
