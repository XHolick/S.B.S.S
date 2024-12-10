using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewDialogueData", menuName = "Dialogue/DialogueData")]
public class DialogueData : ScriptableObject
{
    [System.Serializable]
    public class DialogueLine
    {
        public string name; // The speaker's name
        [TextArea] public string text; // The dialogue text
    }

    public List<DialogueLine> talkScript = new List<DialogueLine>();
}