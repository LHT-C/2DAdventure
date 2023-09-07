using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class PlayerController : MonoBehaviour
{
    public PlayerInputControl inputControl;//����input control�����ɵ�PlayerInputControl����
    public Rigidbody2D rb;//�������������unity��ָ����Ӧ����
    //public SpriteRenderer sr;
    private PhysicsCheck physicsCheck;//���ӵ�PhysicsCheck�ű�
    private PlayerAnimation playerAnimation;//���ӵ�playerAnimation�ű�
    private CapsuleCollider2D coll;//���ӵ�CapsuleCollider2D�ű�

    public Vector2 inputDirection;//���տ�����������Ϣ�ı���

    [Header("��������")]//���ԣ�������unity�в鿴
    public float speed;//�ٶȱ���������unity��������ֵ
    public float jumpForce;//��Ծ���ȣ�����unity��������ֵ

    [Header("���˷���")]
    public bool isHurt;//�����ж�
    public float hurtForce;//���˷�����

    [Header("״̬")]
    public bool isDead;//�����ж�
    public bool isAttack;//�����ж�

    [Header("�������")]
    public PhysicsMaterial2D normal;
    public PhysicsMaterial2D smooth;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();//��ȡ�����Rigidbody2D���
        physicsCheck = GetComponent<PhysicsCheck>();//���PhysicsCheck�е����й�������
        playerAnimation = GetComponent<PlayerAnimation>();//���PlayerAnimation�е����й�������
        coll = GetComponent<CapsuleCollider2D>();//

        inputControl = new PlayerInputControl();//ʵ����

        inputControl.Gameplay.Jump.started += Jump;//��ȡinput system��gameplay��jump���¼�ע��

        inputControl.Gameplay.Attack.started += PlayerAttack;//��ȡinput system��gameplay��Attack���¼�ע��
    }

    private void OnEnable()
    {
        inputControl.Enable();//����(����start)

    }

    private void OnDisable()
    {
        inputControl.Disable();//�ر�
    
    }

    private void Update()
    {
        inputDirection = inputControl.Gameplay.Move.ReadValue<Vector2>();//��ȡmaps��gameplay��move

        CheckState();
    }

    private void FixedUpdate()//�����ƶ�ͨ��������ִ��
    {
        if (!isHurt && !isAttack)
            Move();
    }

    public void Move()//�ƶ�
    {
        rb.velocity = new Vector2(inputDirection.x * speed * Time.deltaTime, rb.velocity.y);//y��ļ��ٶȱ���ԭ�У���g��

        int faceDir=(int)transform.localScale.x;//����
        if (inputDirection.x > 0)
            faceDir = 1;
        if (inputDirection.x < 0)
            faceDir = -1;
            //sr.flipX = true;//���﷭ת
       
        transform.localScale = new Vector3(faceDir, 1, 1);//���﷭ת
        
    }

    private void Jump(InputAction.CallbackContext obj)//��Ծ
    {
        //Debug.Log("jump!");
        if(physicsCheck.isGround)
            rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
    }

    private void PlayerAttack(InputAction.CallbackContext obj)//����
    {
        ///if (!physicsCheck.isGround)//��Ծʱ���ܹ���
        ///    return;
        isAttack = true;
        playerAnimation.PlayAttack();//������������
    }

    #region UnityEvent
    public void GetHurt(Transform attacker)//��ɫ���˺󷴵����¼�����character.cs������unity��Ҳ�ɼ���
    {
        isHurt = true;
        rb.velocity = Vector2.zero;//��xy����ٶȱ��0
        Vector2 dir = new Vector2((transform.position.x - attacker.position.x), 0).normalized;//�õ��������򣬲���һ��
        rb.AddForce(dir*hurtForce, ForceMode2D.Impulse);//ʩ��һ��˲ʱ�ķ�����
    }

    public void PlayerDead()//��ɫ����
    {
        isDead = true;
        inputControl.Gameplay.Disable();//�ر�������Ϸ�ڵĲ�������
    }
    #endregion

    private void CheckState()//����ɫ����λ��״̬������/���У�
    {
        coll.sharedMaterial = physicsCheck.isGround ? normal : smooth;//��Ԫ�ж�
    }
}
