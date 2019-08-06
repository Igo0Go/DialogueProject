using System.Collections.Generic;
using UnityEngine;
using UnityEditor;



[System.Serializable]
[CreateAssetMenu(menuName = "IgoGoTools/DialogueSceneKit")]
public class DialogueSceneKit : ScriptableObject
{
    public string sceneName;

    public List<DialogueNode> nodes = new List<DialogueNode>();
    public void Remove(DialogueNode node)
    {
        if (nodes.Contains(node))
        {
            ClearNextRelations(node);
            ClearPreviousRelations(node);
            int indexBufer;
            if(node.Type == NodeType.Choice)
            {
                for (int i = 0; i < node.AnswerChoice.Count; i++)
                {
                    indexBufer = node.AnswerChoice[i];
                    nodes.Remove(nodes[node.AnswerChoice[i]]);
                    CheckIndexForAll(indexBufer);
                }
            }
            indexBufer = node.Index;
            nodes.Remove(node);
            CheckIndexForAll(indexBufer);
        }
    }
    public bool GetFirst(out DialogueNode node)
    {
        node = null;
        if(nodes.Count > 0)
        {
            node = nodes[0];
            return true;
        }
        else
        {

        }
        return false;
    }
    public void CheckIndexForAll(int removedIndex)
    {
        for (int i = 0; i < nodes.Count; i++)
        {
            for (int j = 0; j < nodes[i].PreviousNodesNumbers.Count; j++)
            {
                if(nodes[i].PreviousNodesNumbers[j] > removedIndex)
                {
                    nodes[i].PreviousNodesNumbers[j]--;
                }
            }
            for (int j = 0; j < nodes[i].AnswerChoice.Count; j++)
            {
                if (nodes[i].AnswerChoice[j] > removedIndex)
                {
                    nodes[i].AnswerChoice[j]--;
                }
            }
            nodes[i].Index = i;
        }
    }


    public void RemoveFromPrevious(DialogueNode from, DialogueNode node)
    {
        if (from.PreviousNodesNumbers.Contains(node.Index))
        {
            from.PreviousNodesNumbers.Remove(node.Index);
        }
    }
    public void AddInPreviousRelations(DialogueNode from, DialogueNode node)
    {
        if (!from.PreviousNodesNumbers.Contains(node.Index))
        {
            from.PreviousNodesNumbers.Add(node.Index);
        }
    }
    public void ClearPreviousRelations(DialogueNode from)
    {
        for (int i = 0; i < from.PreviousNodesNumbers.Count; i++)
        {
            DialogueNode item = nodes[from.PreviousNodesNumbers[i]];
            switch (item.Type)
            {
                case NodeType.Replica:
                    item.NextNodeNumber = -1;
                    break;
                case NodeType.Event:
                    item.NextNodeNumber = -1;
                    break;
                case NodeType.Condition:
                    if (item.PositiveNextNumber == from.Index)
                    {
                        item.PositiveNextNumber = -1;
                    }
                    else if (item.NegativeNextNumber == from.Index)
                    {
                        item.NegativeNextNumber = -1;
                    }
                    break;
            }
        }
    }

    public void RemoveFromNext(DialogueNode from, DialogueNode node)
    {
        switch (from.Type)
        {
            case NodeType.Replica:
                SimpleNodeRemoveFromNext(from, node);
                return;
            case NodeType.Condition:
                ConditionRemoveFromNext(from, node);
                return;
            case NodeType.Event:
                SimpleNodeRemoveFromNext(from, node);
                return;
        }
    }
    public void ClearNextRelations(DialogueNode from)
    {
        switch (from.Type)
        {
            case NodeType.Replica:
                SimpleNodeClearNextRelations(from);
                return;
            case NodeType.Choice:
                ChoiceClearNextRelations(from);
                return;
            case NodeType.Condition:
                ConditionClearNextRelations(from);
                return;
            case NodeType.Event:
                SimpleNodeClearNextRelations(from);
                return;
        }
    }

    public void SimpleNodeRemoveFromNext(DialogueNode from, DialogueNode node)
    {
        if (from.NextNodeNumber != -1 && from.NextNodeNumber == node.Index)
        {
            RemoveFromPrevious(nodes[from.NextNodeNumber], from);
            from.NextNodeNumber = -1;
        }
    }
    public void SimpleNodeClearNextRelations(DialogueNode from)
    {
        if (from.NextNodeNumber != -1)
        {
            if (nodes[from.NextNodeNumber].PreviousNodesNumbers.Contains(from.Index))
            {
                nodes[from.NextNodeNumber].PreviousNodesNumbers.Remove(from.Index);
            }
            from.NextNodeNumber = -1;
        }
    }


