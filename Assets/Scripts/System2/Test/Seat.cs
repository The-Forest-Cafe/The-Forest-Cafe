using UnityEngine;

public class Seat : MonoBehaviour
{
    public CustomerNPC OccupiedNPC { get; private set; }
    public bool IsOccupied => OccupiedNPC != null;

    public void AssignNPC(CustomerNPC npc)
    {
        OccupiedNPC = npc;
    }

    public void RemoveNPC()
    {
        OccupiedNPC = null;
    }
}
