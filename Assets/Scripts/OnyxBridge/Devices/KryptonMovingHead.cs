namespace OnyxBridge.Devices
{
    public class KryptonMovingHead : MartinMacMovingHead
    {
        public override int NumChannels => 14;

        private void Awake()
        {
            IntCh = 1;
            ColCh = 2;
            FocCh = 6;
            PanCh = 8;
            TilCh = 10;
        }
    }
}
