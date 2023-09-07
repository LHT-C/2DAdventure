using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class PlayerController : MonoBehaviour
{
    public PlayerInputControl inputControl;//调用input control中生成的PlayerInputControl方法
    public Rigidbody2D rb;//刚体变量，可在unity中指定对应刚体
    //public SpriteRenderer sr;
    private PhysicsCheck physicsCheck;//链接到PhysicsCheck脚本
    private PlayerAnimation playerAnimation;//链接到playerAnimation脚本
    private CapsuleCollider2D coll;//链接到CapsuleCollider2D脚本

    public Vector2 inputDirection;//接收控制器输入信息的变量

    [Header("基本参数")]//特性，方便在unity中查看
    public float speed;//速度变量，可在unity中输入数值
    public float jumpForce;//跳跃力度，可在unity中输入数值

    [Header("受伤反馈")]
    public bool isHurt;//受伤判断
    public float hurtForce;//受伤反弹力

    [Header("状态")]
    public bool isDead;//死亡判断
    public bool isAttack;//攻击判断

    [Header("物理材质")]
    public PhysicsMaterial2D normal;
    public PhysicsMaterial2D smooth;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();//获取自身的Rigidbody2D组件
        physicsCheck = GetComponent<PhysicsCheck>();//获得PhysicsCheck中的所有公开方法
        playerAnimation = GetComponent<PlayerAnimation>();//获得PlayerAnimation中的所有公开方法
        coll = GetComponent<CapsuleCollider2D>();//

        inputControl = new PlayerInputControl();//实例化

        inputControl.Gameplay.Jump.started += Jump;//读取input system中gameplay的jump，事件注册

        inputControl.Gameplay.Attack.started += PlayerAttack;//读取input system中gameplay的Attack，事件注册
    }

    private void OnEnable()
    {
        inputControl.Enable();//启动(先于start)

    }

    private void OnDisable()
    {
        inputControl.Disable();//关闭
    
    }

    private void Update()
    {
        inputDirection = inputControl.Gameplay.Move.ReadValue<Vector2>();//读取maps中gameplay的move

        CheckState();
    }

    private void FixedUpdate()//刚体移动通常在这里执行
    {
        if (!isHurt && !isAttack)
            Move();
    }

    public void Move()//移动
    {
        rb.velocity = new Vector2(inputDirection.x * speed * Time.deltaTime, rb.velocity.y);//y轴的加速度保持原有（即g）

        int faceDir=(int)transform.localScale.x;//朝向
        if (inputDirection.x > 0)
            faceDir = 1;
        if (inputDirection.x < 0)
            faceDir = -1;
            //sr.flipX = true;//人物翻转
       
        transform.localScale = new Vector3(faceDir, 1, 1);//人物翻转
        
    }

    private void Jump(InputAction.CallbackContext obj)//跳跃
    {
        //Debug.Log("jump!");
        if(physicsCheck.isGround)
            rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
    }

    private void PlayerAttack(InputAction.CallbackContext obj)//攻击
    {
        ///if (!physicsCheck.isGround)//跳跃时不能攻击
        ///    return;
        isAttack = true;
        playerAnimation.PlayAttack();//触发攻击动画
    }

    #region UnityEvent
    public void GetHurt(Transform attacker)//角色受伤后反弹（事件都用character.cs触发，unity中也可见）
    {
        isHurt = true;
        rb.velocity = Vector2.zero;//将xy轴的速度变成0
        Vector2 dir = new Vector2((transform.position.x - attacker.position.x), 0).normalized;//得到反弹方向，并归一化
        rb.AddForce(dir*hurtForce, ForceMode2D.Impulse);//施加一个瞬时的反弹力
    }

    public void PlayerDead()//角色死亡
    {
        isDead = true;
        inputControl.Gameplay.Disable();//关闭所有游戏内的操作输入
    }
    #endregion

    private void CheckState()//检查角色所在位置状态（地面/空中）
    {
        coll.sharedMaterial = physicsCheck.isGround ? normal : smooth;//三元判断
    }
}
