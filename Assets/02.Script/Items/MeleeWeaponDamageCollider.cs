using UnityEngine;

namespace TSG
{
    public class MeleeWeaponDamageCollider : DamageCollider
    {
        [Header("공격하는 캐릭터")]
        public CharacterManager characterCausingDamage; // 데미지를 계산할 때, 공격자의 데미지, 효과등을 계산하기 위한 것(modifiers)

        [Header("무기 공격 보정값")]
        public float light_Attack_01_Modifier;

        protected override void Awake()
        {
            base.Awake();

            if(damageCollider == null)
            {
                damageCollider = GetComponent<Collider>();
            }
            damageCollider.enabled = false; // 물리 무기 충돌판정은 시작 부분에서는 비활성화 되어야 하고, 애니메이션만이 동작할때에만 활성화 되어야 함
        }

        protected override void OnTriggerEnter(Collider other)
        {
            Debug.Log($"[충돌] 무언가와 부딪힘: {other.gameObject.name}");
            CharacterManager damageTarget = other.GetComponentInParent<CharacterManager>();

            if(damageTarget != null)
            {
                // 우리 스스로에게 데미지를 주길 바라지 않음
                if(damageTarget == characterCausingDamage)
                {
                    return;
                }
                contactPoint = other.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);

                // 피아식별을 바탕으로 해당 타겟이 공격 가능한 대상인지 확인

                // 타겟이 막고 있나 확인

                // 타겟이 무적인지 확인

                // 데미지 가하기

                DamageTarget(damageTarget);
            }
        }

        protected override void DamageTarget(CharacterManager damageTarget)
        {
            // 같은 타겟에 데미지를 공격 한번에 한번 이상 주지 말 것
            // 그러니 데미지를 주기 전에 확인부터 한번 할 것

            if (charactersDamaged.Contains(damageTarget))
            {
                return;
            }

            charactersDamaged.Add(damageTarget);

            TakeDamageEffect damageEffect = Instantiate(WorldCharacterEffectsManager.instance.takeDamageEffect);
            damageEffect.physicalDamage = physicalDamage;
            damageEffect.magicDamage = magicDamage;
            damageEffect.fireDamage = fireDamage;
            damageEffect.lightningDamage = lightningDamage;
            damageEffect.holyDamage = holyDamage;
            damageEffect.contactPoint = contactPoint;
            damageEffect.angleHitFrom = Vector3.SignedAngle(characterCausingDamage.transform.forward, damageTarget.transform.forward, Vector3.up);

            switch (characterCausingDamage.characterCombatManager.currentAttackType)
            {
                case AttackType.LightAttack01:
                    ApplyAttackDamageModifiers(light_Attack_01_Modifier, damageEffect);
                    break;
                default:
                    break;
            }

            //damageTarget.characterEffectManager.ProcessInstantEffect(damageEffect);

            if (characterCausingDamage.IsOwner)
            {
                // 서버에 데미지 처리 공식을 넘김
                damageTarget.characterNetworkManager.NotifyTheServerOfCharacterDamageServerRpc(
                    damageTarget.NetworkObjectId, 
                    characterCausingDamage.NetworkObjectId,
                    damageEffect.physicalDamage,
                    damageEffect.magicDamage,
                    damageEffect.fireDamage,
                    damageEffect.lightningDamage,
                    damageEffect.holyDamage,
                    damageEffect.poiseDamage,
                    damageEffect.angleHitFrom,
                    damageEffect.contactPoint.x,
                    damageEffect.contactPoint.y,
                    damageEffect.contactPoint.z);
            }
        }

        private void ApplyAttackDamageModifiers(float modifiers, TakeDamageEffect damage)
        {
            damage.physicalDamage *= modifiers;
            damage.magicDamage *= modifiers;
            damage.fireDamage *= modifiers;
            damage.holyDamage *= modifiers;
            damage.poiseDamage *= modifiers;

            // 만약 강공을 풀차지로 공격한다면, 일반 공격 데미지 계산식 후 풀차지 공격 보정만큼 곱할것
        }
    }
}
