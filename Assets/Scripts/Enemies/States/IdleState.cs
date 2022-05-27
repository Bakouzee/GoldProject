namespace Enemies.States
{
    public class IdleState : EnemyBaseState
    {
        public IdleState(EnemyBase enemy) : base(enemy)
        {
        }

        public override void DoAction()
        {
            // Nothing == Idle
        }
    }
}