using FramePush.DarkModeDetect;
using UnityEngine;

public class DarkModeDetector : MonoBehaviour
{
    [SerializeField] private GameObject backGround;
    [SerializeField] private Color dayLightColor;
    [SerializeField] private Color nightColor;

    
    void Start()
    {
        CheckDarkMode();
    }

    void CheckDarkMode()
    {
        switch (FramePush.DarkModeDetect.DarkModeDetector.CurrentMode)
        {
            case FramePush.DarkModeDetect.Mode.Dark:
                backGround.GetComponent<SpriteRenderer>().material.color = nightColor;
                break;
            case FramePush.DarkModeDetect.Mode.Light:
                backGround.GetComponent<SpriteRenderer>().material.color = dayLightColor;
                break;
            case FramePush.DarkModeDetect.Mode.Unspecified:
                backGround.GetComponent<SpriteRenderer>().material.color = nightColor;
                break;
        }
    }
}