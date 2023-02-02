namespace Utils
{
    public static class DMXDataExtension
    {
        public static int ReadExtendedValue(this byte[] array, int startIndex)
        {
            return (array[startIndex] << 8) | array[startIndex + 1];
        }

        public static float ReadExtendedPercentage(this byte[] array, int startIndex)
        {
            return array.ReadExtendedValue(startIndex) / 65535f;
        }

        public static void WriteExtendedValue(this byte[] array, int startIndex, int value)
        {
            array[startIndex] = (byte) (value / 256);
            array[startIndex + 1] = (byte) (value % 256);
        }

        public static void WriteExtendedPercentage(this byte[] array, int startIndex, float value)
        {
            array.WriteExtendedValue(startIndex, (int) (value * 65535f));
        }
    }
}
