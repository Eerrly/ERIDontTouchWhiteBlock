public interface IManager
{
    bool IsInitialized { get; set; }

    void OnInitialize();

    void OnRelease();

}
