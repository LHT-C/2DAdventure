using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsCheck : MonoBehaviour
{
    private CapsuleCollider2D coll;

    [Header("检测参数")]
    public bool manual;//手动检测
    public Vector2 bottomOffset;//角色脚底的位移差值
    public Vector2 leftOffset;
    public Vector2 rightOffset;
    public float checkRaduis;//检测范围变量
    public LayerMask groundLayer;//地面层，在unity中以下拉菜单形式出现

    [Header("状态")]
    public bool isGround;//判断角色是否处于地面的变量
    public bool touchLeftWall;
    public bool touchRightWall;//判断撞墙

    private void Awake()
    {
        coll = GetComponent<CapsuleCollider2D>();

        if(!manual)//自动设置位移差值
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
        //检测地面
        isGround = Physics2D.OverlapCircle((Vector2)transform.position + bottomOffset * transform.localScale, checkRaduis, groundLayer);//*transform.localScale是为了让监测点跟着怪物转向一起翻转
        //墙体判断
        touchLeftWall = Physics2D.OverlapCircle((Vector2)transform.position + leftOffset * transform.localScale, checkRaduis, groundLayer);
        touchRightWall = Physics2D.OverlapCircle((Vector2)transform.position + rightOffset * transform.localScale, checkRaduis, groundLayer);
    }

    private void OnDrawGizmosSelected()//绘制范围
    {
        Gizmos.DrawWireSphere((Vector2)transform.position + bottomOffset * transform.localScale, checkRaduis);
        Gizmos.DrawWireSphere((Vector2)transform.position + leftOffset * transform.localScale, checkRaduis);
        Gizmos.DrawWireSphere((Vector2)transform.position + rightOffset * transform.localScale, checkRaduis);
    }
}
