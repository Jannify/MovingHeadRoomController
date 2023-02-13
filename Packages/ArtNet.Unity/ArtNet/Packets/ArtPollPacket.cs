using System;
using ArtNet.Enums;
using ArtNet.IO;

namespace ArtNet.Packets
{
    [Flags]
    public enum ArtPollFlags : byte
    {
        DEFAULT = 0b00110000,
        SEND_REPLY_EVERYTIME = 0b00000010,
        DIAGNOSTIC_MESSAGES = 0b00000100,
        DIAGNOSTIC_MESSAGES_UNICAST = 0b00001000,
        VLC_TRANSMISSION = 0b00010000,
        TARGET_MODE = 0b00100000
    }

    public class ArtPollPacket : ArtNetPacket
    {
        public ArtPollPacket(ArtPollFlags flags)
            : base(ArtNetOpCodes.Poll)
        {
            Flags = flags;
        }

        public ArtPollPacket(ArtNetRecieveData data)
            : base(data)
        {

        }

        #region Packet Properties

        private ArtPollFlags flags = 0;

        public ArtPollFlags Flags
        {
            get => flags;
            set => flags = value;
        }

        #endregion

        public override void ReadData(ArtNetBinaryReader data)
        {
            base.ReadData(data);

            Flags = (ArtPollFlags) data.ReadByte();
        }

        public override void WriteData(ArtNetBinaryWriter data)
        {
            base.WriteData(data);

            data.Write((byte)Flags);
            data.Write((byte)0);    // DiagPriority
            data.Write((short)0);   // TargetPortAddressTop
            data.Write((short)0);   // TargetPortAddressBottom
        }

    }
}
