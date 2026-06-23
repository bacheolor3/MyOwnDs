using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


namespace TSG
{
    // Combat Stance State 란?
    // 모든 가능한 공격을 훝어본 후, 거리에 기반해 공격과 각도, 무게를 정하는 것
    // 만약 타겟이 너무 멀어진다면, 추적 상태로 전환
    // 타겟이 더는 존재하지 않다면, Idle 상태로 전환, 공격 스위치가 있다면 공격 상태로
    // 공격 준비를 하면서 전투 액션 실행
    // 타겟 근처를 배회/원형을 돌면서 공격을 기다리는 동작

    [CreateAssetMenu(menuName = "A.I/States/Combat Target")]
    public class CombatStanceState : AIState
    {
        // 1. 공격 상태를 위한 공격을 고르기. 거리와 타겟의 각도에 따라 결정
        // 2. 공격을 기다리며 전투 로직을 진행(막기, 견제, 회피 등)
        // 3. 만약 타겟이 전투 사거리를 벗어난다면 추적 상태로 전환
        // 4. 만약 타겟이 더는 존재하지 않는다면, Idle 상태로 전환

        [Header("공격들")]
        public List<AICharacterAttackAction> aICharacterAttacks;    // 이 캐릭터가 할 수 있는 모든 공격의 리스트
        protected List<AICharacterAttackAction> potentialAttacks;     // 현 상황에 맞는(각도, 혹은 거리에 따라서) 모든 공격의 리스트
        private AICharacterAttackAction choosenAttack;
        private AICharacterAttackAction previousAttack;
        protected bool hasAttack = false;

        [Header("콤보")]
        [SerializeField] protected bool canPerformCombo = false;    // 만약 캐릭터가 콤보 공격을 사용할 수 있다면
        [SerializeField] protected int chanceToPerformCombo = 25;    // 이 확률이 캐릭터가 다음 공격으로 콤보를 사용할 확률
        [SerializeField] protected bool hasRolledForComboChance = false;      // 콤보 어택 확률 계산을 했나 안했나 체크용

        [Header("전투 상태 전환 거리")]
        [SerializeField] public float maximumEngagementDistance = 5;     // 추적 상태에 들어가기 위한 최대 거리

        public override AIState Tick(AICharacterManager aiCharacter)
        {
            if (aiCharacter.isPerformingAction)
            {
                return this;
            }

            if (!aiCharacter.navMeshAgent.enabled)
            {
                aiCharacter.navMeshAgent.enabled = true;
            }


            // 만약 AI 캐릭터가 타겟을 향해 AI 캐릭터의 시야 밖에서부터 얼굴을 맞대며 돌아서길 바란다면 이걸 포함하도록
            if (!aiCharacter.aiCharacterNetworkManager.isMoving.Value)
            {
                if(aiCharacter.aiCharacterCombatManager.viewableAngle <- 30 || aiCharacter.aiCharacterCombatManager.viewableAngle > 30)
                {
                    aiCharacter.aiCharacterCombatManager.PivotTowardsTarget(aiCharacter);
                }
            }

            aiCharacter.aiCharacterCombatManager.RotateTowardsAgent(aiCharacter);

            // 만약 타겟이 더는 존재하지 않는다면, Idle상태로 전환
            if(aiCharacter.aiCharacterCombatManager.currentTarget == null)
            {
                return SwitchState(aiCharacter, aiCharacter.idle);
            }

            // 만약 가지고 있는 공격이 없다면, 하나 가져올 것
            if (!hasAttack)
            {
                GetNewAttack(aiCharacter);
            }
            else
            {
                aiCharacter.attack.currentAttack = choosenAttack;
                // 콤보 찬스를 위한 랜덤 체크
                return SwitchState(aiCharacter, aiCharacter.attack);
            }

            if(aiCharacter.aiCharacterCombatManager.distanceFromTarget > maximumEngagementDistance)
            {
                return SwitchState(aiCharacter, aiCharacter.pursueTarget);
            }

            NavMeshPath path = new NavMeshPath();
            aiCharacter.navMeshAgent.CalculatePath(aiCharacter.aiCharacterCombatManager.currentTarget.transform.position, path);
            aiCharacter.navMeshAgent.SetPath(path);

            return this;
        }

        protected virtual void GetNewAttack(AICharacterManager aICharacter)
        {
            potentialAttacks = new List<AICharacterAttackAction>();

            foreach(var potentialAttack in aICharacterAttacks)
            {
                // 만약 이 공격을 하기에 너무 가깝다면, 다음 공격으로 체크
                if(potentialAttack.minimumAttackDistance > aICharacter.aiCharacterCombatManager.distanceFromTarget)
                {
                    continue;
                }
                // 만약 이 공격을 하기에 너무 멀다면, 다음 공격으로 체크
                if(potentialAttack.maximumAttackDistance < aICharacter.aiCharacterCombatManager.distanceFromTarget)
                {
                    continue;
                }

                // 만약 타겟이 이 공격을 하기에 최소한의 시야각에서 벗어났다면, 다음 공격으로 체크
                if(potentialAttack.minimumAttackAngle > aICharacter.aiCharacterCombatManager.viewableAngle)
                {
                    continue;
                }

                // 만약 타겟이 이 공격을 하기에 최대한의 시야각에서 벗어났다면, 다음 공격으로 체크
                if(potentialAttack.maximumAttackAngle < aICharacter.aiCharacterCombatManager.viewableAngle)
                {
                    continue;
                }

                potentialAttacks.Add(potentialAttack);
            }

            if(potentialAttacks.Count <= 0)
            {
                return;
            }

            var totalWeight = 0;

            foreach(var attack in potentialAttacks)
            {
                totalWeight += attack.attackWeight;
            }

            var randomWeightValue = Random.Range(1, totalWeight + 1);
            var processedWeight = 0;

            foreach(var attack in potentialAttacks)
            {
                processedWeight += attack.attackWeight;

                if(randomWeightValue <= processedWeight)
                {
                    // 이게 실행될 공격
                    choosenAttack = attack;
                    previousAttack = choosenAttack;
                    hasAttack = true;
                    return;
                }
            }

            // 3. 남아있는 공격을 리스트에 추가
            // 4. 그 남아있는 리스트에서 공격을 정함. 무게(Weight)에 기반해서
            // 5. 선택한 공격을 공격 상태로 전달
        }

        protected virtual bool RollForOutcomeChance(int outcomeChance)
        {
            bool outcomeWillBePerformed = false;

            int randomPercentage = Random.Range(0, 100);

            if(randomPercentage < outcomeChance)
            {
                outcomeWillBePerformed = true;
            }

            return outcomeWillBePerformed;
        }

        protected override void ResetStateFlags(AICharacterManager aICharacter)
        {
            base.ResetStateFlags(aICharacter);

            hasAttack = false;
            hasRolledForComboChance = false;
        }
    }    
}
