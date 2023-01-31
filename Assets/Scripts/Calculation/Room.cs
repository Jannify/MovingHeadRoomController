using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    private static Room instance;

    private List<MovingHead> movingHeads = new();

    // Start is called before the first frame update
    void Awake()
    {
        if (instance)
        {
            Debug.LogError("Second room wants to initiate, refused!");
            return;
        }

        instance = this;
    }



    public static void RegisterMovingHead(MovingHead mh) => instance.movingHeads.Add(mh);
    public static void DeregisterMovingHead(MovingHead mh) => instance.movingHeads.Remove(mh);

    public static void MoveMovingHeads(Vector3 point)
    {
        instance.movingHeads.ForEach(x => x.RotateToPoint(point));
    }
}
