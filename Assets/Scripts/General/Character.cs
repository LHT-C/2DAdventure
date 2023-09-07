using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Character : MonoBehaviour
{
    [Header("基本属性")]
    public float maxHealth;
    public float currenHealth;

    [Header("受伤无敌")]
    public float invulnerableDuration;//初始无敌时间，在unity中输入
    private float invulnerableCounter;//无敌计时
    public bool invulnerable;//是否无敌

    public UnityEvent<Character> OnHealthChange;//将血量变化事件广播出去，在unity中绑定character eventSO
    public UnityEvent<Transform> OnTakeDamage;//受伤时触发的事件，在unity中可以往事件中添加各种方法
    public UnityEvent OnDie;//死亡事件

    private void Start()
    {
        currenHealth = maxHealth;
        OnHealthChange?.Invoke(this);//开局时也刷新血量条
    }

    private void Update()
    {
        if (invulnerable) //无敌计时器
        {
            invulnerableCounter -= Time.deltaTime;
            if (invulnerableCounter <= 0)
            {
                invulnerable = false;
            }
        }
    }

    public void TakeDamage(Attack attacker)//受到攻击时触发
    {
        if (invulnerable)
            return;

        if (currenHealth - attacker.damage > 0)//攻击力大于血量时，正常扣血
        {
            currenHealth -= attacker.damage;
            TriggerInvulnerable();//触发受伤无敌
            OnTakeDamage?.Invoke(attacker.transform);//启动受伤事件，这里传入的参数是攻击者的方位
        }
        else
        {
            currenHealth = 0;//否则将血量变为0
            OnDie?.Invoke();//启动死亡事件
        }
        OnHealthChange?.Invoke(this);//启动血量变化事件
    }

    private void TriggerInvulnerable()//受伤无敌
    {
        if (!invulnerable) 
        {
            invulnerable = true;
            invulnerableCounter = invulnerableDuration;
        }
    }
}
