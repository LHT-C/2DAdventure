using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boar : Enemy
{
    //public override void Move()//�̳и����move����д����
    //{
    //    base.Move();//���Ǳ��������move����
    //    anim.SetBool("walk", true);
    //}
    protected override void Awake()
    {
        base.Awake();//ִ�и����awake������ͬʱ�����ݸ��ഫ����stateִ�������״̬�����
        patrolState = new BoarPatrolState();//��Ѳ���߼�����ΪҰ���Ѳ���߼���
        chaseState = new BoarChaseState();//��׷���߼�����ΪҰ���׷���߼���
    }
}
