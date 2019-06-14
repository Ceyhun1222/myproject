namespace Holding
{
    public interface IChanged
    {
        bool ModelChanged {get; set; }
        void SetApplyParams();
        event ModelChangedEventHandler ModelChangedEventHandler;
    }
}
