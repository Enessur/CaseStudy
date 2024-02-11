public interface IReloadable
{
    public void OnEnable();
    public void OnDisable();


    public void UnRegister();

    public void Subscription()
    {
        Node.onReload += UnRegister;
    }
    public void Unsubscription()
    {
        Node.onReload -= UnRegister;
    }
}