using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "IgoGoTools/Dialogue")]
public class DialogueScene : ScriptableObject
{
    public List<DialogueNodePack> dialogueNodes = new List<DialogueNodePack>();
}

[System.Serializable]
public class DialogueNodePack
{
    [Tooltip("Тип узла")]public NodeType type;
    [Tooltip("Номер следующего узла (не нужен в условии)")] public int nextReplicNumber = - 1;

    [Space(20)]
    [Header("Для реплики и диалогов")]
    public DialogueCharacter character;
    public string replicText;
    public AudioClip clip;
    public DialogueAnimType animType;
    public int camPositionNumber;
    public bool endReplic;
    [Space(20)]
    [Header("Для диалогов")]
    public List<DialogueNodePack> answerСhoice;

    [Space(20)]
    [Header("Для условий")]
    public ConditionPack condition;
    public ConditionType conditionType;
    public int positiveNextNode;
    public int negativeNextNode;
    [Space(10)]
    public string conditionName;
    public CheckType checkType;
    public bool boolValue;
    public int intValue;

    [Space(20)]
    [Header("Для событий")]
    public DialogueEvent dialogueEvent;
    

    [Space(10)]
    [Tooltip("Если реагируют объекты из сцены")] public List<int> reactorsNumber;
    [Tooltip("Если реагирует объект из сцены, позиция, на которой должна в этот момент находиться камера.")] public int eventCamPositionNumber;
    [Tooltip("Если реагирует объект из сцены, время перед запуском следующего узла (секунды)."), Range(0,120)] public float eventTime;



    [HideInInspector] public DialogueNodePack previousDialogueNode = null;
    [HideInInspector] public DialogueNodePack nextDialogueNode = null;
}
public enum DialogueAnimType
{
    Talk,
    Yes,
    No,
    Nervously,
    InSurprise,
    Quastion,
    Fear,
}