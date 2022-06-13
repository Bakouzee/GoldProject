using Unity.VisualScripting;

namespace Enemies.States
{
    public class EnemyIdleState : EnemyBaseState
    {
        private int idleTurnCount;
        
        public EnemyIdleState(EnemyBase enemy, int idleTurnCount) : base(enemy)
        {
            this.idleTurnCount = idleTurnCount;
        }

        public override void DoAction()
        {
            idleTurnCount--;
            if (idleTurnCount <= 0)
            {
                
            }
            // Nothing == Idle
        }
    }
}