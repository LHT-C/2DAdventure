using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Character : MonoBehaviour
{
    [Header("��������")]
    public float maxHealth;
    public float currenHealth;

    [Header("�����޵�")]
    public float invulnerableDuration;//��ʼ�޵�ʱ�䣬��unity������
    private float invulnerableCounter;//�޵м�ʱ
    public bool invulnerable;//�Ƿ��޵�

    public UnityEvent<Character> OnHealthChange;//��Ѫ���仯�¼��㲥��ȥ����unity�а�character eventSO
    public UnityEvent<Transform> OnTakeDamage;//����ʱ�������¼�����unity�п������¼�����Ӹ��ַ���
    public UnityEvent OnDie;//�����¼�

    private void Start()
    {
        currenHealth = maxHealth;
        OnHealthChange?.Invoke(this);//����ʱҲˢ��Ѫ����
    }

    private void Update()
    {
        if (invulnerable) //�޵м�ʱ��
        {
            invulnerableCounter -= Time.deltaTime;
            if (invulnerableCounter <= 0)
            {
                invulnerable = false;
            }
        }
    }

    public void TakeDamage(Attack attacker)//�ܵ�����ʱ����
    {
        if (invulnerable)
            return;

        if (currenHealth - attacker.damage > 0)//����������Ѫ��ʱ��������Ѫ
        {
            currenHealth -= attacker.damage;
            TriggerInvulnerable();//���������޵�
            OnTakeDamage?.Invoke(attacker.transform);//���������¼������ﴫ��Ĳ����ǹ����ߵķ�λ
        }
        else
        {
            currenHealth = 0;//����Ѫ����Ϊ0
            OnDie?.Invoke();//���������¼�
        }
        OnHealthChange?.Invoke(this);//����Ѫ���仯�¼�
    }

    private void TriggerInvulnerable()//�����޵�
    {
        if (!invulnerable) 
        {
            invulnerable = true;
            invulnerableCounter = invulnerableDuration;
        }
    }
}
