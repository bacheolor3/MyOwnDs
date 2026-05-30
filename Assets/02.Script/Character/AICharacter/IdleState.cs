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
                // 캐릭터 추적 상태로 전환 (상태를 추적 상태로 변환)
                Debug.Log("WE HAVE A TARGET");
                return this;
            }
            else
            {
                // 이 상태로 다시 돌아오고, 타겟을 다시 수색 (타겟을 찾을 때까지 상태를 이 형태로 고정)
                aiCharacter.aiCharacterCombatManager.FindATargetViaLineOfSight(aiCharacter);
                Debug.Log("SEARCHING FOR TARGET");
                return this;
            }
        }
    }
    
}
