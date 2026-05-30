using UnityEngine;

namespace TSG
{
    public class AICharacterManager : CharacterManager
    {
        public AICharacterCombatManager aiCharacterCombatManager;
        [Header("현재 상태")]
        [SerializeField] AIState currentState;

        protected override void Awake()
        {
            base.Awake();
            aiCharacterCombatManager = GetComponent<AICharacterCombatManager>();
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();

            ProcessStateMachine();
        }

        // 첫번째 옵션
        private void ProcessStateMachine()
        {
            AIState nextState = null;
            if(currentState != null)
            {
                nextState = currentState.Tick(this);
            }

            if(nextState != null)
            {
                currentState = nextState;
            }
        }

        // 두번째 옵션
        // private void ProcessStateMachine2()
        // {
        //     AIState nextState = currentState?.Tick(this);

        //     if(nextState != null)
        //     {
        //         currentState = nextState;
        //     }
        // }
    }
}