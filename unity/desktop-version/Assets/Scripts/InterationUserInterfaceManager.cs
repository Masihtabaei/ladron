using UnityEngine;
using TMPro;

public class InteractionUserInterfaceManager : MonoBehaviour
{
    public TextMeshProUGUI hint;
    public void UpdateHint(string message)
    {
        hint.text = message;
    }

}
