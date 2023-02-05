using UnityEngine;

public class SendingOverride : MonoBehaviour
{
    public static bool ShouldSend;

    [SerializeField] private GameObject[] elements;


    void Start()
    {
        SetOverride(ShouldSend);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F5))
        {
            SetOverride(!ShouldSend);
        }
    }

    public void SetOverride(bool value)
    {
        ShouldSend = value;
        foreach (GameObject element in elements)
        {
            element.SetActive(value);
        }
    }
}
