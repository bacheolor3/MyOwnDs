using UnityEngine;

namespace TSG
{
    [CreateAssetMenu(menuName = "Character EFfects/Instant Effect/Take STamina Damage")]
    public class TakeStaminaDamageEffect : InstantCharacterEffect
    {
        public float staminaDamage;
        public override void ProcessEffect(CharacterManager character)
        {
            CalculateStaminaDamage(character);
        }

        private void CalculateStaminaDamage(CharacterManager character)
        {
            // 기본 스테미나 데미지를 다른 플레이어의 이펙트/상태이상에 비교 Compared the base stamina damage against other player Effect/Modifiers
            // 값을 빼기/더하기 전에 바꿔야 함 Change the Value before Subtracting/Adding it
            // 특수효과/시각효과와 음향을 이펙트 지속 중에 재생할 것 Play Sound FX or VFX during effect

            if (character.IsOwner)
            {
                Debug.Log("캐릭터가 공격받는 중입니다: "+ staminaDamage + " 만큼의 스테미나 데미지");
                character.characterNetworkManager.currentStamina.Value -= staminaDamage;
            }
        }
    }
    
}
