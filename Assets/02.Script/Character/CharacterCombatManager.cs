using UnityEngine;

namespace TSG
{
    public class CharacterCombatManager : MonoBehaviour
    {
        [Header("공격 타겟")]
        public CharacterManager currentTarget;

        [Header("공격 방식")]
        public AttackType currentAttackType;

        [Header("락온 전환")]
        public Transform lockOnTransform;
        protected virtual void Awake()
        {
            
        }
    }    
}
