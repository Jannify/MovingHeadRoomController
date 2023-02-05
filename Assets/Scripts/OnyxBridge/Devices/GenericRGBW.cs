using UnityEngine;
using Utils;

namespace OnyxBridge.Devices
{
    public class GenericRGBW : DMXLight
    {
        public override int NumChannels => 4;

        public override void SetData(byte[] newDmxData)
        {
            base.SetData(newDmxData);

            Color colorRed = new Color(dmxData.ReadPercentage(0), 0, 0);
            Color colorGreen = new Color(0, dmxData.ReadPercentage(1), 0);
            Color colorBlue = new Color(0, 0, dmxData.ReadPercentage(2));
            Color colorWhite = Color.white;
            colorWhite.a = dmxData.ReadPercentage(3);

            dmxLight.color = colorRed + colorGreen + colorBlue + colorWhite;
        }
    }
}
