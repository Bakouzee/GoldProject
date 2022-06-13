using Unity.VisualScripting;

namespace Enemies.States
{
    public class EnemyLookState : EnemyFollowedState
    {
        private int turnDuration;
        private EnemyBaseState onPlayerFoundState;
        
        public EnemyLookState(EnemyBase enemy, int maxTurnDuration, EnemyBaseState playerFoundState, EnemyBaseState playerNotFoundState) : base(enemy, playerNotFoundState)
        {
            this.turnDuration = maxTurnDuration;
        }

        public override void DoAction()
        {
            // Look for player
            var objectWorldPos = PlayerManager.Instance.transform.position;
            if (enemy.IsObjectInSight(objectWorldPos))
            {
                enemy.SetState(onPlayerFoundState);
                return;
            }
            
            turnDuration--;
            if (turnDuration <= 0)
            {
                GoToNextState();
            }
        }
    }
}