    public void ChoiceRemoveFromNext(DialogueNode from, DialogueNode node, int answerNumber)
    {
        if (nodes[from.AnswerChoice[answerNumber]].NextNodeNumber != -1 && nodes[from.AnswerChoice[answerNumber]].NextNodeNumber == node.Index)
        {
            RemoveFromPrevious(nodes[node.Index], nodes[from.AnswerChoice[answerNumber]]);
            nodes[from.AnswerChoice[answerNumber]].NextNodeNumber = -1;
        }
    }
    public void RemoveAnsver(DialogueNode from, int number)
    {
        from.AnswerChoice.Remove(number);
        from.CheckExitOffset();
    }
    public void ChoiceClearNextRelations(DialogueNode from)
    {
        for (int i = 0; i < from.AnswerChoice.Count; i++)
        {
            if(nodes[from.AnswerChoice[i]].NextNodeNumber != -1 &&
                nodes[nodes[from.AnswerChoice[i]].NextNodeNumber].PreviousNodesNumbers.Contains(from.AnswerChoice[i]))
            {
                nodes[nodes[from.AnswerChoice[i]].NextNodeNumber].PreviousNodesNumbers.Remove(from.AnswerChoice[i]);
                nodes[from.AnswerChoice[i]].NextNodeNumber = -1;
            }
        }
    }

    public void ConditionRemoveFromNext(DialogueNode from, DialogueNode node)
    {
        if (from.PositiveNextNumber != -1 && from.PositiveNextNumber == node.Index)
        {
            RemoveFromPrevious(nodes[from.PositiveNextNumber], node);
            from.PositiveNextNumber = -1;
        }
        if (from.NegativeNextNumber != -1 && from.NegativeNextNumber == node.Index)
        {
            RemoveFromPrevious(nodes[from.NegativeNextNumber], node);
            from.NegativeNextNumber = -1;
        }
    }
    public void ConditionRemoveFromNext(DialogueNode from, DialogueNode node, int buttonNumber)
    {
        if (buttonNumber == 0 && from.PositiveNextNumber != -1 && from.PositiveNextNumber == node.Index)
        {
            RemoveFromPrevious(nodes[from.PositiveNextNumber], node);
            from.PositiveNextNumber = -1;
        }
        if (buttonNumber == 1 && from.NegativeNextNumber != -1 && from.NegativeNextNumber == node.Index)
        {
            RemoveFromPrevious(nodes[from.NegativeNextNumber], node);
            from.NegativeNextNumber = -1;
        }
    }
    public void ConditionClearNextRelations(DialogueNode from)
    {
        if (from.PositiveNextNumber != -1 && nodes[from.PositiveNextNumber].PreviousNodesNumbers.Contains(from.Index))
        {
            nodes[from.PositiveNextNumber].PreviousNodesNumbers.Remove(from.Index);
            from.PositiveNextNumber = -1;
        }
        if (from.NegativeNextNumber != -1 && nodes[from.NegativeNextNumber].PreviousNodesNumbers.Contains(from.Index))
        {
            nodes[from.NegativeNextNumber].PreviousNodesNumbers.Remove(from.Index);
            from.NegativeNextNumber = -1;
        }
    }
}


#region Вариант с наследованием

#region Узлы

//[System.Serializable]
//public class DialogueNode 
//{
//    public bool EndNode { get { return endNode; } set { endNode = value; } }
//    [SerializeField]
//    private bool endNode;

//    public List<DialogueNode> PreviousNodes { get { return previousNodes; } set { previousNodes = value; } }
//    [SerializeField]
//    private List<DialogueNode> previousNodes;

//    public Rect TransformRect { get { return transformRect; } set { transformRect = value; } }
//    [SerializeField]
//    private Rect transformRect;

//    public Color ColorInEditor
//    {
//        get
//        {
//            return _colorInEditor;
//        }
//        set
//        {
//            _colorInEditor = value;
//        }
//    }
//    private Color _colorInEditor;

