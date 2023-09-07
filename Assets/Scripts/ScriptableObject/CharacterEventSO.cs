using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Event/CharacterEventSO")]//����unity���Ҽ��½�һ�������ļ�
public class CharacterEventSO : ScriptableObject//��ȫʹ�ô�������¼��ķ�ʽ
{
    public UnityAction<Character> OnEventRaised;//������¼����ɶ�ȡcharacter��

    public void RaiseEvent(Character character)//���������У�ʹ�ø÷����������¼�
    {
        OnEventRaised?.Invoke(character);
    }
}

