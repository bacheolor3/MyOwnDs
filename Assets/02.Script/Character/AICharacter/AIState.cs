using UnityEngine;

namespace TSG
{
    public class AIState : ScriptableObject
    {
        public virtual AIState Tick(AICharacterManager aICharacter)
        {
            Debug.Log("WE ARE RUNNING THIS STATE");

            // 플레이어 찾기 위한 로직

            // 만약 플레이어를 찾았으면, 타겟을 추적하는 상태로 전환

            // 플레이어를 못 찾았다면, 가만히 있는(IDLE)상태로 전환
            return this;
        }
    }    
}