//    public Vector2 EnterPointOffset
//    {
//        get
//        {
//            return _enterPointOffset;
//        }
//        set
//        {
//            _enterPointOffset = value;
//        }
//    }
//    private Vector2 _enterPointOffset;
    
//    public void RemoveFromPrevious(DialogueNode node)
//    {
//        if (PreviousNodes.Contains(node))
//        {
//            PreviousNodes.Remove(node);
//        }
//    }
//    public void ClearPreviousRelations()
//    {
//        foreach (var item in PreviousNodes)
//        {
//            if (item is Replica)
//            {
//                ((Replica)item).NextNode = null;
//            }
//            else if (item is DialogueEventContainer)
//            {
//                ((DialogueEventContainer)item).NextNode = null;
//            }
//            else if (item is ConditionContainer)
//            {
//                ConditionContainer container = item as ConditionContainer;
//                if (container.PositiveNextNode == this)
//                {
//                    container.PositiveNextNode = null;
//                }
//                else if (container.NegativeNextNode == this)
//                {
//                    container.NegativeNextNode = null;
//                }
//            }
//        }
//    }
//    public void AddInPreviousRelations(DialogueNode node)
//    {
//        if (!PreviousNodes.Contains(node))
//        {
//            PreviousNodes.Add(node);
//        }
//    }
//    public virtual void ClearNextRelations()
//    {

//    }
//}

//[System.Serializable]
//public class Replica : DialogueNode
//{
//    public DialogueNode NextNode { get { return nextNode; } set { nextNode = value; } }
//    [SerializeField]
//    private DialogueNode nextNode;

//    public DialogueCharacter Character { get { return character; } set { character = value; } }
//    [SerializeField]
//    private DialogueCharacter character;

//    public AudioClip Clip { get { return clip; } set { clip = value; } }
//    [SerializeField]
//    private AudioClip clip;

//    public DialogueAnimType AnimType { get { return animType; } set { animType = value; } }
//    [SerializeField]
//    private DialogueAnimType animType;

//    public int CamPositionNumber { get { return camPositionNumber; } set { camPositionNumber = value; } }
//    [SerializeField]
//    private int camPositionNumber;

//    public string ReplicText { get { return replicText; } set { replicText = value; } }
//    [SerializeField]
//    private string replicText;


//    public Vector2 ExitPointOffset
//    {
//        get
//        {
//            return _exitPointOffset;
//        }
//    }
//    private Vector2 _exitPointOffset;

//    public Replica()
//    {
//        TransformRect = new Rect(0, 0, 150, 50);
//        ColorInEditor = Color.gray;
//        EnterPointOffset = new Vector2(0, 21);
//        _exitPointOffset = new Vector2(130, 21);
//        PreviousNodes = new List<DialogueNode>();
//    }
//    public Replica(Vector2 pos)
//    {
//        TransformRect = new Rect(pos.x, pos.y, 150, 50);
//        ColorInEditor = Color.gray;
//        EnterPointOffset = new Vector2(0, 21);
//        _exitPointOffset = new Vector2(130, 21);
//        PreviousNodes = new List<DialogueNode>();
//    }
//    public void RemoveFromNext(DialogueNode node)
//    {
//        if (nextNode != null && nextNode == node)
//        {
//            nextNode.RemoveFromPrevious(this);
//            nextNode = null;
//        }
//    }
//    public override void ClearNextRelations()
//    {
//        if (nextNode != null)
//        {
//            if (nextNode.PreviousNodes.Contains(this))
//            {
//                nextNode.PreviousNodes.Remove(this);
//            }
//            nextNode = null;
//        }
//    }
//}
//[System.Serializable]
//public class Choice : DialogueNode
//{
//    public DialogueCharacter Character { get { return character; } set { character = value; } }
//    [SerializeField]
//    private DialogueCharacter character;

//    public List<Replica> AnswerChoice { get { return answerChoice; } set { answerChoice = value; } }
//    [SerializeField]
//    private List<Replica> answerChoice;

//    public int CamPositionNumber { get { return camPositionNumber; } set { camPositionNumber = value; } }
//    [SerializeField]
//    private int camPositionNumber;

//    public List<Vector2> ExitPointsOffset
//    {
//        get
//        {
//            return _exitPointsOffset;
//        }
//    }
//    private readonly List<Vector2> _exitPointsOffset;

