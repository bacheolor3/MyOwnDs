using UnityEngine;

namespace TSG
{
    public class PlayerEffectsManager : CharacterEffectManager
    {
        [Header("디버그용. 후에 지울 것")]
        [SerializeField] InstantCharacterEffect effectToTest;
        [SerializeField] bool processEffect = false;

        private void Update()
        {
            if (processEffect)
            {
                processEffect = false;
                // Q: 왜 이걸 그대로 사용하지 않고, 굳이 복사본을 생성(인스턴스화)해서 사용하는 거죠?
                // A: 그대로 사용하면 자료가 오염될 가능성도 있고, 뭣보다 후에 수정하기 편리하니까

                // 인스턴스화(복제)해서 사용하면, 여기서 값을 수정해도 프로젝트의 원본 파일(Asset)은 그대로 유지됩니다.
                // TakeStaminaDamageEffect effect = Instantiate(effectToTest) as TakeStaminaDamageEffect;
                // effect.staminaDamage = 55;

                // "인스턴스화하지 않고 원본을 그대로 쓰면 파일 자체가 수정되어 버립니다. (이는 대부분의 경우 의도치 않은 버그를 유발합니다.)                
                // effectToTest.staminaDamage = 55;
                InstantCharacterEffect effect = Instantiate(effectToTest);
                ProcessInstantEffect(effect);
            }
        }
    }    
}
