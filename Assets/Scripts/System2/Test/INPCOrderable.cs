using UnityEngine;

public interface INPCOrderable
{
    bool HasOrder { get; }
    OrderData GetOrder();
    void MarkOrderReceived();
}
