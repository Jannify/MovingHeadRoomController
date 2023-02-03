using TMPro;
using UnityEngine;

namespace Calculation
{
    public class InputSelector : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI selectText;
        [SerializeField] private BasicInputBehaviour[] inputs;

        private int index = 0;

        private void Awake()
        {
            UpdateInput();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F3))
            {
                index++;

                if (index >= inputs.Length)
                    index = 0;

                UpdateInput();
            }
        }


        private void UpdateInput()
        {
            for (int i = 0; i < inputs.Length; i++)
            {
                var input = inputs[i];
                input.enabled = this.index == i;
            }

            selectText.text = "Input: " + inputs[index].Name;
        }
    }

    public abstract class BasicInputBehaviour : MonoBehaviour
    {
        public abstract string Name { get; }
    }
}
