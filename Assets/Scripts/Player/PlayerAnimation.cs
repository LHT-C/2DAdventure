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
        anim.SetFloat("velocityX", Mathf.Abs(rb.velocity.x));//���x�߷����ϵ��ٶȾ���ֵ
        anim.SetFloat("velocityY", rb.velocity.y);//���y�߷����ϵ��ٶȾ���ֵ
        anim.SetBool("isGround", physicsCheck.isGround);//��õ���״̬
        anim.SetBool("isDead", playerController.isDead);//��playerController�л�ý�ɫ����״̬������animator�е�isDead
        anim.SetBool("isAttack", playerController.isAttack);
    }

    public void PlayHurt()
    {
        anim.SetTrigger("hurt");//����animator�е�hurt
    }

    public void PlayAttack()
    {
        anim.SetTrigger("attack");//����animator�е�attack
    }
}
