using ArtNet.Enums;
using ArtNet.IO;
using ArtNet.Sockets;
using System;

namespace ArtNet.Packets
{
    [Flags]
    public enum PollReplyStatus : byte
    {
        UBEA_SUPPORT = 0b00000001,
        RDM_CAPABLE = 0b00000010,
        ROM_BOOT = 0b00000100,

        PORTADDRESS_FRONTPANNEL = 0b00010000,
        PORTADDRESS_NETWORK = 0b00100000,

        FRONTPANNEL_LOCATE= 0b01000000,
        FRONTPANNEL_MUTE= 0b10000000,
        FRONTPANNEL_NORMAL= 0b11000000,
    }

    [Flags]
    public enum PollReplyStatus2 : byte
    {
        WEB_SITE_AVAILABLE = 0b00000001,
        DHCP = 0b00000010,
        DHCP_CAPABLE = 0b00000100,
        ART_NET_3_PORTS = 0b00001000
    }

    public enum PollReplyNodeReport : byte
    {
        RcDebug = 0x0000,
        RcPowerOk = 0x0001,
        RcPowerFail = 0x0002,
        RcSocketWr1 = 0x0003,
        RcParseFail = 0x0004,
        RcUdpFail = 0x0005,
        RcShNameOk = 0x0006,
        RcLoNameOk = 0x0007,
        RcDmxError = 0x0008,
        RcDmxUdpFull = 0x0009,
        RcDmxRxFull = 0x000a,
        RcSwitchErr = 0x000b,
        RcConfigErr = 0x000c,
        RcDmxShort = 0x000d,
        RcFirmwareFail = 0x000e,
        RcUserFail = 0x000f,
        RcFactoryRes = 0x0010
    }

    public class ArtPollReplyPacket : ArtNetPacket
    {
        public ArtPollReplyPacket()
            : base(ArtNetOpCodes.PollReply)
        {
        }

        public ArtPollReplyPacket(ArtNetRecieveData data)
            : base(data)
        {

        }

        #region Packet Properties

        private byte[] ipAddress = new byte[4];

        public byte[] IpAddress
        {
            get => ipAddress;
            set
            {
                if (value.Length != 4)
                    throw new ArgumentException("The IP address must be an array of 4 bytes.");

                ipAddress = value;
            }
        }

        private ushort port = ArtNetSocket.Port;

        public ushort Port
        {
            get => port;
            set => port = value;
        }

        private ushort firmwareVersion = 0;

        public ushort FirmwareVersion
        {
            get => firmwareVersion;
            set => firmwareVersion = value;
        }

        private byte netSwitch = 0;

        public byte NetSwitch
        {
            get => netSwitch;
            set => netSwitch = value;
        }

        private byte subSwitch = 0;

        public byte SubSwitch
        {
            get => subSwitch;
            set => subSwitch = value;
        }

        private ushort oem = 0xff;

        public ushort Oem
        {
            get => oem;
            set => oem = value;
        }

        private byte ubeaVersion = 0;

        public byte UbeaVersion
        {
            get => ubeaVersion;
            set => ubeaVersion = value;
        }

        private PollReplyStatus status = 0;

        public PollReplyStatus Status
        {
            get => status;
            set => status = value;
        }

        private short estaCode = 0;

        public short EstaCode
        {
            get => estaCode;
            set => estaCode = value;
        }

        private string shortName = string.Empty;

        public string ShortName
        {
            get => shortName;
            set => shortName = value;
        }

        private string longName = string.Empty;

        public string LongName
        {
            get => longName;
            set => longName = value;
        }

        private string nodeReport = string.Empty;

        public string NodeReport
        {
            get => nodeReport;
            set => nodeReport = value;
        }

        private static int nodeReportIncrement = 0;

        private ushort portCount = 0;

        public ushort PortCount
        {
            get => portCount;
            set => portCount = value;
        }

        private byte[] portTypes = new byte[4];

        public byte[] PortTypes
        {
            get => portTypes;
            set
            {
                if (value.Length != 4)
                    throw new ArgumentException("The port types must be an array of 4 bytes.");

                portTypes = value;
            }
        }

        private byte[] goodInput = new byte[4];

        public byte[] GoodInput
        {
            get => goodInput;
            set
            {
                if (value.Length != 4)
                    throw new ArgumentException("The good input must be an array of 4 bytes.");

                goodInput = value;
            }
        }

        private byte[] goodOutput = new byte[4];

        public byte[] GoodOutput
        {
            get => goodOutput;
            set
            {
                if (value.Length != 4)
                    throw new ArgumentException("The good output must be an array of 4 bytes.");

                goodOutput = value;
            }
        }

        private byte[] swIn = new byte[4];

        public byte[] SwIn
        {
            get => swIn;
            set => swIn = value;
        }

        private byte[] swOut = new byte[4];

        public byte[] SwOut
        {
            get => swOut;
            set => swOut = value;
        }

