using System.Diagnostics.CodeAnalysis;
using UnityEngine;

public class LightInteractionHandler : MonoBehaviour, IInteractable
{
    private bool _lightsOn;
    private Light _source;

    private void Awake()
    {
        _lightsOn = false;
        _source = GetComponent<Light>();
    }
    public string GetHint()
    {
        if (_lightsOn)
        {
            return "Press E to turn lights off.";
        }
        else { return "Press E to turn lights on."; }
    }

    public void React()
    {
        _lightsOn = !_lightsOn;
        _source.enabled = !_source.enabled;
    }
}