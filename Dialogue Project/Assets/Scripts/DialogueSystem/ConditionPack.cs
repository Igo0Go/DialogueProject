using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(menuName = "IgoGoTools / Condition")]
public class ConditionPack : ScriptableObject
{
    public List<ScenarioCondition> conditions = new List<ScenarioCondition>();

    public bool GetType(int number, out ConditionType checkType)
    {
        checkType = ConditionType.Bool;
        if (FindCondition(number, out ScenarioCondition condition))
        {
            checkType = condition.type;
            return true;
        }
        else
        {
            Debug.LogError("В пакете условий " + this.name + " не найдено условие с именем " + name + ". Проверьте написание");
        }
        return false;
    }
    public bool Check(string conditionName, CheckType checkType, bool value)
    {
        if(FindCondition(conditionName, out ScenarioCondition condition))
        {
            switch(checkType)
            {
                case CheckType.Equlas:
                    return condition.boolValue == value;
                case CheckType.NotEqulas:
                    return condition.boolValue != value;
            }
            Debug.LogError("В пакет условий " + this.name + " передан не верный тип проверки " + checkType.ToString());
        }
        Debug.LogError("В пакете условий " + this.name + " не найдено условие с именем " + name + ". Проверьте написание");
        return false;
    }
    public bool Check(string conditionName, CheckType checkType, int value)
    {
        if (FindCondition(conditionName, out ScenarioCondition condition))
        {
            switch (checkType)
            {
                case CheckType.Equlas:
                    return condition.intValue == value;
                case CheckType.NotEqulas:
                    return condition.intValue != value;
                case CheckType.Greate:
                    return condition.intValue > value;
                case CheckType.Less:
                    return condition.intValue < value;
            }
            Debug.LogError("В пакет условий " + this.name + " передан не верный тип проверки " + checkType.ToString());
        }
        Debug.LogError("В пакете условий " + this.name + " не найдено условие с именем " + name + ". Проверьте написание");
        return false;
    }
    public bool Check(int conditionNumber, CheckType checkType, bool value)
    {
        if (FindCondition(conditionNumber, out ScenarioCondition condition))
        {
            switch (checkType)
            {
                case CheckType.Equlas:
                    return condition.boolValue == value;
                case CheckType.NotEqulas:
                    return condition.boolValue != value;
            }
            Debug.LogError("В пакет условий " + this.name + " передан не верный тип проверки " + checkType.ToString());
        }
        Debug.LogError("В пакете условий " + this.name + " не найдено условие с именем " + name + ". Проверьте написание");
        return false;
    }
    public bool Check(int conditionNumber, CheckType checkType, int value)
    {
        if (FindCondition(conditionNumber, out ScenarioCondition condition))
        {
            switch (checkType)
            {
                case CheckType.Equlas:
                    return condition.intValue == value;
                case CheckType.NotEqulas:
                    return condition.intValue != value;
                case CheckType.Greate:
                    return condition.intValue > value;
                case CheckType.Less:
                    return condition.intValue < value;
            }
            Debug.LogError("В пакет условий " + this.name + " передан не верный тип проверки " + checkType.ToString());
        }
        Debug.LogError("В пакете условий " + this.name + " не найдено условие с именем " + name + ". Проверьте написание");
        return false;
    }
    public void SetBool(string name, bool value)
    {
        if (FindCondition(name, out ScenarioCondition condition))
        {
            condition.boolValue = value;
        }
        else
        {
            Debug.LogError("В пакете условий " + this.name + " не найдено условие с именем " + name + ". Проверьте написание");
        }
    }
    public void SetInt(string name, int value)
    {
        if (FindCondition(name, out ScenarioCondition condition))
        {
            condition.intValue = value;
        }
        else
        {
            Debug.LogError("В пакете условий " + this.name + " не найдено условие с именем " + name + ". Проверьте написание");
        }
    }
    public bool GetBool(string name)
    {
        if (FindCondition(name, out ScenarioCondition condition))
        {
            return condition.boolValue;
        }
        else
        {
            Debug.LogError("В пакете условий " + this.name + " не найдено условие с именем " + name + ". Проверьте написание");
        }
        return false;
    }
    public int GetInt(string name)
    {
        if (FindCondition(name, out ScenarioCondition condition))
        {
            return condition.intValue;
        }
        else
        {
            Debug.LogError("В пакете условий " + this.name + " не найдено условие с именем " + name + ". Проверьте написание");
        }
        return 0;
    }

    public void SetBool(int number, bool value)
    {
        if (FindCondition(number, out ScenarioCondition condition))
        {
            condition.boolValue = value;
        }
        else
        {
            Debug.LogError("В пакете условий " + this.name + " не найдено условие с именем " + name + ". Проверьте написание");
        }
    }
    public void SetInt(int number, int value)
    {
        if (FindCondition(number, out ScenarioCondition condition))
        {
            condition.intValue = value;
        }
        else
        {
            Debug.LogError("В пакете условий " + this.name + " не найдено условие с именем " + name + ". Проверьте написание");
        }
    }
    public bool GetBool(int number)
    {
        if (FindCondition(number, out ScenarioCondition condition))
        {
            return condition.boolValue;
        }
        else
        {
            Debug.LogError("В пакете условий " + this.name + " не найдено условие с именем " + name + ". Проверьте написание");
        }
        return false;
    }
    public int GetInt(int number)
    {
        if (FindCondition(number, out ScenarioCondition condition))
        {
            return condition.intValue;
        }
        else
        {
            Debug.LogError("В пакете условий " + this.name + " не найдено условие с именем " + name + ". Проверьте написание");
        }
        return 0;
    }

    public string[] GetCharacteristic()
    {
        string[] result = new string[conditions.Count];

        for (int i = 0; i < result.Length; i++)
        {
            result[i] = conditions[i].conditionName;
        }
        return result;
    }
    bool FindCondition(string name, out ScenarioCondition condition)
    {
        condition = null;
        foreach (var item in conditions)
        {
            if(item.conditionName.Equals(name))
            {
                condition = item;
                return true;
            }
        }
        return false;
    }
    bool FindCondition(int number, out ScenarioCondition condition)
    {
        condition = null;
        if(conditions.Count >= number+1)
        {
            condition = conditions[number];
            return true;
        }
        return false;
    }

}

[System.Serializable]
public class ScenarioCondition
{
    public ConditionType type;
    public string conditionName;
    public int intValue;
    public bool boolValue;
}
[System.Serializable]
public enum CheckType
{
    Equlas = 0,
    NotEqulas = 1,
    Greate = 2,
    Less = 3
}
[System.Serializable]
public enum ConditionType
{
    Bool,
    Int
}