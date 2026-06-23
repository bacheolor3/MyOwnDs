using UnityEngine;


namespace TSG
{
    [CreateAssetMenu(menuName = "A.I/States/Attack")]
    public class AttackState : AIState
    {
        [Header("현재 공격")]
        [HideInInspector] public AICharacterAttackAction currentAttack;
        [HideInInspector] public bool willPerformCombo = false;

        [Header("상태 변환용 플래그들")]
        protected bool hasPerformedAttack = false;
        protected bool hasPerformedCombo = false;

        [Header("공격 후 피봇(에디터 상 위치)")]
        [SerializeField] protected bool pivotAfterAttack = false;

        public override AIState Tick(AICharacterManager aiCharacter)
        {
            if(aiCharacter.aiCharacterCombatManager.currentTarget == null)
            {
                return SwitchState(aiCharacter, aiCharacter.idle);
            }

            if (aiCharacter.aiCharacterCombatManager.currentTarget.isDead.Value)
            {
                return SwitchState(aiCharacter, aiCharacter.idle);
            }
            // 공격 중 타겟을 향해 회전

            aiCharacter.characterAnimatorManager.UpdateAnimatorMovementParameters(0, 0, false);

            // 콤보공격을 실행
            if(willPerformCombo && !hasPerformedCombo)
            {
                if(currentAttack.comboAction != null)
                {
                    // 콤보가 가능하다면
                    // hasPerformedCombo = true;
                    // currentAttack.comboAction.AttemptToPerformAction(aiCharacter);
                }
            }

            if (!hasPerformedAttack)
            {
                // 만약 액션 회복 타이머가 안 지났다면, 다음 공격 하기 전에 다 채워질때까지 기다릴 것
                if(aiCharacter.aiCharacterCombatManager.actionRecoveryTimer > 0)
                {
                    return this;
                }

                if (aiCharacter.isPerformingAction)
                {
                    return this;
                }

                PerformAttack(aiCharacter);

                // 맨 처음 상태로 돌아가야 콤보 공격이 가능한지 아닌지 확인할 수 있음
                return this;
            }

            if (pivotAfterAttack)
            {
                aiCharacter.aiCharacterCombatManager.PivotTowardsTarget(aiCharacter);
            }

            return SwitchState(aiCharacter, aiCharacter.combatStance);
        }

        protected void PerformAttack(AICharacterManager aiCharacter)
        {
            hasPerformedAttack = true;
            currentAttack.AttemptToPerformAction(aiCharacter);
            aiCharacter.aiCharacterCombatManager.actionRecoveryTimer = currentAttack.actionRecoveryTime;
        }

        protected override void ResetStateFlags(AICharacterManager aICharacter)
        {
            base.ResetStateFlags(aICharacter);

            hasPerformedAttack = false;
            hasPerformedCombo = false;
        }
    }    
}
