using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueSceneProcessor : MonoBehaviour
{
    [Tooltip("Схема диалога")] public ControllerProcessor controllerProcessor;
    [Tooltip("Схема диалога")] public DialogueScene scene;
    
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


    public bool debug;

    private DialogueController activeDialogueController;
    private int currentReplicNumber;
    private int dialogueStatus;
    private int answerNumber;

    void Start()
    {
        currentReplicNumber = 0;
        dialogueStatus = 0;
        subsPanel.SetActive(false);
        CheckVariants(false);
        HideMessage();
        audioSource.playOnAwake = false;
        audioSource.Stop();
    }

    void Update()
    {
        CheckReplic();
    }

    public void StartScene()
    {
        controllerProcessor.OnChooseAnswer += CheckAnswer;
        controllerProcessor.inDialog = true;
        for (int i = 0; i < actors.Count; i++)
        {
            actors[i].ToDialogue(actorsPoints[i]);
        }
        StartReplic(scene.dialogueNodes[0]);
    }
    private void StartReplic(DialogueNodePack node)
    {
        CheckVariants(false);

        if (node.type == NodeType.Replica || node.type == NodeType.Choice)
        {
            sceneCamera.position = cameraPoints[node.camPositionNumber].position;
            sceneCamera.rotation = cameraPoints[node.camPositionNumber].rotation;

            if (node.type == NodeType.Choice)
            {
                dialogueStatus = 2;
                for (int i = 0; i < node.answerСhoice.Count; i++)
                {
                    CheckVariant(answers[i], node.answerСhoice[i]);
                }
            }
            else
            {
                audioSource.clip = node.clip;
                audioSource.Play();
                subsPanel.SetActive(true);
                subsText.color = node.character.color;
                subsText.text = node.replicText;
                if (FindController(node.character))
                {
                    activeDialogueController.SetTalkType(node.animType);
                }
                else
                {
                    Debug.LogError("Контроллер не найден");
                }
                if (node.endReplic)
                {
                    dialogueStatus = -1;
                }
                else
                {
                    dialogueStatus = 1;
                    currentReplicNumber = node.nextReplicNumber;
                }
            }
        }
        else if (node.type == NodeType.Condition)
        {
            if (node.conditionType == ConditionType.Bool)
            {
                if (node.condition.Check(node.conditionName, node.checkType, node.boolValue))
                {
                    StartReplic(scene.dialogueNodes[node.positiveNextNode]);
                }
                else
                {
                    StartReplic(scene.dialogueNodes[node.negativeNextNode]);
                }
            }
            else
            {
                if (node.conditionType == ConditionType.Int)
                {
                    if (node.condition.Check(node.conditionName, node.checkType, node.intValue))
                    {
                        StartReplic(scene.dialogueNodes[node.positiveNextNode]);
                    }
                    else
                    {
                        StartReplic(scene.dialogueNodes[node.negativeNextNode]);
                    }
                }
            }
        }
        else if (node.type == NodeType.Event)
        {
            if(!node.dialogueEvent.changeCondition && !node.dialogueEvent.inSceneInvoke)
            {
                dialogueStatus = 4;
            }
            else
            {
                if (node.dialogueEvent.isMessage)
                {
                    messagePanel.SetActive(true);
                    messageText.text = node.dialogueEvent.messageText;
                    Invoke("HideMessage", 2);
                }
                if (node.dialogueEvent.changeCondition)
                {
                    if (node.dialogueEvent.changeConditionType == ConditionType.Bool)
                    {
                        node.dialogueEvent.conditionCharacteristic.SetBool(node.dialogueEvent.changeConditionName, node.dialogueEvent.changeBoolValue);
                    }
                    else
                    {
                        node.dialogueEvent.conditionCharacteristic.SetInt(node.dialogueEvent.changeConditionName,
                                                node.dialogueEvent.conditionCharacteristic.GetInt(node.dialogueEvent.changeConditionName)
                                                + node.dialogueEvent.addIntValue);
                    }
                    if (!node.dialogueEvent.inSceneInvoke)
                    {
                        dialogueStatus = 4;
                    }
                }
                if (node.dialogueEvent.inSceneInvoke)
                {
                    foreach (var item in node.reactorsNumber)
                    {
                        if (reactors[item] != null)
                        {
                            reactors[item].OnEvent();
                        }
                    }
                    dialogueStatus = 2;
                    sceneCamera.position = cameraPoints[node.eventCamPositionNumber].position;
                    sceneCamera.rotation = cameraPoints[node.eventCamPositionNumber].rotation;
                    Invoke("StopEvent", node.eventTime);
                }
            }
     }
    }
    private void CheckReplic()
    {
        if(dialogueStatus == 4)
        {
            if (scene.dialogueNodes[currentReplicNumber].endReplic)
            {
                StopScene();
                dialogueStatus = 0;
            }
            else
            {
                StartReplic(scene.dialogueNodes[scene.dialogueNodes[currentReplicNumber].nextReplicNumber]);
            }
        }
        else if(dialogueStatus == 3)
        {
            StartReplic(scene.dialogueNodes[currentReplicNumber].answerСhoice[answerNumber]);
        }
        else if(dialogueStatus != 0 && dialogueStatus != 2)
        {
            if(!audioSource.isPlaying)
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
                    StartReplic(scene.dialogueNodes[currentReplicNumber]);
                }
            }
        }
    }
    private void StopScene()
    {
        subsPanel.SetActive(false);
        CheckVariants(false);
        foreach (var item in actors)
        {
            item.ToDefault();
        }
        controllerProcessor.inDialog = false;
        controllerProcessor.ReturnFromDialogue();
        controllerProcessor.OnChooseAnswer -= CheckAnswer;
    }
    private bool FindController(DialogueCharacter character)
    {
        for (int i = 0; i < actors.Count; i++)
        {
            if(actors[i].dialogueCharacter == character)
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
    private void CheckVariant(AnswerUI answer, DialogueNodePack node)
    {
        if(!node.replicText.Equals(string.Empty))
        {
            answer.variantPanel.SetActive(true);
            answer.variantText.color = node.character.color;
            answer.variantText.text = node.replicText;
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
        if(debug)
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

[System.Serializable]
public class AnswerUI
{
    [Tooltip("Панель для варианта")] public GameObject variantPanel;
    [Tooltip("Текст варианта")] public Text variantText;
}
