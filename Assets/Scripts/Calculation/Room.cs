using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    private static Room instance;

    [SerializeField] private DmxController controller;
    [SerializeField] private List<Entour_DMXMovingHead> movingHeads = new();

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
        instance.controller.StartSend(2);
        instance.movingHeads.ForEach(x =>
        {
            instance.controller.AppendSend(x, x.RotateToPoint(point));
        });
        instance.controller.EndSend(2);
    }
}
