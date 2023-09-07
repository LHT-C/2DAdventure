using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoarChaseState : BaseState
{
    public override void OnEnter(Enemy enemy)
    {
        currentEnemy = enemy;//�ӻ����еõ�enemy����ĵ�ǰ�ĵ�������
        Debug.Log("Chase");
        currentEnemy.currentSpeed = currentEnemy.ChaseSpeed;//�ı䵱ǰ�����ٶ�
        currentEnemy.anim.SetBool("run", true);//���ű��ܶ���
    }

    public override void LogicUpdate()
    {
        if (!currentEnemy.physicsCheck.isGround || (currentEnemy.physicsCheck.touchLeftWall && currentEnemy.faceDir.x < 0) || (currentEnemy.physicsCheck.touchRightWall && currentEnemy.faceDir.x > 0))
        {
            //currentEnemy.wait = true;//ײǽ�򵽴����±ߺ󣬽���ȴ�ʱ�䣬���ı��ж�����
            //currentEnemy.rb.drag = 1000000;//��Ħ������Ϊ���޴��ù���ֹͣ
            //currentEnemy.currentSpeed = 0;
            //currentEnemy.anim.SetBool("walk", false);
            currentEnemy.transform.localScale = new Vector3(currentEnemy.faceDir.x, 1, 1);//ײǽ�򵽴����±ߺ�����ת��
        }

        if (currentEnemy.lostTimeCounter <= 0)
            currentEnemy.SwitchState(NPCState.Patrol);//��ʧ��ҵĵ���ʱ����󣬻ص�Ѳ��״̬
    }
    

    public override void PhysicsUpdate()
    {
    }

    public override void OnExit()
    {
        currentEnemy.anim.SetBool("run", false);
    }
}

