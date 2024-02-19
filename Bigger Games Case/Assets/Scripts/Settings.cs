using Script;
using UnityEngine.UI;

public class Settings : Singleton<Settings>
{
    public Toggle vibrationToggle;
    public Toggle soundToggle;

    protected override void Awake()
    {
        base.Awake();
        vibrationToggle.isOn = true;
        soundToggle.isOn = true;
    }
}