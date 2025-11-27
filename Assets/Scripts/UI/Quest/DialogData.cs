using UnityEngine;

[CreateAssetMenu(menuName = "NPC/DialogData")]
public class DialogData : ScriptableObject
{
    [System.Serializable]
    public class DialogByQuest
    {
        public string questID;
        [TextArea] public string[] lines;
    }

    public DialogByQuest[] dialogs;
}
