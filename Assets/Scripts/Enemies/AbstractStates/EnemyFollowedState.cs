using UnityEngine.Windows.WebCam;

namespace Enemies.States
{
    /// <summary>
    /// Class meant to be inherited from.
    /// Allow an EnemyBaseState to force the enemy
    /// switch to a new state
    /// </summary>
    public abstract class EnemyFollowedState : EnemyBaseState
    {
        protected EnemyBaseState nextState;
        
        public EnemyFollowedState(EnemyBase enemy, EnemyBaseState nextState) : base(enemy)
        {
            this.nextState = nextState;
        }
        
        /// <summary>Method used to force the enemy to switch to the new state</summary>
        protected void GoToNextState()
        {
            enemy.SetState(nextState);
        }
    }
}