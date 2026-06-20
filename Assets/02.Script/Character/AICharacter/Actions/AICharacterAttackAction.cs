using UnityEngine;


namespace TSG
{
    [CreateAssetMenu(menuName = "A.I/Actions/Attack")]
    public class AICharacterAttackAction : ScriptableObject
    {
        [Header("공격")]
        [SerializeField] private string attackAnimation;

        [Header("콤보 액션")]
        //public bool actionHasComboAction = false;   // 이 액션이 콤보 액션을 가지고 있다면 (일단 기본값은 null)
        public AICharacterAttackAction comboAction; // 이 공격의 콤보 액션

        [Header("액션 값")]
        public int attackWeight = 50;
        [SerializeField] AttackType attackType;
        // 공격 타입
        // 공격은 반복될 수 있음
        public float actionRecoveryTime = 1.5f;     // 캐릭터가 현재 이 공격을 하고 다른 공격을 하려면 필요한 시간
        public float minimumAttackAngle = -35;
        public float maximumAttackAngle = 35;
        public float minimumAttackDistance = 0;
        public float maximumAttackDistance = 2;

        public void AttemptToPerformAction(AICharacterManager aICharacter)
        {
            aICharacter.characterAnimatorManager.PlayTargetAttackActionAnimation(attackType, attackAnimation, true);
        }
    }    
}
