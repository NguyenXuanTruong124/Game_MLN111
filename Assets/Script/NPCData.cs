using UnityEngine;

[CreateAssetMenu(fileName = "NPC Data", menuName = "NPC/New NPC")]
public class NPCData : ScriptableObject
{
    [SerializeField] public string npcName = "NPC";
    [SerializeField] public string question = "Bạn là ai?";
    [SerializeField] public string[] answers = { "Câu trả lời 1", "Câu trả lời 2", "Câu trả lời 3" };
    [SerializeField] public Sprite npcSprite;
    [SerializeField] public float interactionDistance = 2f;
}
