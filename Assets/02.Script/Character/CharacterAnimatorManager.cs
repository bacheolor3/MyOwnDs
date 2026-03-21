using UnityEngine;
using Unity.Netcode;
using System.Collections.Generic;

namespace TSG
{
    public class CharacterAnimatorManager : MonoBehaviour
    {
        CharacterManager character;

        int vertical;
        int horizontal;

        [Header("데미지 받는 애니메이션")]
        public string lastDamageAnimationPlayed;

        [SerializeField] string hit_Forward_Medium_01 = "Hit_Forward_Medium_01";
        [SerializeField] string hit_Forward_Medium_02 = "Hit_Forward_Medium_02";
        [SerializeField] string hit_Backward_Medium_01 = "Hit_Backward_Medium_01";
        [SerializeField] string hit_Backward_Medium_02 = "Hit_Backward_Medium_02";
        [SerializeField] string hit_Left_Medium_01 = "Hit_Left_Medium_01";
        [SerializeField] string hit_Left_Medium_02 = "Hit_Left_Medium_02";
        [SerializeField] string hit_Right_Medium_01 = "Hit_Right_Medium_01";
        [SerializeField] string hit_Right_Medium_02 = "Hit_Right_Medium_02";

        public List<string> forward_Medium_Damage = new List<string>();
        public List<string> backward_Medium_Damage = new List<string>();
        public List<string> left_Medium_Damage = new List<string>();
        public List<string> right_Medium_Damage = new List<string>();

        protected virtual void Awake()
        {
            character = GetComponent<CharacterManager>();

            vertical = Animator.StringToHash("Vertical");
            horizontal = Animator.StringToHash("Horizontal");
        }

        protected virtual void Start()
        {
            forward_Medium_Damage.Add(hit_Forward_Medium_01);
            forward_Medium_Damage.Add(hit_Forward_Medium_02);

            backward_Medium_Damage.Add(hit_Backward_Medium_01);
            backward_Medium_Damage.Add(hit_Backward_Medium_02);

            left_Medium_Damage.Add(hit_Left_Medium_01);
            left_Medium_Damage.Add(hit_Left_Medium_02);

            right_Medium_Damage.Add(hit_Right_Medium_01);
            right_Medium_Damage.Add(hit_Right_Medium_02);
        }

        public string GetRandomAnimationFromList(List<string> animationList)
        {
            List<string> finalList = new List<string>();

            foreach(var item in animationList)
            {
                finalList.Add(item);
            }

            // 이 애니메이션이 이미 실행되어서 반복되지 않도록 확인
            finalList.Remove(lastDamageAnimationPlayed);

            // null이 없도록 리스트 체크하고 있다면 없애기
            for (int i = finalList.Count - 1; i > -1; i--)
            {
                if(finalList[i] == null)
                {
                    finalList.RemoveAt(i);
                }
            }

            int randomValue = Random.Range(0, finalList.Count);

            return finalList[randomValue];
        }

        public void UpdateAnimatorMovementParameters(float horizontalMovement, float verticalMovement, bool isSprinting)
        {
            float snappedHorizontal = horizontalMovement;
            float snappedVertical = verticalMovement;
            // This if chain will round the horizontal movement to -1, -0.5, 0, 0.5 or 1

            if(horizontalMovement > 0 && horizontalMovement <= 0.5f)
            {
                snappedHorizontal = 0.5f;
            }
            else if(horizontalMovement > 0.5f && horizontalMovement <= 1f)
            {
                snappedHorizontal = 1;
            }else if (horizontalMovement < 0 && horizontalMovement >= -0.5f)
            {
                snappedHorizontal = -0.5f;
            }else if (horizontalMovement < -0.5 && horizontalMovement >= -1f)
            {
                snappedHorizontal = -1;
            }
            else
            {
                snappedHorizontal = 0;
            }

            if (isSprinting)
            {
                snappedVertical = 2;
            }

            // Option 1 (애니메이션 품질이 괜찮을 때)
            character.animator.SetFloat(horizontal, snappedHorizontal, 0.1f, Time.deltaTime);
            character.animator.SetFloat(vertical, snappedVertical, 0.1f, Time.deltaTime);

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
            Debug.Log("애니메이션 재생중: " + targetAnimation);
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

        public virtual void PlayTargetAttackActionAnimation(AttackType attackType,
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
            
            character.characterCombatManager.currentAttackType = attackType;
            character.characterCombatManager.lastAttackAnimationPerformed = targetAnimation;
            character.applyRootMotion = applyRootMotion;
            character.animator.CrossFade(targetAnimation, 0.2f);
            character.isPerformingAction = isPerformingAction;
            character.canRotate = canRotate;
            character.canMove = canMove;

            // 서버/호스트 에게 현시점 서버에 있는 사람에게 이 애니메이션을 실행하라고 해야 함
            character.characterNetworkManager.NotifyTheServerOfAttackActionAnimationServerRpc(NetworkManager.Singleton.LocalClientId, targetAnimation, applyRootMotion);
        }

        public virtual void EnableCanDoCombo()
        {
            
        }

        public virtual void DisableCanDoCombo()
        {
            
        }
    }
}
