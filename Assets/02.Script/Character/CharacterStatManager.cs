using UnityEngine;


namespace TSG
{
    public class CharacterStatManager : MonoBehaviour
    {
        CharacterManager character;
        [Header("스태미나 재생")]
        private float staminaRegenrationTimer = 0;
        private float staminaTickTimer = 0;
        [SerializeField] float staminaRegenerationDelay = 2;
        [SerializeField] float staminaRegenrationAmount = 2;

        protected virtual void Awake()
        {
            character = GetComponent<CharacterManager>();
        }

        public int CalculateStatminaBasedOnEnduranceLevel(int endurance)
        {
            float stamina = 0;

            // 스테미나가 어떻게 계산되는지 공식 만들기

            stamina = endurance * 10;

            return Mathf.RoundToInt(stamina);
        }

        public virtual void RegenerateStamina()
        {
            // 오로지 서버의 주인만이 네트워크 변수를 편집할 수 있다
            if (!character.IsOwner)
            {
                return;
            }

            // 스테미나를 소모하고 있는 중엔 재생되지 않게 할 것
            if (character.characterNetworkManager.isSprinting.Value)
            {
                return;
            }

            if (character.isPerformingAction)
            {
                return;
            }

            staminaRegenrationTimer += Time.deltaTime;

            if(staminaRegenrationTimer >= staminaRegenerationDelay)
            {
                if(character.characterNetworkManager.currentStamina.Value < character.characterNetworkManager.maxStamina.Value)
                {
                    staminaTickTimer = staminaTickTimer + Time.deltaTime;

                    if(staminaTickTimer >= 0.1)
                    {
                        staminaTickTimer = 0;
                        character.characterNetworkManager.currentStamina.Value += staminaRegenrationAmount;
                    }
                }
            }
        }
    
        public virtual void ResetStaminaRegenTimer(float previousStaminaAmount, float currentStaminaAmount)
        {
            // 스태미나 재생 리셋되는 건 스태미나가 소모되는 행동을 했을 때에만
            // 스태미나 재생 중일 때에는 리셋을 하지 말아야 함
            if(currentStaminaAmount < previousStaminaAmount)
            {
                staminaRegenrationTimer = 0;
            }            
        }
    }    
}
