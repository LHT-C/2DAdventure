using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rb;
    private PhysicsCheck physicsCheck;
    private PlayerController playerController;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        physicsCheck = GetComponent<PhysicsCheck>();
        playerController = GetComponent<PlayerController>();
    }

    private void Update()
    {
        SetAnimation();
    }

    private void SetAnimation()
    {
        anim.SetFloat("velocityX", Mathf.Abs(rb.velocity.x));//获得x走方向上的速度绝对值
        anim.SetFloat("velocityY", rb.velocity.y);//获得y走方向上的速度绝对值
        anim.SetBool("isGround", physicsCheck.isGround);//获得地面状态
        anim.SetBool("isDead", playerController.isDead);//从playerController中获得角色死亡状态，传给animator中的isDead
        anim.SetBool("isAttack", playerController.isAttack);
    }

    public void PlayHurt()
    {
        anim.SetTrigger("hurt");//触发animator中的hurt
    }

    public void PlayAttack()
    {
        anim.SetTrigger("attack");//触发animator中的attack
    }
}
