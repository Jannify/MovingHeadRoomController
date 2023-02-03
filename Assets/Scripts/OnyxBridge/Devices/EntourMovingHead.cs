namespace OnyxBridge.Devices
{
    public class EntourMovingHead : MartinMacMovingHead
    {
        private void Awake()
        {
            IntCh = 1;
            ColCh = 2;
            FocCh = 7;
            PanCh = 9;
            TilCh = 11;
        }
    }
}