//    public Choice()
//    {
//        answerChoice = new List<Replica>();
//        TransformRect = new Rect(250, 30, 200, 50);
//        ColorInEditor = Color.grey;
//        EnterPointOffset = new Vector2(0, 21);
//        _exitPointsOffset = new List<Vector2>();
//        PreviousNodes = new List<DialogueNode>();
//    }
//    public Choice(Vector2 pos)
//    {
//        answerChoice = new List<Replica>();
//        TransformRect = new Rect(pos.x, pos.y, 200, 50);
//        ColorInEditor = Color.grey;
//        EnterPointOffset = new Vector2(0, 21);
//        _exitPointsOffset = new List<Vector2>();
//        PreviousNodes = new List<DialogueNode>();
//    }

//    public void RemoveFromNext(DialogueNode node, int answerNumber)
//    {
//        if (answerChoice[answerNumber].NextNode != null && answerChoice[answerNumber].NextNode == node)
//        {
//            answerChoice[answerNumber].NextNode.RemoveFromPrevious(answerChoice[answerNumber]);
//            answerChoice[answerNumber].NextNode = null;
//        }
//    }
//    public void RemoveAnsver(int number)
//    {
//        answerChoice.Remove(answerChoice[number]);
//        CheckExitOffset();
//    }
//    public void CheckExitOffset()
//    {
//        _exitPointsOffset.Clear();
//        for (int i = 0; i < answerChoice.Count; i++)
//        {
//            _exitPointsOffset.Add(new Vector2(180, 21 + i * 21));
//        }
//    }
//    public override void ClearNextRelations()
//    {
//        for (int i = 0; i < answerChoice.Count; i++)
//        {
//            if (answerChoice[i].NextNode != null && answerChoice[i].NextNode.PreviousNodes.Contains(answerChoice[i]))
//            {
//                answerChoice[i].NextNode.PreviousNodes.Remove(answerChoice[i]);
//                answerChoice[i].NextNode = null;
//            }
//        }
//    }
//}
//[System.Serializable]
//public class ConditionContainer : DialogueNode
//{
//    public ConditionPack Condition { get { return condition; } set { condition = value; } }
//    [SerializeField]
//    private ConditionPack condition;

//    public DialogueNode PositiveNextNode { get { return positiveNextNode; } set { positiveNextNode = value; } }
//    [SerializeField]
//    private DialogueNode positiveNextNode;


//    public DialogueNode NegativeNextNode { get { return negativeNextNode; } set { negativeNextNode = value; } }
//    [SerializeField]
//    private DialogueNode negativeNextNode;

//    public CheckType CheckType { get { return checkType; } set { checkType = value; } }
//    [SerializeField]
//    private CheckType checkType;

//    public int ConditionNumber { get { return conditionNumber; } set { conditionNumber = value; } }
//    [SerializeField]
//    private int conditionNumber;

//    public bool CheckBoolValue { get { return checkBoolValue; } set { checkBoolValue = value; } }
//    [SerializeField]
//    private bool checkBoolValue;

//    public int CheckIntValue { get { return checkIntValue; } set { checkIntValue = value; } }
//    [SerializeField]
//    private int checkIntValue;

//    public Vector2 PositiveExitPointOffset
//    {
//        get
//        {
//            return _positiveExitPointOffset;
//        }
//    }
//    private Vector2 _positiveExitPointOffset;
//    public Vector2 NegativeExitPointOffset
//    {
//        get
//        {
//            return _negativeExitPointOffset;
//        }
//    }
//    private Vector2 _negativeExitPointOffset;

//    public ConditionContainer()
//    {
//        TransformRect = new Rect(300, 100, 180, 65);
//        ColorInEditor = Color.cyan;
//        EnterPointOffset = new Vector2(0, 21);
//        _positiveExitPointOffset = new Vector2(150, 21);
//        _negativeExitPointOffset = new Vector2(150, 42);
//        PreviousNodes = new List<DialogueNode>();
//    }
//    public ConditionContainer(Vector2 pos)
//    {
//        TransformRect = new Rect(pos.x, pos.y, 180, 65);
//        ColorInEditor = Color.cyan;
//        EnterPointOffset = new Vector2(0, 21);
//        _positiveExitPointOffset = new Vector2(150, 21);
//        _negativeExitPointOffset = new Vector2(150, 42);
//        PreviousNodes = new List<DialogueNode>();
//    }

