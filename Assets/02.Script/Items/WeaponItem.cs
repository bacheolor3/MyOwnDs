using UnityEngine;

namespace TSG
{
    public class WeaponItem : Item
    {
        // 애니메이터 컨트롤러 오버라이드(들고 있는 무기에 따라 공격 애니메이션 전환)

        [Header("무기 모델")]
        public GameObject weaponModel;

        [Header("무기 장착 요구스탯")]
        public int strengthREQ = 0;
        public int dexREQ = 0;
        public int intREQ = 0;
        public int faithREQ = 0;

        [Header("무기 기본 데미지")]
        public int physicalDamage = 0;
        public int magicDamage = 0;
        public int fireDamage = 0;
        public int holyDamage = 0;
        public int lightningDamage = 0;

        // 무기의 방어시 흡수도(방어력)

        [Header("무기로 주는 강인도 데미지")]
        public float poiseDamage = 10;
        // 공격시 강인도 보너스

        // 무기 보정치
        // 약공격 보정
        [Header("공격 보정치")]
        public float light_Attack_01_Modifier = 1.1f;
        // 강공격 보정
        // 치명타 공격 보정 등

        [Header("스테미나 소모도")]
        public int baseStaminaCost = 20;
        public float lightAttackStaminaCostMultiplier = 0.9f;
        // 달리기 공격 스테미나 소모도
        // 약공격 스테미나 소모도
        // 강공격 스테미나 소모도 등
        
        // 아이템 기반 액션(RB, RT, LB, LT)
        [Header("액션")]
        public WeaponItemAction oh_RB_Action;   // One Hand Right Bumper Action

        // 전장의 재(보스룸 연기 말하는거..)

        // 막기 소리
    }
}
