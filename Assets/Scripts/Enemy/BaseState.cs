//�������
public abstract class BaseState
{
    protected Enemy currentEnemy;//��ȡ��ǰ�ĵ�������
    public abstract void OnEnter(Enemy enemy);
    public abstract void LogicUpdate();//�������߼�����
    public abstract void PhysicsUpdate();//�����߼��ж�
    public abstract void OnExit();
}
