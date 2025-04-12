public enum BuffTriggerEventType { OnAdd, OnRemove, OnTick, OnEvent }
public interface IBuffHandler
{
    void HandleBuffEffect(BuffInstance buff, BuffTriggerEventType triggerEvent, object eventData = null);
}