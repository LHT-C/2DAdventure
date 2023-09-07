using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Event/CharacterEventSO")]//可在unity中右键新建一个该类文件
public class CharacterEventSO : ScriptableObject//完全使用代码控制事件的方式
{
    public UnityAction<Character> OnEventRaised;//激活该事件即可读取character类

    public void RaiseEvent(Character character)//在其他类中，使用该方法来激活事件
    {
        OnEventRaised?.Invoke(character);
    }
}

