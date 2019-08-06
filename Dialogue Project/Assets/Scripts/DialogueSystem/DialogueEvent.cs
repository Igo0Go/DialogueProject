using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueEvent
{
    [Tooltip("Параметры из пака условий, которые можно изменить")] public ConditionPack conditionCharacteristic;
    [Tooltip("Отправлять событие в сцену подключенному на месте объекту")] public bool changeCondition;
    [Tooltip("Отправлять событие в сцену подключенному на месте объекту")] public bool inSceneInvoke;
    [Tooltip("Показывать сообщение")] public bool isMessage;
    [Tooltip("Текст сообщения")] public string messageText;
    [Space(10)]
    [Tooltip("Название того параметра в пакете условий, который будет изменён")] public string changeConditionName;
    [Tooltip("Тип того параметра в пакете условий, который будет изменён")] public ConditionType changeConditionType;
    [Tooltip("Новое значение")] public bool changeBoolValue;
    [Tooltip("Добавочное значение для целочисленного параметра (может быть отрицательным)")] public int addIntValue;
}
[System.Serializable]
public abstract class DialogueEventReactor : MonoBehaviour
{
    public abstract void OnEvent();
}
