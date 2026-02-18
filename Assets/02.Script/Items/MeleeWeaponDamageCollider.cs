using UnityEngine;

namespace TSG
{
    public class MeleeWeaponDamageCollider : DamageCollider
    {
        [Header("공격하는 캐릭터")]
        public CharacterManager characterCausingDamage; // 데미지를 계산할 때, 공격자의 데미지, 효과등을 계산하기 위한 것(modifiers)
    }    
}
