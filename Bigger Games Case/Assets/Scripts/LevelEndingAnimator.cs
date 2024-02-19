public static class LevelEndingAnimator
{
    public const float RotateDuration = 0.75f;
    public const float ScaleDuration = 0.5f;
    public const float Delay = 0.3f;
    public static float PuzzleSizeDelay;

    public static float GetNextLevelDuration()
    {
        return RotateDuration + ScaleDuration + Delay + PuzzleSizeDelay;
    }
}