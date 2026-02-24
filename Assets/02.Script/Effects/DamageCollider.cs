using System.Collections.Generic;
using UnityEngine;

namespace TSG
{
    public class DamageCollider : MonoBehaviour
    {
        [Header("충돌판정")]
        protected Collider damageCollider;

        [Header("데미지")]
        public float physicalDamage = 0;    // (미래에는 "기본", "충격", "참격" 그리고 "관통"으로 나눌 것)
        public float magicDamage = 0;
        public float fireDamage = 0;
        public float lightningDamage = 0;
        public float holyDamage = 0;

        [Header("충돌 지점")]
        private Vector3 contactPoint;

        [Header("데미지를 받고 있는 캐릭터")]
        protected List<CharacterManager> charactersDamaged = new List<CharacterManager>();

        private void OnTriggerEnter(Collider other)
        {
            Debug.Log($"{other.gameObject.name}와 충돌함!"); // 이 로그조차 안 찍히면 물리 설정 문제
            CharacterManager damageTarget = other.GetComponentInParent<CharacterManager>();
            // 만약 데미지를 줄 수 있는 캐릭터 콜라이더와 캐릭터 콜라이더를 전부 확인해보고 싶다면
            // if(damageTarget == null)
            // {
            //     damageTarget = other.GetComponent<CharacterManager>();
            // }

            // 충돌하는 오브젝트가 캐릭터인지 확인
            // if(other.gameObject.layer == LayerMask.NameToLayer("Character"))
            // {
                
            // }

            if(damageTarget != null)
            {
                contactPoint = other.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);

                // 피아식별을 바탕으로 해당 타겟이 공격 가능한 대상인지 확인

                // 타겟이 막고 있나 확인

                // 타겟이 무적인지 확인

                // 데미지 가하기

                DamageTarget(damageTarget);
            }
        }

        protected virtual void DamageTarget(CharacterManager damageTarget)
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
            damageEffect.holyDamage = holyDamage;
            damageEffect.contactPoint = contactPoint;

            damageTarget.characterEffectManager.ProcessInstantEffect(damageEffect);
        }

        public virtual void EnableDamageCollider()
        {
            damageCollider.enabled = true;
        }

        public virtual void DisableDamageCollider()
        {
            damageCollider.enabled = false;
            charactersDamaged.Clear();      // 캐릭터가 공격받을 때, 충돌 판정을 리셋함과 동시에 캐릭터도 리셋함. 그래야 다시 공격할 수 있으니까
        }
    }    
}
