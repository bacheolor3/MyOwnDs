using UnityEngine;

namespace TSG
{
    [CreateAssetMenu(menuName = "Character Effects/Instant Effect/Take Damage")]
    public class TakeDamageEffect : InstantCharacterEffect
    {
        [Header("캐릭터가 가하는 데미지")]
        public CharacterManager characterCausingDamage;     // 만약 다른 캐릭터에 의해 가해진 데미지면 여기에 저장될 것

        [Header("데미지")]
        public float physicalDamage = 0;    // (미래에는 "기본", "충격", "참격" 그리고 "관통"으로 나눌 것)
        public float magicDamage = 0;
        public float fireDamage = 0;
        public float lightningDamage = 0;
        public float holyDamage = 0;

        // 빌드업
        // 이펙트 효과의 빌드업

        [Header("최종 데미지")]
        private int finalDamageDealt = 0; // 모든 계산이 끝난 후 캐릭터가 받는 데미지를 계산한다

        [Header("강인도(Poise)")]       // 공격을 받아도 내 자세나 평정심(동작)이 무너지지 않는 수치 = Poise의 뜻
                                // 한국은 보통 "강인도"라고 많이들 기억함
        public float poiseDamage = 0;
        public bool poiseIsBroken = false;  // 만약 캐릭터의 강인도가 무너졌다면, "기절"과 함께 데미지를 입음

        [Header("애니메이션")]
        public bool playDamageAnimation = true;
        public bool manuallySelectDamageAnimation = false;
        public string damageAnimation;

        [Header("사운드 FX")]
        public bool willPlayDamageSFX = true;
        public AudioClip elementalDamageSoundFX;   // 만약 속성(마법/불/번개/신성) 데미지 효과 사운드가 필요하다면 일반적인 SFX보다 먼저 우선순위를 가짐

        [Header("데미지 입는 방향 따라 반응")]
        public float angleHitFrom;      // 어떤 데미지를 받는 애니메이션을 실행할지 결정(뒤로 물러설지, 옆으로 갈지, 오른쪽으로 갈지 등)
        public Vector3 contactPoint;    // 혈흔 FX효과를 어디에 표현할 지 정할것

        public override void ProcessEffect(CharacterManager character)
        {
            base.ProcessEffect(character);
            
            // 캐릭터가 죽었다면, 더는 그 어떤 데미지 효과도 진행하지 말 것
            if (character.isDead.Value)
            {
                return;
            }

            // "무적"상태인지 아닌지 확인할 것

            // 데미지 계산
            CalculateDamage(character);
            // 데미지 받는 방향 계산
            // 데미지 애니메이션 재생
            // 빌드업 계산(독, 출혈 등)
            // 데미지 사운드 재생
            // 데미지 효과(VFX)(피 등) 재생

            // 캐릭터가 AI인 경우, 데미지를 가한 캐릭터가 존재한다면 새로운 타겟으로 설정할지 확인
        }

        private void CalculateDamage(CharacterManager character)
        {
            Debug.Log($"[1] CalculateDamage 호출됨. 대상: {character.name}");

            Debug.Log($"[2] 권한 확인 - IsOwner: {character.IsOwner}, IsServer: {character.IsServer}");

            if (!character.IsOwner)
            {
                return;
            }
            if(characterCausingDamage != null)
            {
                // 데미지 전환을 확인하고, 그 전환된 형태로 데미지 형식을 바꿀 것(물리/원소 데미지 버프 등)
                // (physical *= physicalModifer etc)
            }

            // 캐릭터의 고정 방어력을 확인하고 그 수치만큼 데미지에서 절감

            // 캐릭터의 아머 흡수력과 그로 인한 데미지 경감율도 계산

            // 그 모든 데미지 타입을 더해서 최종 데미지 결정
            finalDamageDealt = Mathf.RoundToInt(physicalDamage + magicDamage + fireDamage + lightningDamage + holyDamage);

            if(finalDamageDealt <= 0)
            {
                finalDamageDealt = 1;
            }

            Debug.Log("최종 데미지 : " + finalDamageDealt);
            character.characterNetworkManager.currentHealth.Value -= finalDamageDealt;
            Debug.Log($"[4] 최종 체력 차감 완료. 현재 체력: {character.characterNetworkManager.currentHealth.Value}");
            // 캐릭터가 스턴 걸릴지 말지를 확인하기 위해 강인도 계산
        }
    }    
}
