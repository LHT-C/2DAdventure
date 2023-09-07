using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoarChaseState : BaseState
{
    public override void OnEnter(Enemy enemy)
    {
        currentEnemy = enemy;//从基类中得到enemy传入的当前的敌人类型
        Debug.Log("Chase");
        currentEnemy.currentSpeed = currentEnemy.ChaseSpeed;//改变当前敌人速度
        currentEnemy.anim.SetBool("run", true);//播放奔跑动画
    }

    public override void LogicUpdate()
    {
        if (!currentEnemy.physicsCheck.isGround || (currentEnemy.physicsCheck.touchLeftWall && currentEnemy.faceDir.x < 0) || (currentEnemy.physicsCheck.touchRightWall && currentEnemy.faceDir.x > 0))
        {
            //currentEnemy.wait = true;//撞墙或到达悬崖边后，进入等待时间，随后改变行动方向
            //currentEnemy.rb.drag = 1000000;//将摩擦力设为无限大，让怪物停止
            //currentEnemy.currentSpeed = 0;
            //currentEnemy.anim.SetBool("walk", false);
            currentEnemy.transform.localScale = new Vector3(currentEnemy.faceDir.x, 1, 1);//撞墙或到达悬崖边后，立即转身
        }

        if (currentEnemy.lostTimeCounter <= 0)
            currentEnemy.SwitchState(NPCState.Patrol);//丢失玩家的倒计时归零后，回到巡逻状态
    }
    

    public override void PhysicsUpdate()
    {
    }

    public override void OnExit()
    {
        currentEnemy.anim.SetBool("run", false);
    }
}

