using UnityEngine;
using Unity.Netcode;

namespace TSG
{
    public class CharacterCombatManager : NetworkBehaviour
    {
        CharacterManager character;

        [Header("마지막에 재생된 공격 애니메이션")]
        public string lastAttackAnimationPerformed;

        [Header("공격 타겟")]
        public CharacterManager currentTarget;

        [Header("공격 방식")]
        public AttackType currentAttackType;

        [Header("락온 전환")]
        public Transform lockOnTransform;
        protected virtual void Awake()
        {
            character = GetComponent<CharacterManager>();
        }
        public virtual void SetTarget(CharacterManager newTarget)
        {
            if (character.IsOwner)
            {
                if(newTarget != null)
                {
                    currentTarget = newTarget;
                    character.characterNetworkManager.currentTargetNetworkObjectID.Value = newTarget.GetComponent<NetworkObject>().NetworkObjectId;
                }
                else
                {
                    currentTarget = null;
                }
            }
        }
    }    
}
