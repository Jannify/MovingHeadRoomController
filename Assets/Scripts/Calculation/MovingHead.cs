using System;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class MovingHead : MonoBehaviour
{
    [SerializeField] private int univers;
    [SerializeField] private int address;
    [SerializeField] private Transform corpus;

    private void OnEnable()
    {
        Room.RegisterMovingHead(this);
    }

    private void OnDisable()
    {
        Room.DeregisterMovingHead(this);
    }

    public void RotateToPoint(Vector3 point)
    {
        corpus.LookAt(point);
    }
}
