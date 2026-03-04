using UnityEngine;
using Unity.Netcode;

namespace TSG
{
    public class CharacterAnimatorManager : MonoBehaviour
    {
        CharacterManager character;

        int vertical;
        int horizontal;

        protected virtual void Awake()
        {
            character = GetComponent<CharacterManager>();

            vertical = Animator.StringToHash("Vertical");
            horizontal = Animator.StringToHash("Horizontal");
        }

        public void UpdateAnimatorMovementParameters(float horizontalMovement, float verticalMovement, bool isSprinting)
        {
            float horizontalAmount = horizontalMovement;
            float verticalAmount = verticalMovement;

            if (isSprinting)
            {
                verticalAmount = 2;
            }

            // Option 1 (애니메이션 품질이 괜찮을 때)
            character.animator.SetFloat(horizontal, horizontalAmount, 0.1f, Time.deltaTime);
            character.animator.SetFloat(vertical, verticalAmount, 0.1f, Time.deltaTime);

            // Option 2 (애니메이션 품질이 영 별로거나, 직접 만들었을 때)

            // float snappedHorizontal = 0;
            // float snappedVertical = 0;

            // #region Horizontal
            // // This if chain will round the horizontal movement to -1, -0.5, 0, 0.5 or 1

            // if(horizontalMovement > 0 && horizontalMovement <= 0.5f)
            // {
            //     snappedHorizontal = 0.5f;
            // }
            // else if(horizontalMovement > 0.5f && horizontalMovement <= 1f)
            // {
            //     snappedHorizontal = 1;
            // }else if (horizontalMovement < 0 && horizontalMovement >= -0.5f)
            // {
            //     snappedHorizontal = -0.5f;
            // }else if (horizontalMovement < -0.5 && horizontalMovement >= -1f)
            // {
            //     snappedHorizontal = -1;
            // }
            // else
            // {
            //     snappedHorizontal = 0;
            // }

            // #endregion

            // #region Vertical
            // // This if chain will round the vertical movement to -1, -0.5, 0, 0.5 or 1

            // if(verticalMovement > 0 && verticalMovement <= 0.5f)
            // {
            //     snappedVertical = 0.5f;
            // }
            // else if (verticalMovement > 0.5f && verticalMovement <= 1)
            // {
            //     snappedVertical = 1;
            // }else if (verticalMovement < 0 && verticalMovement >=  -0.5f)
            // {
            //     snappedVertical = -0.5f;
            // }else if (verticalMovement < -0.5f && verticalMovement >= -1)
            // {
            //     snappedVertical = -1;
            // }
            // else
            // {
            //     snappedVertical = 0;
            // }
            // #endregion
        }
    
        public virtual void PlayTargetActionAnimation(
            string targetAnimation, 
            bool isPerformingAction, 
            bool applyRootMotion = true, 
            bool canRotate = false, 
            bool canMove = false)
        {
            character.applyRootMotion = applyRootMotion;
            character.animator.CrossFade(targetAnimation, 0.2f);
            // 캐릭터가 새로운 동작을 시도하는 걸 막기 위해 사용
            // 예: 플레이어가 데미지를 받을 경우, 데미지를 받는 애니메이션을 실행
            // 이 기준점이 (isPerformingAction) True로 변환
            // 새로운 동작 및 액션을 취하기 전에 이 기준점을 체크하게 함
            character.isPerformingAction = isPerformingAction;
            character.canRotate = canRotate;
            character.canMove = canMove;

            // 서버/호스트 에게 현시점 서버에 있는 사람에게 이 애니메이션을 실행하라고 해야 함
            character.characterNetworkManager.NotifyTheServerOfActionAnimationServerRpc(NetworkManager.Singleton.LocalClientId, targetAnimation, applyRootMotion);
        }

        public virtual void PlayTargetAttackActionAnimation(
            string targetAnimation, 
            bool isPerformingAction, 
            bool applyRootMotion = true, 
            bool canRotate = false, 
            bool canMove = false)
        {
            // 마지막으로 실행한 공격을 확인(콤보를 위해)
            // 지금 시전하는 공격 타입을 확인 (약공, 강공, 그외)
            // 지금 무기에 맞는 애니메이션으로 업데이트
            // 우리 공격이 패링 가능한지 아닌지 확인
            // 네트워크에 우리가 "공격 중"이라는 플래그(신호)를 보냄(카운터 데미지등 처리위해)
            
            character.applyRootMotion = applyRootMotion;
            character.animator.CrossFade(targetAnimation, 0.2f);
            character.isPerformingAction = isPerformingAction;
            character.canRotate = canRotate;
            character.canMove = canMove;

            // 서버/호스트 에게 현시점 서버에 있는 사람에게 이 애니메이션을 실행하라고 해야 함
            character.characterNetworkManager.NotifyTheServerOfAttackActionAnimationServerRpc(NetworkManager.Singleton.LocalClientId, targetAnimation, applyRootMotion);
        }
    }
    
}
