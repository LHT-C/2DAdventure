using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Rigidbody2D rb;//protected：只有内部和子类可以访问
    [HideInInspector]public Animator anim;//在unity中隐藏某些不需要的词条
    [HideInInspector]public PhysicsCheck physicsCheck;

    [Header("基本参数")]
    public float normalSpeed;
    public float ChaseSpeed;
    public float currentSpeed;
    public Vector3 faceDir;//怪物面朝的方向
    public float hurtForce;
    public Transform attacker;//攻击者（玩家或机关）

    [Header("检测")]
    public Vector2 centerOffset;//中心点的位移差值
    public Vector2 checkSize;//检测范围
    public float checkDistance;//检测距离
    public LayerMask attackLayer;//检测图层

    [Header("计时器相关")]
    public float waitTime;
    public float waitTimeCounter;
    public bool wait;
    public float lostTime;
    public float lostTimeCounter;

    [Header("状态")]
    public bool isHurt;
    public bool isDead;

    private BaseState currentState;
    protected BaseState patrolState;
    protected BaseState chaseState;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        physicsCheck = GetComponent<PhysicsCheck>();
        currentSpeed = normalSpeed;
        waitTimeCounter = waitTime;
    }

    private void OnEnable()
    {
        currentState = patrolState;//开始时，将状态设为巡逻状态类（具体哪种敌人的巡逻状态，在子类中设置）
        currentState.OnEnter(this);//向基类中传入当前敌人类型
    }

    private void Update()
    {
        faceDir = new Vector3(-transform.localScale.x, 0, 0);//从transform中获得方向
        ////巡逻状态
        //if ((physicsCheck.touchLeftWall && faceDir.x < 0) || (physicsCheck.touchRightWall && faceDir.x > 0))
        //{
        //    wait = true;//撞墙后，,进入等待时间，随后改变行动方向
        //    anim.SetBool("walk", false);
        //}
        currentState.LogicUpdate();//update中，执行当前状态下的逻辑更新
        TimeCounter();
    }

    private void FixedUpdate()
    {
        if (!isHurt && !isDead && !wait) 
            Move();
        currentState.PhysicsUpdate();//fixupdate中，执行物理判断逻辑
    }

    private void OnDisable()
    {
        currentState.OnExit();//敌人消除后，状态设为退出
    }

    public virtual void Move()//移动，虚的，可以通过子类进行修改
    {
        rb.velocity = new Vector2(currentSpeed * faceDir.x * Time.deltaTime, rb.velocity.y);//y轴的加速度保持原有（即g）
    }

    public void TimeCounter()//计时器
    {
        if(wait)//等待（撞墙、悬崖边）计时器
        {
            waitTimeCounter -= Time.deltaTime;
        }
        if(!FoundPlayer() && lostTimeCounter > 0)//丢失目标计时器
        {
            lostTimeCounter -= Time.deltaTime;
        }
        else if(FoundPlayer())
        {
            lostTimeCounter = lostTime;
        }
    }

    public bool FoundPlayer()//索敌
    {
        return Physics2D.BoxCast(transform.position + (Vector3)centerOffset, checkSize, 0, faceDir, checkDistance, attackLayer);//发射一个方形的判断器，检测某一图层，为0的值是角度，返回的是一个布尔值（有或无）
    }

    public void SwitchState(NPCState state)//状态切换(枚举)
    {
        var newState = state switch//定义一个新的state，依照当前的state来进行switch的切换。语法糖，简单的状态切换
        {
            NPCState.Patrol => patrolState,
            NPCState.Chase => chaseState,
            _ => null
        };
        currentState.OnExit();//退出旧状态
        currentState = newState;//将状态转为新的state
        currentState.OnEnter(this);//激活新状态(在子类中执行对应敌人类型state的激活设置)
    }

    #region 事件执行方法
    public void OnTakeDamage(Transform attackTrans)//受伤时触发
    {
        attacker = attackTrans;
        if (attackTrans.position.x - transform.position.x > 0)//受击后转身
            transform.localScale = new Vector3(-1, 1, 1);
        if (attackTrans.position.x - transform.position.x < 0)
            transform.localScale = new Vector3(1, 1, 1);

        isHurt = true;
        anim.SetTrigger("hurt");
        Vector2 dir = new Vector2(transform.position.x - attackTrans.position.x, 0).normalized;//记录受击方向
        rb.velocity = new Vector2(0, rb.velocity.y);//受击时，将当前速度改为0
        StartCoroutine(OnHurt(dir));//启动协程
    }

    private IEnumerator OnHurt(Vector2 dir)//协程（协同程序）,使程序能够逐步执行，返回的是一个迭代器
    {
        rb.AddForce(dir * hurtForce, ForceMode2D.Impulse);//施加弹开的力
        yield return new WaitForSeconds(0.45f);//等待0.45秒后执行下一条语句
        isHurt = false;
    }

    public void OnDie()//死亡时触发
    {
        gameObject.layer = 2;//修改图层，使尸体不和玩家产生碰撞
        anim.SetBool("dead", true);
        isDead = true;
    }

    public void DestroyAfterAnimation()//死亡动画结束后，销毁该敌人
    {
        Destroy(this.gameObject);
    }
    #endregion

    private void OnDrawGizmosSelected()//绘制追击范围
    {
        Gizmos.DrawWireSphere(transform.position + (Vector3)centerOffset + new Vector3(checkDistance * -transform.localScale.x, 0), 0.2f);
    }
}