        private byte swVideo = 0;

        public byte SwVideo
        {
            get => swVideo;
            set => swVideo = value;
        }

        private byte swMacro = 0;

        public byte SwMacro
        {
            get => swMacro;
            set => swMacro = value;
        }

        private byte swRemote = 0;

        public byte SwRemote
        {
            get => swRemote;
            set => swRemote = value;
        }

        private byte style = 0;

        public byte Style
        {
            get => style;
            set => style = value;
        }

        private byte[] macAddress = new byte[6];

        public byte[] MacAddress
        {
            get => macAddress;
            set
            {
                if (value.Length != 6)
                    throw new ArgumentException("The mac address must be an array of 6 bytes.");

                macAddress = value;
            }
        }

        private byte[] bindIpAddress = new byte[4];

        public byte[] BindIpAddress
        {
            get => bindIpAddress;
            set
            {
                if (value.Length != 4)
                    throw new ArgumentException("The bind IP address must be an array of 4 bytes.");

                bindIpAddress = value;
            }
        }

        private byte bindIndex = 0;

        public byte BindIndex
        {
            get => bindIndex;
            set => bindIndex = value;
        }

        private PollReplyStatus2 status2 = 0;

        public PollReplyStatus2 Status2
        {
            get => status2;
            set => status2 = value;
        }


        #endregion

        #region Packet Helpers

        /// <summary>
        /// Interprets the universe address to ensure compatibility with ArtNet I, II and III devices.
        /// </summary>
        /// <param name="outPorts">Whether to get the address for in or out ports.</param>
        /// <param name="portIndex">The port index to obtain the universe for.</param>
        /// <returns>The 15 Bit universe address</returns>
        public int UniverseAddress(bool outPorts, int portIndex)
        {
            int universe;

            if (SubSwitch > 0)
            {
                universe = (SubSwitch & 0x7F00);
                universe += (SubSwitch & 0x0F) << 4;
                universe += (outPorts ? SwOut[portIndex] : SwIn[portIndex]) & 0xF;
            }
            else
            {
                universe = (outPorts ? SwOut[portIndex] : SwIn[portIndex]);
            }

            return universe;
        }

        public static string GetNodeReport(PollReplyNodeReport report, string message)
        {
            if (nodeReportIncrement > 9999)
            {
                nodeReportIncrement -= 9999;
            }

            return $"#{((byte)report):X4} [{nodeReportIncrement++:0000}] {message}";
        }

        #endregion

        public override void ReadData(ArtNetBinaryReader data)
        {
            base.ReadData(data);

            IpAddress = data.ReadBytes(4);
            Port = data.ReadUInt16();
            FirmwareVersion = data.ReadNetworkU16();
            NetSwitch = data.ReadByte();
            SubSwitch = data.ReadByte();
            Oem = data.ReadNetworkU16();
            UbeaVersion = data.ReadByte();
            Status = (PollReplyStatus)data.ReadByte();
            EstaCode = data.ReadNetwork16();
            ShortName = data.ReadNetworkString(18);
            LongName = data.ReadNetworkString(64);
            NodeReport = data.ReadNetworkString(64);
            PortCount = data.ReadNetworkU16();
            PortTypes = data.ReadBytes(4);
            GoodInput = data.ReadBytes(4);
            GoodOutput = data.ReadBytes(4);
            SwIn = data.ReadBytes(4);
            SwOut = data.ReadBytes(4);
            SwVideo = data.ReadByte();
            SwMacro = data.ReadByte();
            SwRemote = data.ReadByte();
            data.ReadBytes(3);
            Style = data.ReadByte();
            MacAddress = data.ReadBytes(6);
            BindIpAddress = data.ReadBytes(4);
            BindIndex = data.ReadByte();
            Status2 = (PollReplyStatus2)data.ReadByte();
        }

        public override void WriteData(ArtNetBinaryWriter data)
        {
            base.WriteData(data);

            data.Write(IpAddress);
            data.Write(Port);
            data.WriteNetwork(FirmwareVersion);
            data.Write(NetSwitch);
            data.Write(SubSwitch);
            data.WriteNetwork(Oem);
            data.Write(UbeaVersion);
            data.Write((byte)Status);
            data.Write(EstaCode);
            data.WriteNetwork(ShortName, 18);
            data.WriteNetwork(LongName, 64);
            data.WriteNetwork(NodeReport, 64);
            data.WriteNetwork(PortCount);
            data.Write(PortTypes);
            data.Write(GoodInput);
            data.Write(GoodOutput);
            data.Write(SwIn);
            data.Write(SwOut);
            data.Write(SwVideo);
            data.Write(SwMacro);
            data.Write(SwRemote);
            data.Write(new byte[3]);
            data.Write(Style);
            data.Write(MacAddress);
            data.Write(BindIpAddress);
            data.Write(BindIndex);
            data.Write((byte)Status2);
            data.Write(new byte[208]);
        }
    }
}
