using System;
using UnityEngine;
using Utils;

public abstract class MartinMacMovingHead : DMXDevice
{
    [Header("Components")]
    [SerializeField] private Transform rootRotator;
    [SerializeField] private Transform panRotator;
    [SerializeField] private Transform tiltRotator;
    [SerializeField] private Light mhLight;

    [Header("Settings")]
    [SerializeField] private float panRange = 540;
    [SerializeField] private float tiltRange = 257;
    [SerializeField] private bool invertPan;
    [SerializeField] private float intensityMultiply = 1;

    [Header("Debug-Data")]
    [SerializeField, ReadOnlyField] private float panTarget;
    [SerializeField, ReadOnlyField] private float tiltTarget;
    [SerializeField, ReadOnlyField] private int panTargetDMX;
    [SerializeField, ReadOnlyField] private int tiltTargetDMX;

    protected int IntCh;
    protected int ColCh;
    protected int FocCh;
    protected int PanCh;
    protected int TilCh;

    private void Update()
    {
        panRotator.localRotation = Quaternion.Euler(0, 0, invertPan ? panTarget * -1 : panTarget);
        tiltRotator.localRotation = Quaternion.Euler(tiltTarget, 0, 0);

        panTargetDMX = (int)(Mathf.Abs((panTarget / panRange) - 0.5f) * 65535f);
        tiltTargetDMX = (int)(Mathf.Abs((tiltTarget / tiltRange) - 0.5f) * 65535f);
    }

    public override void SetData(byte[] newDmxData)
    {
        base.SetData(newDmxData);

        mhLight.intensity = dmxData[IntCh] / 255f * intensityMultiply;
        SetColor(dmxData[ColCh]);
        panTarget = (0.5f - dmxData.ReadExtendedPercentage(PanCh)) * (invertPan ? -1 : 1) * panRange;
        tiltTarget = (0.5f - dmxData.ReadExtendedPercentage(TilCh)) * tiltRange;
    }

    private void SetColor(byte colorIndex)
    {
        mhLight.color = colorIndex switch
        {
            < 11 => new Color(0.8431f, 0.8862f, 1f), // Normal
            < 22 => new Color(0.9725f, 1f, 0.6784f), // CTB
            < 33 => new Color(0.93f, 0.88f, 0.29f),
            < 44 => new Color(0.12f, 0.31f, 0.44f),
            < 55 => new Color(1f, 0.75f, 0.8f),
            < 66 => new Color(0.59f, 0.8f, 0.66f),
            < 77 => new Color(0.23f, 0.28f, 0.46f),
            < 88 => new Color(0.8f, 0.19f, 0.12f),
            < 99 => new Color(0.56f, 0.31f, 0.49f),
            < 110 => new Color(0.22f, 0.32f, 0.53f),
            < 121 => new Color(1f, 0.65f, 0f),
            < 132 => new Color(0f, 0.39f, 0f),
            < 143 => new Color(0.65f, 0.4f, 0.92f),
            _ => new Color(0.8431f, 0.8862f, 1f), // Normal
        };
    }

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

        dmxData.WriteExtendedPercentage(PanCh, panPercentage);
        dmxData.WriteExtendedPercentage(TilCh, tiltPercentage);

        float distance = Vector3.Distance(mhLight.transform.position, point);

        int rawFocusValue = (int)((distance - 2f) * 14.17f);
        dmxData[FocCh] = (byte)Math.Clamp(rawFocusValue, 0, 255);

        return dmxData;
    }
}
