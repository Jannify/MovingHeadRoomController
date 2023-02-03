using Calculation;
using UnityEngine;

public class MovingPlayerInput : BasicInputBehaviour
{
    public override string Name => "Joystick";

    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private Vector3 spawnPosition;
    [SerializeField] private float movingThreshold;

    private GameObject player;
    private Vector3 lastPosition;

    private void OnEnable()
    {
        player = Instantiate(playerPrefab, spawnPosition, Quaternion.identity, Room.Transform);

    }

    private void OnDisable()
    {
        Destroy(player);
    }

    private void Update()
    {
        if ((lastPosition - player.transform.position).sqrMagnitude > movingThreshold)
        {
            lastPosition = player.transform.position;
            Room.MoveMovingHeads(player.transform.position);
        }
    }
}
