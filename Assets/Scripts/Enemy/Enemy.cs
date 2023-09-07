using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Rigidbody2D rb;//protected��ֻ���ڲ���������Է���
    [HideInInspector]public Animator anim;//��unity������ĳЩ����Ҫ�Ĵ���
    [HideInInspector]public PhysicsCheck physicsCheck;

    [Header("��������")]
    public float normalSpeed;
    public float ChaseSpeed;
    public float currentSpeed;
    public Vector3 faceDir;//�����泯�ķ���
    public float hurtForce;
    public Transform attacker;//�����ߣ���һ���أ�

    [Header("���")]
    public Vector2 centerOffset;//���ĵ��λ�Ʋ�ֵ
    public Vector2 checkSize;//��ⷶΧ
    public float checkDistance;//������
    public LayerMask attackLayer;//���ͼ��

    [Header("��ʱ�����")]
    public float waitTime;
    public float waitTimeCounter;
    public bool wait;
    public float lostTime;
    public float lostTimeCounter;

    [Header("״̬")]
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
        currentState = patrolState;//��ʼʱ����״̬��ΪѲ��״̬�ࣨ�������ֵ��˵�Ѳ��״̬�������������ã�
        currentState.OnEnter(this);//������д��뵱ǰ��������
    }

    private void Update()
    {
        faceDir = new Vector3(-transform.localScale.x, 0, 0);//��transform�л�÷���
        ////Ѳ��״̬
        //if ((physicsCheck.touchLeftWall && faceDir.x < 0) || (physicsCheck.touchRightWall && faceDir.x > 0))
        //{
        //    wait = true;//ײǽ��,����ȴ�ʱ�䣬���ı��ж�����
        //    anim.SetBool("walk", false);
        //}
        currentState.LogicUpdate();//update�У�ִ�е�ǰ״̬�µ��߼�����
        TimeCounter();
    }

    private void FixedUpdate()
    {
        if (!isHurt && !isDead && !wait) 
            Move();
        currentState.PhysicsUpdate();//fixupdate�У�ִ�������ж��߼�
    }

    private void OnDisable()
    {
        currentState.OnExit();//����������״̬��Ϊ�˳�
    }

    public virtual void Move()//�ƶ�����ģ�����ͨ����������޸�
    {
        rb.velocity = new Vector2(currentSpeed * faceDir.x * Time.deltaTime, rb.velocity.y);//y��ļ��ٶȱ���ԭ�У���g��
    }

    public void TimeCounter()//��ʱ��
    {
        if(wait)//�ȴ���ײǽ�����±ߣ���ʱ��
        {
            waitTimeCounter -= Time.deltaTime;
        }
        if(!FoundPlayer() && lostTimeCounter > 0)//��ʧĿ���ʱ��
        {
            lostTimeCounter -= Time.deltaTime;
        }
        else if(FoundPlayer())
        {
            lostTimeCounter = lostTime;
        }
    }

    public bool FoundPlayer()//����
    {
        return Physics2D.BoxCast(transform.position + (Vector3)centerOffset, checkSize, 0, faceDir, checkDistance, attackLayer);//����һ�����ε��ж��������ĳһͼ�㣬Ϊ0��ֵ�ǽǶȣ����ص���һ������ֵ���л��ޣ�
    }

    public void SwitchState(NPCState state)//״̬�л�(ö��)
    {
        var newState = state switch//����һ���µ�state�����յ�ǰ��state������switch���л����﷨�ǣ��򵥵�״̬�л�
        {
            NPCState.Patrol => patrolState,
            NPCState.Chase => chaseState,
            _ => null
        };
        currentState.OnExit();//�˳���״̬
        currentState = newState;//��״̬תΪ�µ�state
        currentState.OnEnter(this);//������״̬(��������ִ�ж�Ӧ��������state�ļ�������)
    }

    #region �¼�ִ�з���
    public void OnTakeDamage(Transform attackTrans)//����ʱ����
    {
        attacker = attackTrans;
        if (attackTrans.position.x - transform.position.x > 0)//�ܻ���ת��
            transform.localScale = new Vector3(-1, 1, 1);
        if (attackTrans.position.x - transform.position.x < 0)
            transform.localScale = new Vector3(1, 1, 1);

        isHurt = true;
        anim.SetTrigger("hurt");
        Vector2 dir = new Vector2(transform.position.x - attackTrans.position.x, 0).normalized;//��¼�ܻ�����
        rb.velocity = new Vector2(0, rb.velocity.y);//�ܻ�ʱ������ǰ�ٶȸ�Ϊ0
        StartCoroutine(OnHurt(dir));//����Э��
    }

    private IEnumerator OnHurt(Vector2 dir)//Э�̣�Эͬ����,ʹ�����ܹ���ִ�У����ص���һ��������
    {
        rb.AddForce(dir * hurtForce, ForceMode2D.Impulse);//ʩ�ӵ�������
        yield return new WaitForSeconds(0.45f);//�ȴ�0.45���ִ����һ�����
        isHurt = false;
    }

    public void OnDie()//����ʱ����
    {
        gameObject.layer = 2;//�޸�ͼ�㣬ʹʬ�岻����Ҳ�����ײ
        anim.SetBool("dead", true);
        isDead = true;
    }

    public void DestroyAfterAnimation()//�����������������ٸõ���
    {
        Destroy(this.gameObject);
    }
    #endregion

    private void OnDrawGizmosSelected()//����׷����Χ
    {
        Gizmos.DrawWireSphere(transform.position + (Vector3)centerOffset + new Vector3(checkDistance * -transform.localScale.x, 0), 0.2f);
    }
}
