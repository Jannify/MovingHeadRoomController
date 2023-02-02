using UnityEngine;

namespace Calculation
{
    public class PlayMovement : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 10;

        private void FixedUpdate()
        {
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");

            transform.Translate(new Vector3(verticalInput, 0, -horizontalInput) * (moveSpeed * Time.deltaTime));
        }
    }
}
