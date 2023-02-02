using TMPro;
using UnityEngine;

namespace Calculation
{
    public class InputSelector : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI selectText;

        private IInput[] inputs;
        private int index = 1;


        private void Awake()
        {
            inputs = GetComponents<IInput>();

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
                input.SetEnabled(this.index == i);
            }

            selectText.text = "Input: " + inputs[index].Name;
        }
    }

    public interface IInput
    {
        public string Name { get; }
        public void SetEnabled(bool value);
    }
}
