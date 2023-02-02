using UnityEngine;

public abstract class DMXDevice : MonoBehaviour
{
    [Header("Debug-Data")]
    [SerializeField, ReadOnlyField] protected byte[] dmxData;
    [SerializeField] public int universe;
    [SerializeField] public int startChannel;
    public abstract int NumChannels { get; }

    public virtual void SetData(byte[] newDmxData)
    {
        this.dmxData = newDmxData;
    }
}
