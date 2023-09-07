using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public PlayerStatBar playerStatBar;

    [Header("�¼�����")]
    public CharacterEventSO healthEvent;//����Ѫ���仯�¼�

    private void OnEnable()//ע���¼�
    {
        healthEvent.OnEventRaised += OnHealthEvent;
    }

    private void OnDisable()//ע���¼�
    {
        healthEvent.OnEventRaised -= OnHealthEvent;
    }

    private void OnHealthEvent(Character character)
    {
        var persentage = character.currenHealth / character.maxHealth;//����Ѫ���ٷֱ�
        playerStatBar.OnHealthChange(persentage);//������İٷֱȴ���PlayerStatBar
    }
}
