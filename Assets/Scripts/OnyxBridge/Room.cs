using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    private static Room instance;
    public static Transform Transform => instance.transform;

    [SerializeField] private DmxController controller;
    [SerializeField] private List<MartinMacMovingHead> controllingMovingHeads = new();

    private readonly short universe = 2;

    private void Awake()
    {
        if (instance)
        {
            Debug.LogError("Second room wants to initiate, refused!");
            return;
        }

        instance = this;
    }

    public static void MoveMovingHeads(Vector3 point)
    {
        if (SettingsManager.Settings.SendData && instance.controller.IsReadyToSend(instance.universe))
        {
            instance.controller.StartSend(instance.universe);
            instance.controllingMovingHeads.ForEach(x => instance.controller.AppendSend(x, x.RotateToPoint(point)));
            instance.controller.EndSend(instance.universe);
        }
        else
        {
            instance.controllingMovingHeads.ForEach(x => x.RotateToPoint(point));
        }
    }
}
