//抽象基类
public abstract class BaseState
{
    protected Enemy currentEnemy;//获取当前的敌人类型
    public abstract void OnEnter(Enemy enemy);
    public abstract void LogicUpdate();//基本的逻辑更新
    public abstract void PhysicsUpdate();//物理逻辑判断
    public abstract void OnExit();
}
