using UnityEngine;

[RequireComponent(typeof(Light))]
public class RGB_DMXLight : DMXDevice
{
    [Header("DMX Addresses")]
    [SerializeField] private int RedAddress = 0;
    [SerializeField] private int GreenAddress = 1;
    [SerializeField] private int BlueAddress = 2;

    [Header("Components")]
    [SerializeField] private Light dmxLight;

    public override int NumChannels => 3;

    public override void SetData(byte[] dmxData)
    {
        base.SetData(dmxData);

        Color color = dmxLight.color;

        color.r = dmxData[RedAddress] / 256f;
        color.g = dmxData[GreenAddress] / 256f;
        color.b = dmxData[BlueAddress] / 256f;

        dmxLight.color = color;
    }

    private void OnValidate()
    {
        dmxLight = GetComponent<Light>();
    }
}