//    public void RemoveFromNext(DialogueNode node)
//    {
//        if (positiveNextNode != null && positiveNextNode == node)
//        {
//            positiveNextNode.RemoveFromPrevious(this);
//            positiveNextNode = null;
//        }
//        if (negativeNextNode != null && negativeNextNode == node)
//        {
//            negativeNextNode.RemoveFromPrevious(this);
//            negativeNextNode = null;
//        }
//    }
//    public override void ClearNextRelations()
//    {
//        if (positiveNextNode != null && positiveNextNode.PreviousNodes.Contains(this))
//        {
//            positiveNextNode.PreviousNodes.Remove(this);
//            positiveNextNode = null;
//        }
//        if (negativeNextNode != null && negativeNextNode.PreviousNodes.Contains(this))
//        {
//            negativeNextNode.PreviousNodes.Remove(this);
//            negativeNextNode = null;
//        }
//    }
//}
//[System.Serializable]
//public class DialogueEventContainer : DialogueNode
//{
//    public ConditionPack ConditionCharacteristic { get { return conditionCharacteristic; } set { conditionCharacteristic = value; } }
//    [SerializeField]
//    private ConditionPack conditionCharacteristic;

//    public DialogueNode NextNode { get { return nextNode; } set { nextNode = value; } }
//    [SerializeField]
//    private DialogueNode nextNode;

//    public List<int> ReactorsNumbers { get { return reactorsNumbers; } set { reactorsNumbers = value; } }
//    [SerializeField]
//    private List<int> reactorsNumbers;

//    public ConditionType ChangeConditionType { get { return changeConditionType; } set { changeConditionType = value; } }
//    [SerializeField]
//    private ConditionType changeConditionType;

//    public string MessageText { get { return messageText; } set { messageText = value; } }
//    [SerializeField]
//    private string messageText;

//    public float EventTime { get { return eventTime; } set { eventTime = value; } }
//    [SerializeField]
//    private float eventTime;

//    public bool ChangeCondition { get { return changeCondition; } set { changeCondition = value; } }
//    [SerializeField]
//    private bool changeCondition;

//    public bool InSceneInvoke { get { return inSceneInvoke; } set { inSceneInvoke = value; } }
//    [SerializeField]
//    private bool inSceneInvoke;

//    public bool IsMessage { get { return isMessage; } set { isMessage = value; } }
//    [SerializeField]
//    private bool isMessage;

//    public bool ChangeBoolValue { get { return changeBoolValue; } set { changeBoolValue = value; } }
//    [SerializeField]
//    private bool changeBoolValue;

//    public int ChangeConditionNumber { get { return changeConditionNumber; } set { changeConditionNumber = value; } }
//    [SerializeField]
//    private int changeConditionNumber;

//    public int AddIntValue { get { return addIntValue; } set { addIntValue = value; } }
//    [SerializeField]
//    private int addIntValue;

//    public int EventCamPositionNumber { get { return eventCamPositionNumber; } set { eventCamPositionNumber = value; } }
//    [SerializeField]
//    private int eventCamPositionNumber;


//    public Vector2 ExitPointOffset
//    {
//        get
//        {
//            return _exitPointOffset;
//        }
//    }
//    private Vector2 _exitPointOffset;
//    public DialogueEventContainer()
//    {
//        reactorsNumbers = new List<int>();
//        TransformRect = new Rect(200, 200, 150, 90);
//        ColorInEditor = Color.yellow;
//        EnterPointOffset = new Vector2(0, 21);
//        _exitPointOffset = new Vector2(130, 21);
//        PreviousNodes = new List<DialogueNode>();
//    }
//    public DialogueEventContainer(Vector2 pos)
//    {
//        reactorsNumbers = new List<int>();
//        TransformRect = new Rect(pos.x, pos.y, 150, 90);
//        ColorInEditor = Color.yellow;
//        EnterPointOffset = new Vector2(0, 21);
//        _exitPointOffset = new Vector2(130, 21);
//        PreviousNodes = new List<DialogueNode>();
//    }
//    public void RemoveFromNext(DialogueNode node)
//    {
//        if (nextNode != null && nextNode == node)
//        {
//            nextNode.RemoveFromPrevious(this);
//            nextNode = node;
//        }
//    }
//    public override void ClearNextRelations()
//    {
//        if (nextNode != null)
//        {
//            if (nextNode.PreviousNodes.Contains(this))
//            {
//                nextNode.PreviousNodes.Remove(this);
//            }
//            nextNode = null;
//        }
//    }
//}
#endregion



