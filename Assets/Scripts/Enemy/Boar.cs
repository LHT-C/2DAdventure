using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boar : Enemy
{
    //public override void Move()//继承父类的move来重写方法
    //{
    //    base.Move();//但是保留父类的move方法
    //    anim.SetBool("walk", true);
    //}
    protected override void Awake()
    {
        base.Awake();//执行父类的awake方法的同时，根据父类传来的state执行下面的状态激活方法
        patrolState = new BoarPatrolState();//将巡逻逻辑设置为野猪的巡逻逻辑类
        chaseState = new BoarChaseState();//将追踪逻辑设置为野猪的追踪逻辑类
    }
}
