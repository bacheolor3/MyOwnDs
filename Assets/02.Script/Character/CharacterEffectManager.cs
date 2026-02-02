using UnityEngine;


namespace TSG
{
    public class CharacterEffectManager : MonoBehaviour
    {
        // 즉발적으로 효과가 나오는 이펙트(데미지를 받는다던지, 회복한다던지)

        // 지속시간이 있는 이펙트(독, 빌드 등)

        // 정적인 이펙트들(버프를 추가 / 제거한다던지 등)

        CharacterManager character;

        protected virtual void Awake()
        {
            character = GetComponent<CharacterManager>();
        }

        public void ProcessInstantEffect(InstantCharacterEffect effect)
        {
            // 효과를 받고
            // 실행시킴
            effect.ProcessEffect(character);
        }
    }    
}