[System.Serializable]
public enum NodeType
{
    Replica,
    Choice,
    Event,
    Condition
}
//[System.Serializable]
//public abstract class DialogueEventReactor : MonoBehaviour
//{
//    public abstract void OnEvent();
//}

#endregion

[System.Serializable]
public class DialogueNode
{
    #region Общее

    public int Index { get { return index; } set { index = value; } }
    [SerializeField]
    private int index;

    public bool Hide { get { return hide; }}
    [SerializeField]
    private bool hide;

    public NodeType Type { get { return _type; } }
    [SerializeField]
    private NodeType _type;
    public bool EndNode { get { return endNode; } set { endNode = value; } }
    [SerializeField]
    private bool endNode;

    public bool LeftToRight { get { return _leftToRight; } set { _leftToRight = value; } }
    [SerializeField]
    private bool _leftToRight;

    [SerializeField]
    public List<int> PreviousNodesNumbers { get { return previousNodesNumbers; } set { previousNodesNumbers = value; } }
    [SerializeField]
    private List<int> previousNodesNumbers;

    public Rect TransformRect { get { return transformRect; } set { transformRect = value; } }
    [SerializeField]
    private Rect transformRect;

    public Color ColorInEditor
    {
        get
        {
            return _colorInEditor;
        }
        set
        {
            _colorInEditor = value;
        }
    }
    [SerializeField]
    private Color _colorInEditor;

    public Vector2 LeftPointOffset
    {
        get
        {
            return _leftPointOffset;
        }
    }
    [SerializeField]
    private Vector2 _leftPointOffset;

    public DialogueNode(NodeType nodeType, int index)
    {
        _leftToRight = true;
        _type = nodeType;
        Index = index;
        nextNodesNumbers = new List<int>();
        _leftPointOffset = new Vector2(0, 21);

        switch (Type)
        {
            case NodeType.Replica:
                TransformRect = new Rect(0, 0, 150, 50);
                ColorInEditor = Color.gray;
                PreviousNodesNumbers = new List<int>();
                _rightPointOffset = new Vector3(130, 21);
                break;
            case NodeType.Choice:
                TransformRect = new Rect(0, 0, 200, 50);
                _rightPointOffset = new Vector3(180, 21);
                ColorInEditor = Color.grey;
                _exitPointsOffset = new List<Vector2>();
                PreviousNodesNumbers = new List<int>();
                break;
            case NodeType.Condition:
                TransformRect = new Rect(0, 0, 180, 65);
                _rightPointOffset = new Vector3(160, 21);
                ColorInEditor = Color.cyan;
                _positiveExitPointOffset = new Vector2(150, 21);
                _negativeExitPointOffset = new Vector2(150, 42);
                PreviousNodesNumbers = new List<int>();
                break;
            case NodeType.Event:
                reactorsNumbers = new List<int>();
                TransformRect = new Rect(0, 0, 150, 90);
                ColorInEditor = Color.yellow;
                _rightPointOffset = new Vector2(130, 21);
                PreviousNodesNumbers = new List<int>();
                break;

        }
    }
    public DialogueNode(NodeType nodeType, int index, bool hideStatus)
    {
        _leftToRight = true;
        _type = nodeType;
        Index = index;
        hide = hideStatus;
        nextNodesNumbers = new List<int>();
        _leftPointOffset = new Vector2(0, 21);
        switch (Type)
        {
            case NodeType.Replica:
                TransformRect = new Rect(0, 0, 150, 50);
                ColorInEditor = Color.gray;
                _rightPointOffset = new Vector2(130, 21);
                PreviousNodesNumbers = new List<int>();
                break;
            case NodeType.Choice:
                TransformRect = new Rect(0, 0, 200, 50);
                _rightPointOffset = new Vector3(180, 21);
                ColorInEditor = Color.grey;
                _exitPointsOffset = new List<Vector2>();
                PreviousNodesNumbers = new List<int>();
                break;
            case NodeType.Condition:
                TransformRect = new Rect(0, 0, 180, 65);
                _rightPointOffset = new Vector3(160, 21);
                ColorInEditor = Color.cyan;
                _positiveExitPointOffset = new Vector2(150, 21);
                _negativeExitPointOffset = new Vector2(150, 42);
                PreviousNodesNumbers = new List<int>();
                break;
            case NodeType.Event:
                reactorsNumbers = new List<int>();
                TransformRect = new Rect(0, 0, 150, 90);
                ColorInEditor = Color.yellow;
                _rightPointOffset = new Vector2(130, 21);
                PreviousNodesNumbers = new List<int>();
                break;

        }
    }
    public DialogueNode(Vector2 pos, NodeType nodeType, int index)
    {
        _leftToRight = true;
        _type = nodeType;
        Index = index;
        nextNodesNumbers = new List<int>();
        _leftPointOffset = new Vector2(0, 21);
        switch (Type)
        {
            case NodeType.Replica:
                TransformRect = new Rect(pos.x, pos.y, 150, 50);
                ColorInEditor = Color.gray;
                _rightPointOffset = new Vector2(130, 21);
                PreviousNodesNumbers = new List<int>();
                break;
            case NodeType.Choice:
                TransformRect = new Rect(pos.x, pos.y, 200, 50);
                _rightPointOffset = new Vector3(180, 21);
                ColorInEditor = Color.grey;
                _exitPointsOffset = new List<Vector2>();
                PreviousNodesNumbers = new List<int>();
                break;
            case NodeType.Condition:
                TransformRect = new Rect(pos.x, pos.y, 180, 65);
                _rightPointOffset = new Vector3(160, 21);
                ColorInEditor = Color.cyan;
                _positiveExitPointOffset = new Vector2(150, 21);
                _negativeExitPointOffset = new Vector2(150, 42);
                PreviousNodesNumbers = new List<int>();
                positiveNumber = negativeNumber = -1;
                break;
            case NodeType.Event:
                reactorsNumbers = new List<int>();
                TransformRect = new Rect(pos.x, pos.y, 150, 90);
                ColorInEditor = Color.yellow;
                _rightPointOffset = new Vector2(130, 21);
                PreviousNodesNumbers = new List<int>();
                break;
        }
    }

