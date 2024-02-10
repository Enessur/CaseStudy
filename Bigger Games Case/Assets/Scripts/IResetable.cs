public interface IResetable
{
    public void OnEnable();
    public void OnDisable();
    public void Reset();
    
    public void Subscription()
    {
        LevelManager.onLevelReset += Reset;
    }

    public void Unsubscription()
    {
        LevelManager.onLevelReset -= Reset;
    }
}