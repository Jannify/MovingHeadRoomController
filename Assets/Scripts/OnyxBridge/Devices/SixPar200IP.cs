using UnityEngine;
using Utils;

namespace OnyxBridge.Devices
{
    public class SixPar200IP : DMXLight
    {
        public override int NumChannels => 8;

        public override void SetData(byte[] newDmxData)
        {
            base.SetData(newDmxData);

            Color color = new Color(
                dmxData.ReadPercentage(0),
                dmxData.ReadPercentage(1),
                dmxData.ReadPercentage(2)
            );
            dmxLight.color = color;

            dmxLight.intensity = dmxData.ReadPercentage(6) * intensityMultiply;
        }
    }
}
