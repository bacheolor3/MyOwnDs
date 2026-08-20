using UnityEngine;


namespace TSG
{
    public class DummyFistDamageCollider : DamageCollider
    {
        [SerializeField] AICharacterManager dummyPunchDamage;

        protected override void Awake()
        {
            base.Awake();

            damageCollider = GetComponent<Collider>();
            dummyPunchDamage = GetComponentInParent<AICharacterManager>();
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
            damageEffect.angleHitFrom = Vector3.SignedAngle(dummyPunchDamage.transform.forward, damageTarget.transform.forward, Vector3.up);


            // 첫번째 방법
            // 호스트 쪽에서 AI 가 공격을 맞추면 클라이언트 쪽에서 어떻게 보이든 데미지가 들어가게 하기

            /*if (dummyPunchDamage.IsOwner)
            {
                // 서버에 데미지 처리 공식을 넘김
                damageTarget.characterNetworkManager.NotifyTheServerOfCharacterDamageServerRpc(
                    damageTarget.NetworkObjectId, 
                    dummyPunchDamage.NetworkObjectId,
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
            }*/

            // 두번째 방법
            // 연결된 캐릭터를 AI가 공격한다면 클라이언트쪽에서 어떻게 보이든 공격이 들어가게 하기
            // 보통은 이 방법을 씀
            if (dummyPunchDamage.IsOwner)
            {
                // 서버에 데미지 처리 공식을 넘김
                damageTarget.characterNetworkManager.NotifyTheServerOfCharacterDamageServerRpc(
                    damageTarget.NetworkObjectId, 
                    dummyPunchDamage.NetworkObjectId,
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


    }    
}
