namespace Enemies.States
{
    public class EnemyIdleState : EnemyBaseState
    {
        public EnemyIdleState(EnemyBase enemy) : base(enemy)
        {
        }

        public override void DoAction()
        {
            // Nothing == Idle
        }
    }
}