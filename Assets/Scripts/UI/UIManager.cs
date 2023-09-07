using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public PlayerStatBar playerStatBar;

    [Header("事件监听")]
    public CharacterEventSO healthEvent;//收听血量变化事件

    private void OnEnable()//注册事件
    {
        healthEvent.OnEventRaised += OnHealthEvent;
    }

    private void OnDisable()//注销事件
    {
        healthEvent.OnEventRaised -= OnHealthEvent;
    }

    private void OnHealthEvent(Character character)
    {
        var persentage = character.currenHealth / character.maxHealth;//计算血量百分比
        playerStatBar.OnHealthChange(persentage);//将算出的百分比传入PlayerStatBar
    }
}
