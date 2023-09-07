using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class BoarPatrolState : BaseState//�̳��Ի���
{

    public override void OnEnter(Enemy enemy)
    {
        currentEnemy = enemy;//�ӻ����еõ�enemy����ĵ�ǰ�ĵ�������
        currentEnemy.currentSpeed = currentEnemy.normalSpeed;//���ٶ��л���Ѳ���ٶȣ������׷��״̬�ص�Ѳ��״̬ʱ��
    }

    public override void LogicUpdate()
    {
        //Ѳ��״̬
        if (!currentEnemy.physicsCheck.isGround || (currentEnemy.physicsCheck.touchLeftWall && currentEnemy.faceDir.x < 0) || (currentEnemy.physicsCheck.touchRightWall && currentEnemy.faceDir.x > 0))
        {
            currentEnemy.wait = true;//ײǽ�򵽴����±ߺ󣬽���ȴ�ʱ�䣬���ı��ж�����
            currentEnemy.rb.drag = 1000000;//��Ħ������Ϊ���޴��ù���ֹͣ
            currentEnemy.currentSpeed = 0;
            currentEnemy.anim.SetBool("walk", false);
        }
        else
        {
            currentEnemy.anim.SetBool("walk", true);
        }

        if (currentEnemy.waitTimeCounter <= 0)//�ȴ���ʱ������ת���������
        {
            currentEnemy.wait = false;
            currentEnemy.waitTimeCounter = currentEnemy.waitTime;
            currentEnemy.transform.localScale = new Vector3(currentEnemy.faceDir.x, 1, 1);//ת��
            currentEnemy.rb.drag = 0;//�ָ�Ħ����
            currentEnemy.currentSpeed = currentEnemy.normalSpeed;//�ٶȻָ�
        }

        //����playerʱswitchstate�л���chase״̬
        if (currentEnemy.FoundPlayer())
        {
            currentEnemy.SwitchState(NPCState.Chase);
        }
    }

    public override void PhysicsUpdate()
    {
        
    }

    public override void OnExit()
    {
        currentEnemy.anim.SetBool("walk", false);
    }
}