    #endregion

    #region Реплика
    public int NextNodeNumber {
        get
        {
            if(nextNodesNumbers.Count == 0)
            {
                return -1;
            }
            return nextNodesNumbers[0];
        }
        set
        {
            if(value == -1)
            {
                nextNodesNumbers.Clear();
                return;
            }
            if(nextNodesNumbers.Count == 0)
            {
                nextNodesNumbers.Add(value);
            }
            else
            {
                nextNodesNumbers[0] = value;
            }
        }
    }
    public DialogueCharacter Character { get { return character; } set { character = value; } }
    [SerializeField]
    private DialogueCharacter character;

    public AudioClip Clip { get { return clip; } set { clip = value; } }
    [SerializeField]
    private AudioClip clip;

    public DialogueAnimType AnimType { get { return animType; } set { animType = value; } }
    [SerializeField]
    private DialogueAnimType animType;

    public int CamPositionNumber { get { return camPositionNumber; } set { camPositionNumber = value; } }
    [SerializeField]
    private int camPositionNumber;

    public string ReplicText { get { return replicText; } set { replicText = value; } }
    [SerializeField]
    private string replicText;


    public Vector2 RightPointOffset
    {
        get
        {
            return _rightPointOffset;
        }
    }
    [SerializeField]
    private Vector2 _rightPointOffset;

    #endregion

    #region Развилка
    public List<int> AnswerChoice { get { return nextNodesNumbers; } set { nextNodesNumbers = value; } }
    [SerializeField]
    private List<int> nextNodesNumbers;

    public List<Vector2> ExitPointsOffset
    {
        get
        {
            return _exitPointsOffset;
        }
    }
    [SerializeField]
    private List<Vector2> _exitPointsOffset;

    public void RemoveAnsver(int number)
    {
        AnswerChoice.Remove(AnswerChoice[number]);
        CheckExitOffset();
    }
    public void CheckExitOffset()
    {
        _exitPointsOffset.Clear();
        for (int i = 0; i < AnswerChoice.Count; i++)
        {
            _exitPointsOffset.Add(new Vector2(180, 21 + i * 21));
        }
    }
    #endregion

    #region Условие
    public ConditionPack Condition { get { return condition; } set { condition = value; } }
    [SerializeField]
    private ConditionPack condition;

