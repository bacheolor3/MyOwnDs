using UnityEngine;

namespace TSG
{
    [CreateAssetMenu(menuName = "A.I/States/Idle")]
    public class IdleState : AIState
    {
        public override AIState Tick(AICharacterManager aiCharacter)
        {

            if(aiCharacter.aiCharacterCombatManager.currentTarget != null)
            {
                return SwitchState(aiCharacter, aiCharacter.pursueTarget);                
            }
            else
            {
                // 이 상태로 다시 돌아오고, 타겟을 다시 수색 (타겟을 찾을 때까지 상태를 이 형태로 고정)
                aiCharacter.aiCharacterCombatManager.FindATargetViaLineOfSight(aiCharacter);                
                return this;
            }
        }
    }
    
}
