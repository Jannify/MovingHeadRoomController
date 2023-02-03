using UnityEngine;

namespace Utils
{
    [ExecuteAlways]
    public class FollowObject : MonoBehaviour
    {
        [SerializeField] private GameObject followObject;

        private void Update()
        {
            if (followObject)
            {
                transform.position = followObject.transform.position;
            }

            enabled = false;
        }
    }
}
