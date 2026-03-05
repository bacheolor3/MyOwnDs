using UnityEngine;
using Unity.Netcode;
using UnityEngine.TextCore.Text;
using System.Collections;
using System.Collections.Generic;

namespace TSG
{
    public class CharacterManager : NetworkBehaviour
    {
        [Header("상태")]
        public NetworkVariable<bool> isDead = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        [HideInInspector] public CharacterController characterController;
        [HideInInspector] public Animator animator;

        [HideInInspector] public CharacterNetworkManager characterNetworkManager;
        [HideInInspector] public CharacterEffectManager characterEffectManager;
        [HideInInspector] public CharacterAnimatorManager characterAnimatorManager;
        [HideInInspector] public CharacterCombatManager characterCombatManager;

        [Header("기준점")]
        public bool isPerformingAction = false;
        
        public bool isGrounded = true;
        public bool applyRootMotion = false;
        public bool canRotate = true;
        public bool canMove = true;

        protected virtual void Awake()
        {
            DontDestroyOnLoad(this);

            characterController = GetComponent<CharacterController>();
            animator = GetComponent<Animator>();

            characterNetworkManager = GetComponent<CharacterNetworkManager>();
            characterEffectManager = GetComponent<CharacterEffectManager>();
            characterAnimatorManager = GetComponent<CharacterAnimatorManager>();
            characterCombatManager = GetComponent<CharacterCombatManager>();
        }

        protected virtual void Start()
        {
            IgnoreMyOwnColliders();
        }

        protected virtual void Update()
        {
            animator.SetBool("isGrounded", isGrounded);
            if (IsOwner)
            {
                // 만약 캐릭터가 우리쪽에서 컨트롤 되고 있다면, 그러면 그 물체의 네트워크 포지션을  우리의 포지션으로 동일시한다
                characterNetworkManager.networkPosition.Value = transform.position;
                characterNetworkManager.networkRotation.Value = transform.rotation;
            }
                // 만약 이 캐릭터가 다른데에서 컨트롤 되고 있다면, 그 다음, 이 오브젝트의 로컬 위치를 네트워크 트랜스폼(NetworkTransform)의 위치로 설정해라
            else
            {
                // 포지션
                transform.position = Vector3.SmoothDamp
                    (transform.position,
                    characterNetworkManager.networkPosition.Value,
                    ref characterNetworkManager.networkPositionVelocity,
                    characterNetworkManager.networkPositionSmoothTime);
                // 로테이션
                transform.rotation = Quaternion.Slerp
                    (transform.rotation,
                    characterNetworkManager.networkRotation.Value,
                    characterNetworkManager.networkRotationSmoothTime);
            }
        }
    
        protected virtual void LateUpdate()
        {
            
        }

        public virtual IEnumerator ProcessDeathEvent(bool manuallySelectDeathAnimation = false)
        {
            if (IsOwner)
            {
                characterNetworkManager.currentHealth.Value = 0;
                isDead.Value = true;

                // 필요한 모든 요소들 여기서 다 리셋할것
                // 아직은 없음

                // 만약 우리가 땅에 있는 게 아니라면, 공중 사망 애니메이션을 적용할 것
                if (!manuallySelectDeathAnimation)
                {
                    characterAnimatorManager.PlayTargetActionAnimation("Dead_01", true);
                }
            }
            // 사망할 때의 효과들 재생

            yield return new WaitForSeconds(5);

            // 플레이어에게 룬으로 보상 제공

            // 캐릭터 삭제(Disable)
        }

        public virtual void ReviveCharacter()
        {
            if (IsOwner)
            {
                isDead.Value = false;
                isPerformingAction = false; // 액션 고정 해제
                // 필요한 경우 콜라이더나 리지드바디 복구 로직 추가
            }
        }

        protected virtual void IgnoreMyOwnColliders()
        {
            Collider characterControllerCollider = GetComponent<Collider>();
            Collider[] damageableCharacterColliders = GetComponentsInChildren<Collider>();
            List<Collider> ignoreColliders = new List<Collider>();

            // 내 캐릭터가 가지고 있는 모든 충돌 판정용 콜라이더는 리스트에 넣고, 서로 무시하게 될 것
            foreach(var collider in damageableCharacterColliders)
            {
                ignoreColliders.Add(collider);
            }

            // 캐릭터 컨트롤러 콜라이더를 리스트에 추가해 충돌 무시에 쓰도록 할 것
            ignoreColliders.Add(characterControllerCollider);

            // 리스트에 있는 모든 콜라이더를 지나가며, 서로간의 충돌을 무시할 것
            foreach(var collider in ignoreColliders)
            {
                foreach(var otherCollder in ignoreColliders)
                {
                    Physics.IgnoreCollision(collider, otherCollder, true);
                }
            }
        }
    }    
}
