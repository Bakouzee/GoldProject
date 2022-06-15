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
            if(!enemy.interrogationPoint.activeInHierarchy)
                enemy.interrogationPoint.SetActive(true);

            var objectWorldPos = PlayerManager.Instance.transform.position;
            if (enemy.IsObjectInSight(objectWorldPos))
            {
                enemy.SetState(onPlayerFoundState);
                return;
            }
            
            turnDuration--;
            if (turnDuration <= 0)
            {
                if (enemy.interrogationPoint.activeInHierarchy)
                    enemy.interrogationPoint.SetActive(false);

                GoToNextState();
            }
        }
    }
}