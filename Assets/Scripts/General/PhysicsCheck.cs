using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsCheck : MonoBehaviour
{
    private CapsuleCollider2D coll;

    [Header("������")]
    public bool manual;//�ֶ����
    public Vector2 bottomOffset;//��ɫ�ŵ׵�λ�Ʋ�ֵ
    public Vector2 leftOffset;
    public Vector2 rightOffset;
    public float checkRaduis;//��ⷶΧ����
    public LayerMask groundLayer;//����㣬��unity���������˵���ʽ����

    [Header("״̬")]
    public bool isGround;//�жϽ�ɫ�Ƿ��ڵ���ı���
    public bool touchLeftWall;
    public bool touchRightWall;//�ж�ײǽ

    private void Awake()
    {
        coll = GetComponent<CapsuleCollider2D>();

        if(!manual)//�Զ�����λ�Ʋ�ֵ
        {
            rightOffset = new Vector2((coll.bounds.size.x + coll.offset.x) / 2, coll.bounds.size.y / 2);
            leftOffset = new Vector2(-rightOffset.x, rightOffset.y);
        }
    }

    private void Update()
    {
        Check();
    }

    public void Check()
    {
        //������
        isGround = Physics2D.OverlapCircle((Vector2)transform.position + bottomOffset * transform.localScale, checkRaduis, groundLayer);//*transform.localScale��Ϊ���ü�����Ź���ת��һ��ת
        //ǽ���ж�
        touchLeftWall = Physics2D.OverlapCircle((Vector2)transform.position + leftOffset * transform.localScale, checkRaduis, groundLayer);
        touchRightWall = Physics2D.OverlapCircle((Vector2)transform.position + rightOffset * transform.localScale, checkRaduis, groundLayer);
    }

    private void OnDrawGizmosSelected()//���Ʒ�Χ
    {
        Gizmos.DrawWireSphere((Vector2)transform.position + bottomOffset * transform.localScale, checkRaduis);
        Gizmos.DrawWireSphere((Vector2)transform.position + leftOffset * transform.localScale, checkRaduis);
        Gizmos.DrawWireSphere((Vector2)transform.position + rightOffset * transform.localScale, checkRaduis);
    }
}
