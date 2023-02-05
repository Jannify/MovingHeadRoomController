using UnityEngine;

public abstract class DMXLight : DMXDevice
{
    [Header("Components")]
    [SerializeField] protected Light dmxLight;

    protected float intensityMultiply;

    private void Start()
    {
        intensityMultiply = dmxLight.intensity;
    }
}
