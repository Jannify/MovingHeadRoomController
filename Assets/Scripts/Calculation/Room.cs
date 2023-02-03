using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    private static Room instance;
    public static Transform Transform => instance.transform;

    [SerializeField] private DmxController controller;
    [SerializeField] private List<MartinMacMovingHead> controllingMovingHeads = new();

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
        //instance.movingHeads.ForEach(x => x.RotateToPoint(point));
        instance.controller.StartSend(2);
        instance.controllingMovingHeads.ForEach(x => instance.controller.AppendSend(x, x.RotateToPoint(point)));
        instance.controller.EndSend(2);
    }
}
