using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations;


namespace TSG
{
    [CreateAssetMenu(menuName = "A.I/States/Pursue Target")]
    public class PursueTargetState : AIState
    {
        public override AIState Tick(AICharacterManager aiCharacter)
        {
            // 액션을 할지 말지 확인(만약 한다면 액션 끝날때까지 아무것도 안하기)
            if (aiCharacter.isPerformingAction)
            {
                return this;
            }
            
            // 타겟이 Null인지 아닌지 확인. 만약 타겟이 없다면, Idle상태로 돌아가기
            if(aiCharacter.aiCharacterCombatManager.currentTarget == null)
            {
                return SwitchState(aiCharacter, aiCharacter.idle);
            }

            // Navmesh Agent가 활성화 되었는지 확인, 아니라면 활성화 되지 않음
            if (!aiCharacter.navMeshAgent.enabled)
            {
                aiCharacter.navMeshAgent.enabled = true;
            }

            // 만약 타겟이 캐릭터의 시야에서 벗어났다면 그들을 마주하게 변경
            if(aiCharacter.aiCharacterCombatManager.viewableAngle < aiCharacter.aiCharacterCombatManager.minimumFOV 
            || aiCharacter.aiCharacterCombatManager.viewableAngle > aiCharacter.aiCharacterCombatManager.maximumFOV)
            {
                aiCharacter.aiCharacterCombatManager.PivotTowardsTarget(aiCharacter);
            }
            
            aiCharacter.aiCharacterLocomotionManager.RotateTowardsAgent(aiCharacter);

            // 추적 상태에서 공격 상태로 전환하는 첫번째 방법
            // if(aiCharacter.aiCharacterCombatManager.distanceFromTarget <= aiCharacter.combatStance.maximumEngagementDistance)
            // {
            //     return SwitchState(aiCharacter, aiCharacter.combatStance);
            // }

            // 추적 상태에서 공격 상태로 전환하는 두번째 방법
            if(aiCharacter.aiCharacterCombatManager.distanceFromTarget <= aiCharacter.navMeshAgent.stoppingDistance)
            {
                return SwitchState(aiCharacter, aiCharacter.combatStance);
            }

            // 만약 타겟이 닿을 수 없는 거리고, 멀리 떨어진다면, 원래 자리로 돌아가기

            // 타겟을 추적

            // 첫번째 방식
            // 비동기(Asynchronistically)로 작동
            // 메인 프레임(게임이 화면을 그리는 메인 루프)과 별개로 백그라운드에서 조금씩 경로를 계산
            // 완벽한 전체 경로가 나올 때까지 기다리는 게 아니라, "대충 이 방향이네" 하고 먼저 움직이면서 뒤로 계속 남은 경로를 계산해 나가는 방식
            //aICharacter.navmeshAgent.SetDestination(aICharacter.aiCharacterCombatManager.currentTarget.transform.position);

            // 두번째 방식
            // 동기(Synchronistically)로 작동
            // 코드가 실행되는 그 즉시(Immediately) 목적지까지의 전체 경로를 한 번에 전부 계산
            // 만약 맵이 엄청나게 복잡하거나 목적지가 너무 멀어서 경로가 길다면, 컴퓨터가 그 순간 수많은 연산을 한 번에 처리해야 함
            // 강의에선 이 방식을 사용한다 함. 강사의 경험 상, 지형이나 길이 이상하면 첫번째 방식은 너무 멍청해진다고 함
            // 다만 둘 다 사용해보고 본인에게 맞는 것을 고르라 하긴 했음
            NavMeshPath path = new NavMeshPath();
            aiCharacter.navMeshAgent.CalculatePath(aiCharacter.aiCharacterCombatManager.currentTarget.transform.position, path);
            aiCharacter.navMeshAgent.SetPath(path);

            return this;
        } 
    }    
}