    public int PositiveNextNumber
    {
        get
        {
            if(positiveNumber != -1)
            {
                return nextNodesNumbers[positiveNumber];
            }
            return -1;
        }
        set
        {
            if(value == -1 && nextNodesNumbers.Count >= positiveNumber-1)
            {
                nextNodesNumbers.Remove(nextNodesNumbers[positiveNumber]);
                positiveNumber = -1;
                if(negativeNumber == 1)
                {
                    negativeNumber = 0;
                }
                return;
            }
            if(nextNodesNumbers.Count == 0)
            {
                nextNodesNumbers.Add(value);
                positiveNumber = 0;
            }
            else if(nextNodesNumbers.Count == 1)
            {
                if(positiveNumber != -1)
                {
                    nextNodesNumbers[positiveNumber] = value;
                }
                else
                {
                    nextNodesNumbers.Add(value);
                    positiveNumber = 1;
                }
            }
            else
            {
                nextNodesNumbers[positiveNumber] = value;
            }
        }
    }
    public int NegativeNextNumber
    {
        get
        {
            if (negativeNumber != -1)
            {
                return nextNodesNumbers[negativeNumber];
            }
            return -1;
        }
        set
        {
            if (value == -1 && nextNodesNumbers.Count >= negativeNumber - 1)
            {
                nextNodesNumbers.Remove(nextNodesNumbers[negativeNumber]);
                negativeNumber = -1;
                if (positiveNumber == 1)
                {
                    positiveNumber = 0;
                }
                return;
            }
            if (nextNodesNumbers.Count == 0)
            {
                nextNodesNumbers.Add(value);
                negativeNumber = 0;
            }
            else if (nextNodesNumbers.Count == 1)
            {
                if (negativeNumber != -1)
                {
                    nextNodesNumbers[negativeNumber] = value;
                }
                else
                {
                    nextNodesNumbers.Add(value);
                    negativeNumber = 1;
                }
            }
            else
            {
                nextNodesNumbers[negativeNumber] = value;
            }
        }
    }

    [SerializeField]
    private int positiveNumber;
    [SerializeField]
    private int negativeNumber;
    public CheckType CheckType { get { return checkType; } set { checkType = value; } }
    [SerializeField]
    private CheckType checkType;

    public int ConditionNumber { get { return conditionNumber; } set { conditionNumber = value; } }
    [SerializeField]
    private int conditionNumber;

    public bool CheckBoolValue { get { return checkBoolValue; } set { checkBoolValue = value; } }
    [SerializeField]
    private bool checkBoolValue;

    public int CheckIntValue { get { return checkIntValue; } set { checkIntValue = value; } }
    [SerializeField]
    private int checkIntValue;

    public Vector2 PositiveExitPointOffset
    {
        get
        {
            return _positiveExitPointOffset;
        }
    }
    [SerializeField]
    private Vector2 _positiveExitPointOffset;
    public Vector2 NegativeExitPointOffset
    {
        get
        {
            return _negativeExitPointOffset;
        }
    }
    [SerializeField]
    private Vector2 _negativeExitPointOffset;

    #endregion

    #region Событие

    public List<int> ReactorsNumbers { get { return reactorsNumbers; } set { reactorsNumbers = value; } }
    [SerializeField]
    private List<int> reactorsNumbers;

    public string MessageText { get { return messageText; } set { messageText = value; } }
    [SerializeField]
    private string messageText;

    public float EventTime { get { return eventTime; } set { eventTime = value; } }
    [SerializeField]
    private float eventTime;

    public bool ChangeCondition { get { return changeCondition; } set { changeCondition = value; } }
    [SerializeField]
    private bool changeCondition;

    public bool InSceneInvoke { get { return inSceneInvoke; } set { inSceneInvoke = value; } }
    [SerializeField]
    private bool inSceneInvoke;

    public bool IsMessage { get { return isMessage; } set { isMessage = value; } }
    [SerializeField]
    private bool isMessage;

    public bool ChangeBoolValue { get { return changeBoolValue; } set { changeBoolValue = value; } }
    [SerializeField]
    private bool changeBoolValue;

    public int AddIntValue { get { return addIntValue; } set { addIntValue = value; } }
    [SerializeField]
    private int addIntValue;

    public int EventCamPositionNumber { get { return eventCamPositionNumber; } set { eventCamPositionNumber = value; } }
    [SerializeField]
    private int eventCamPositionNumber;


    #endregion
}
