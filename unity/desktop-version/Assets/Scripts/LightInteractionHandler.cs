using System.Diagnostics.CodeAnalysis;
using UnityEngine;

public class LightInteractionHandler : MonoBehaviour, IInteractable
{
    public bool lightsOn;

    [SerializeField]
    private new Light light;

    public string GetHint()
    {
        if (lightsOn)
        {
            return "Press E to turn lights off.";
        }
        else { return "Press E to turn lights on."; }
    }

    public void React()
    {
        if (lightsOn)
        {
            lightsOn = false;
            light.enabled = false;
        }
        else
        {
            lightsOn = true;
            light.enabled = true;
        }
    }
}