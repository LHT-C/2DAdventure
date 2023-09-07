using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class BoarPatrolState : BaseState//继承自基类
{

    public override void OnEnter(Enemy enemy)
    {
        currentEnemy = enemy;//从基类中得到enemy传入的当前的敌人类型
        currentEnemy.currentSpeed = currentEnemy.normalSpeed;//将速度切换到巡逻速度（比如从追击状态回到巡逻状态时）
    }

    public override void LogicUpdate()
    {
        //巡逻状态
        if (!currentEnemy.physicsCheck.isGround || (currentEnemy.physicsCheck.touchLeftWall && currentEnemy.faceDir.x < 0) || (currentEnemy.physicsCheck.touchRightWall && currentEnemy.faceDir.x > 0))
        {
            currentEnemy.wait = true;//撞墙或到达悬崖边后，进入等待时间，随后改变行动方向
            currentEnemy.rb.drag = 1000000;//将摩擦力设为无限大，让怪物停止
            currentEnemy.currentSpeed = 0;
            currentEnemy.anim.SetBool("walk", false);
        }
        else
        {
            currentEnemy.anim.SetBool("walk", true);
        }

        if (currentEnemy.waitTimeCounter <= 0)//等待计时结束后，转向继续行走
        {
            currentEnemy.wait = false;
            currentEnemy.waitTimeCounter = currentEnemy.waitTime;
            currentEnemy.transform.localScale = new Vector3(currentEnemy.faceDir.x, 1, 1);//转向
            currentEnemy.rb.drag = 0;//恢复摩擦力
            currentEnemy.currentSpeed = currentEnemy.normalSpeed;//速度恢复
        }

        //发现player时switchstate切换到chase状态
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
