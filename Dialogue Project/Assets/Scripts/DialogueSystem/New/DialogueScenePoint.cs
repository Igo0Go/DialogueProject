using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueScenePoint : MonoBehaviour
{
    [Tooltip("Контроллер персонажей")] public ControllerProcessor controllerProcessor;
    [Tooltip("Схема диалога")] public DialogueSceneKit scene;
    [Tooltip("Подсказка, которая будет высвечиваться.")] public string tip;

    [Space(20), Header("UI"), Tooltip("Панель для субтитров")] public GameObject subsPanel;
    [Tooltip("Текст субтитров")] public Text subsText;
    [Tooltip("Панель для сообщения")] public GameObject messagePanel;
    [Tooltip("Текст сообщения")] public Text messageText;
    [Tooltip("Варианты ответа")] public List<AnswerUI> answers;


    [Space(10), Header("Постановка сцены"), Tooltip("Позиции, куда будут поставлены игроки")] public List<Transform> actorsPoints = new List<Transform>();
    [Tooltip("Основная камера")] public Transform sceneCamera;
    [Tooltip("Позиции для ракурса камеры")] public List<Transform> cameraPoints = new List<Transform>();
    [Tooltip("Источник звука для проигрывания реплик")] public AudioSource audioSource;
    [Tooltip("Участники диалога")] public List<DialogueController> actors = new List<DialogueController>();
    [Tooltip("Если реагируют объекты из сцены")] public List<DialogueEventReactor> reactors;

    [Space(20)]
    public bool once;
    public bool debug;

    private DialogueController activeDialogueController;
    private int currentIndex;
    private int dialogueStatus;
    private int answerNumber;

    void Start()
    {
        dialogueStatus = 0;
        subsPanel.SetActive(false);
        CheckVariants(false);
        HideMessage();
        audioSource.playOnAwake = false;
        audioSource.Stop();
        currentIndex = 0;
    }

    void Update()
    {
        CheckReplic();
    }


    public void StartScene()
    {
        if(currentIndex >= 0)
        {
            controllerProcessor.OnChooseAnswer += CheckAnswer;
            controllerProcessor.inDialog = true;
            for (int i = 0; i < actors.Count; i++)
            {
                actors[i].ToDialogue(actorsPoints[i]);
            }
            StartReplic(currentIndex);
        }
    }
    private void StartReplic(int nodeIndex)
    {
        currentIndex = nodeIndex;
        DialogueNode node = scene.nodes[nodeIndex];
        CheckVariants(false);

        if (node.Type == NodeType.Replica || node.Type == NodeType.Choice)
        {
            sceneCamera.position = cameraPoints[node.CamPositionNumber].position;
            sceneCamera.rotation = cameraPoints[node.CamPositionNumber].rotation;
            sceneCamera.parent = cameraPoints[node.CamPositionNumber];

            if (node.Type == NodeType.Choice)
            {
                dialogueStatus = 2;
                for (int i = 0; i < node.AnswerChoice.Count; i++)
                {
                    CheckVariant(answers[i], scene.nodes[node.AnswerChoice[i]]);
                }
            }
            else
            {
                audioSource.clip = node.Clip;
                audioSource.Play();
                subsPanel.SetActive(true);
                subsText.color = node.Character.color;
                subsText.text = node.ReplicText;
                if (FindController(node.Character))
                {
                    activeDialogueController.SetTalkType(node.AnimType);
                }
                else
                {
                    Debug.LogError("Контроллер не найден");
                }
                if (node.EndNode)
                {
                    dialogueStatus = -1;
                }
                else
                {
                    dialogueStatus = 1;
                    currentIndex = node.NextNodeNumber;
                }
            }
        }
        else if (node.Type == NodeType.Condition)
        {
            if (node.Condition.GetType(node.ConditionNumber, out ConditionType type) && type == ConditionType.Bool)
            {
                if (node.Condition.Check(node.ConditionNumber, node.CheckType, node.CheckBoolValue))
                {
                    StartReplic(node.PositiveNextNumber);
                }
                else
                {
                    StartReplic(node.NegativeNextNumber);
                }
            }
            else
            {
                if (node.Condition.GetType(node.ConditionNumber, out type) && type == ConditionType.Int)
                {
                    if (node.Condition.Check(node.ConditionNumber, node.CheckType, node.CheckIntValue))
                    {
                        StartReplic(node.PositiveNextNumber);
                    }
                    else
                    {
                        StartReplic(node.NegativeNextNumber);
                    }
                }
            }
        }
        else if (node.Type == NodeType.Event)
        {
            if(node.IsMessage)
            {
                messagePanel.SetActive(true);
                messageText.text = node.MessageText;
                Invoke("HideMessage", 4);
            }


            if (!node.ChangeCondition && !node.InSceneInvoke)
            {
                dialogueStatus = 4;
            }
            else
            {
                if (node.ChangeCondition)
                {
                    if (node.Condition.GetType(node.ConditionNumber, out ConditionType type) && type == ConditionType.Bool)
                    {
                        node.Condition.SetBool(node.ConditionNumber, node.ChangeBoolValue);
                    }
                    else
                    {
                        node.Condition.SetInt(node.ConditionNumber, node.Condition.GetInt(node.ConditionNumber) + node.AddIntValue);
                    }
                    if (!node.InSceneInvoke)
                    {
                        dialogueStatus = 4;
                    }
                }
                if (node.InSceneInvoke)
                {
                    foreach (var item in node.ReactorsNumbers)
                    {
                        if (reactors[item] != null)
                        {
                            reactors[item].OnEvent();
                        }
                    }
                    dialogueStatus = 2;
                    sceneCamera.position = cameraPoints[node.EventCamPositionNumber].position;
                    sceneCamera.rotation = cameraPoints[node.EventCamPositionNumber].rotation;
                    sceneCamera.parent = cameraPoints[node.EventCamPositionNumber];
                    Invoke("StopEvent", node.EventTime);
                }
            }
        }
    }
    private void CheckReplic()
    {
        if (dialogueStatus == 4)
        {
            if (scene.nodes[currentIndex].EndNode)
            {
                StopScene();
                dialogueStatus = 0;
            }
            else
            {
                StartReplic(scene.nodes[currentIndex].NextNodeNumber);
            }
        }
        else if (dialogueStatus == 3)
        {
            StartReplic(scene.nodes[currentIndex].AnswerChoice[answerNumber]);
        }
        else if (dialogueStatus != 0 && dialogueStatus != 2)
        {
            if (!audioSource.isPlaying)
            {
                audioSource.Stop();
                activeDialogueController.StopReplic();

                if (dialogueStatus == -1)
                {
                    StopScene();
                    dialogueStatus = 0;
                }
                else
                {
                    StartReplic(currentIndex);
                }
            }
        }
    }
    private void StopScene()
    {
        currentIndex = -1;
        sceneCamera.parent = null;
        subsPanel.SetActive(false);
        CheckVariants(false);
        foreach (var item in actors)
        {
            item.ToDefault();
        }
        controllerProcessor.inDialog = false;
        controllerProcessor.ReturnFromDialogue();
        controllerProcessor.OnChooseAnswer -= CheckAnswer;
        if(once)
        {
            Destroy(gameObject, 5);
        }
    }
    private bool FindController(DialogueCharacter character)
    {
        for (int i = 0; i < actors.Count; i++)
        {
            if (actors[i].dialogueCharacter == character)
            {
                activeDialogueController = actors[i];
                return true;
            }
        }
        return false;
    }
    private void CheckVariants(bool value)
    {
        foreach (var item in answers)
        {
            item.variantPanel.SetActive(value);
        }
    }
    private void CheckVariant(AnswerUI answer, DialogueNode node)
    {
        if (!node.ReplicText.Equals(string.Empty))
        {
            answer.variantPanel.SetActive(true);
            answer.variantText.color = node.Character.color;
            answer.variantText.text = node.ReplicText;
        }
    }
    private void CheckAnswer(int number)
    {
        answerNumber = number;
        dialogueStatus = 3;
    }
    private void HideMessage()
    {
        messagePanel.SetActive(false);
    }
    private void StopEvent()
    {
        dialogueStatus = 4;
    }
    private void OnDrawGizmos()
    {
        if (debug)
        {
            Gizmos.color = Color.green;
            foreach (var item in actorsPoints)
            {
                Gizmos.DrawCube(item.position, new Vector3(0.2f, 0.1f, 0.2f));
            }
            Gizmos.color = Color.cyan;
            foreach (var item in cameraPoints)
            {
                Gizmos.DrawLine(item.position, item.position + item.forward + item.right * 0.3f + item.up * 0.3f);
                Gizmos.DrawLine(item.position, item.position + item.forward + item.right * 0.3f - item.up * 0.3f);
                Gizmos.DrawLine(item.position, item.position + item.forward - item.right * 0.3f + item.up * 0.3f);
                Gizmos.DrawLine(item.position, item.position + item.forward - item.right * 0.3f - item.up * 0.3f);
            }
        }
    }

}
