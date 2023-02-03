using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Linq;
using UnityEngine;
using ArtNet.Sockets;
using ArtNet.Packets;

public class DmxController : MonoBehaviour
{
    [Header("ArtNet Settings")] [SerializeField]
    private bool useBroadcast;

    [SerializeField] private bool isServer;
    [SerializeField, ReadOnlyField] private string remoteIP = "localhost";
    [SerializeField, ReadOnlyField] private string remoteSubnetMask = "255.255.255.0";

    private ArtNetSocket artnet;
    private IPEndPoint remote;

    [Header("DMX Devices")] [SerializeField]
    private UniverseDevices[] universes;

    [Header("Debug-Data")] [SerializeField, ReadOnlyField]
    private ArtNetDmxPacket latestReceivedDMX;

    [SerializeField, ReadOnlyField] private ArtNetDmxPacket dmxToSend;
    private readonly Dictionary<int, byte[]> dmxDataMap = new();

    public bool IsReadyToSend(short sourceUniverse)
    {
        return artnet.PortOpen && dmxDataMap.ContainsKey(sourceUniverse - 1);
    }

    public void StartSend(short sourceUniverse, short targetUniverse = 5)
    {
        Buffer.BlockCopy(dmxDataMap[sourceUniverse - 1], 0, dmxToSend.DmxData, 0, dmxToSend.DmxData.Length);
        dmxToSend.Universe = targetUniverse;
    }

    public void AppendSend(DMXDevice device, byte[] dmxData)
    {
        Buffer.BlockCopy(dmxData, 0, dmxToSend.DmxData, device.startChannel - 1, dmxData.Length);
    }

    public void EndSend(short sourceUniverse)
    {
        if (dmxDataMap[sourceUniverse - 1].SequenceEqual(dmxToSend.DmxData))
        {
            return;
        }

        if (useBroadcast && isServer)
            artnet.Send(dmxToSend);
        else
            artnet.Send(dmxToSend, remote);
    }

    private void OnValidate()
    {
        foreach (UniverseDevices u in universes)
        {
            u.Initialize();
        }
    }

    private void Start()
    {
        ResetDMXValuesInDevices();

        remoteIP = SettingsManager.Settings.IP;
        remoteSubnetMask = SettingsManager.Settings.SubMask;

        artnet = new ArtNetSocket();

        artnet.EnableBroadcast = useBroadcast;
        if (isServer)
        {
            artnet.Open(FindFromHostName("localhost"), IPAddress.Parse(remoteSubnetMask));
        }
        else
        {
            artnet.Open(IPAddress.Any, IPAddress.Broadcast);
        }

        dmxToSend.DmxData = new byte[512];

        artnet.NewPacket += OnNewPacket;

        if (!useBroadcast || !isServer)
            remote = new IPEndPoint(FindFromHostName(remoteIP), ArtNetSocket.Port);
    }

    private void OnNewPacket(object _, NewPacketEventArgs<ArtNetPacket> e)
    {
        if (e.Packet.OpCode != ArtNet.Enums.ArtNetOpCodes.Dmx)
        {
            Debug.Log(e.Packet.OpCode + ": " + e.Packet.ToArray());
            return;
        }

        ArtNetDmxPacket packet = latestReceivedDMX = e.Packet as ArtNetDmxPacket;

        short universe = packet.Universe;
        if (dmxDataMap.ContainsKey(universe))
            dmxDataMap[universe] = packet.DmxData;
        else
            dmxDataMap.Add(universe, packet.DmxData);
    }

    private void OnDestroy()
    {
        artnet.Close();
    }

    private void Update()
    {
        int[] keys = dmxDataMap.Keys.ToArray();

        foreach (int universe in keys)
        {
            byte[] dmxData = dmxDataMap[universe];
            if (dmxData == null)
                continue;

            int universe1 = universe;
            UniverseDevices universeDevices = universes.FirstOrDefault(u => u.universe == universe1 + 1);
            if (universeDevices != null)
            {
                foreach (DMXDevice d in universeDevices.devices)
                {
                    d.SetData(dmxData.Skip(d.startChannel - 1).Take(d.NumChannels).ToArray());
                }
            }
        }
    }

    private void ResetDMXValuesInDevices()
    {
        foreach (UniverseDevices universe in universes)
        {
            foreach (DMXDevice d in universe.devices)
            {
                d.SetData(new byte[512]);
            }
        }
    }

    private static IPAddress FindFromHostName(string hostname)
    {
        IPAddress address = IPAddress.None;
        try
        {
            if (IPAddress.TryParse(hostname, out address))
                return address;

            var addresses = Dns.GetHostAddresses(hostname);
            foreach (IPAddress ad in addresses)
            {
                if (ad.AddressFamily == AddressFamily.InterNetwork)
                {
                    address = ad;
                    break;
                }
            }
        }
        catch (System.Exception e)
        {
            Debug.LogErrorFormat("Failed to find IP for :\n host name = {0}\n exception={1}", hostname, e);
        }

        return address;
    }

    [System.Serializable]
    public class UniverseDevices
    {
        public string universeName;
        public int universe;
        public DMXDevice[] devices;

        public void Initialize()
        {
            int startChannel = 0;
            foreach (DMXDevice d in devices)
                if (d != null)
                {
                    //d.startChannel = startChannel;
                    startChannel += d.NumChannels;
                    d.name = $"{d.GetType().Name.Split('.').Last()}:({universe}, {d.startChannel:d3}-{d.startChannel + d.NumChannels - 1:d3})";
                }

            if (512 < startChannel)
                Debug.LogErrorFormat(
                    "The number({0}) of channels of the universe {1} exceeds the upper limit(512 channels)!",
                    startChannel, universe);
        }
    }
}
