using UnityEngine;
using UnityEngine.AI;

namespace TSG
{
    public class AICharacterManager : CharacterManager
    {
        [HideInInspector] public AICharacterNetworkManager aiCharacterNetworkManager;
        [HideInInspector] public AICharacterCombatManager aiCharacterCombatManager;
        [HideInInspector] public AICharacterLocomotionManager aiCharacterLocomotionManager;
        [Header("Navmesh Agent")]
        public NavMeshAgent navMeshAgent;

        [Header("현재 상태")]
        [SerializeField] AIState currentState;

        [Header("상태")]
        public IdleState idle;
        public PursueTargetState pursueTarget;
        // 전투 상태
        // 공격

        protected override void Awake()
        {
            base.Awake();
            aiCharacterCombatManager = GetComponent<AICharacterCombatManager>();
            aiCharacterNetworkManager = GetComponent<AICharacterNetworkManager>();
            aiCharacterLocomotionManager = GetComponent<AICharacterLocomotionManager>();

            navMeshAgent = GetComponentInChildren<NavMeshAgent>();

            // 원본이 변하지 않기 위해 스크립트 가능한 오브젝트를 복사해서 사용
            idle = Instantiate(idle);
            pursueTarget = Instantiate(pursueTarget);

            currentState = idle;
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();

            if (IsOwner)
            {
                ProcessStateMachine();
            }
        }

        // 첫번째 옵션
        private void ProcessStateMachine()
        {
            AIState nextState = currentState?.Tick(this);

            if(currentState != null)
            {
                currentState = nextState;
            }

            // position/rotattion은 상태 머신이 틱 상태일때에만 리셋되어야 함
            navMeshAgent.transform.localPosition = Vector3.zero;
            navMeshAgent.transform.localRotation = Quaternion.identity;

            if(aiCharacterCombatManager.currentTarget != null)
            {
                aiCharacterCombatManager.targetsDirection = aiCharacterCombatManager.currentTarget.transform.position - transform.position;
                aiCharacterCombatManager.viewableAngle = WorldUtilityManager.Instance.GetAngleOfTarget(transform, aiCharacterCombatManager.targetsDirection);
                aiCharacterCombatManager.distanceFromTarget = Vector3.Distance(transform.position,aiCharacterCombatManager.currentTarget.transform.position);
            }

            if(navMeshAgent.enabled)
            {
                Vector3 agentDestination = navMeshAgent.destination;
                float remainingDistance = Vector3.Distance(agentDestination, transform.position);

                if(remainingDistance > navMeshAgent.stoppingDistance)
                {
                    aiCharacterNetworkManager.isMoving.Value = true;
                }
                else
                {
                    aiCharacterNetworkManager.isMoving.Value = false;
                }
            }
            else
            {
                aiCharacterNetworkManager.isMoving.Value = false;
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