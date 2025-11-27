using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance;
    HashSet<string> completeQuests = new HashSet<string>();


    void Awake()
    {
        Instance = this;
    }

    public void Complete(string questID)
    {
        completeQuests.Add(questID);
    }

    public bool IsComplete(string questID)
    {
        return completeQuests.Contains(questID);
    }
}
