using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TSG
{
    public class PlayerManager : CharacterManager
    {
        [HideInInspector] public PlayerAnimatorManager playerAnimatorManager;
        [HideInInspector] public PlayerLocomotionManager playerLocomotionManager;
        [HideInInspector] public PlayerNetworkManager playerNetworkManager;
        [HideInInspector] public PlayerStatsManager playerStatsManager;

        protected override void Awake()
        {
            base.Awake();

            // 플레이어만을 위한 기능을 추가할 때 사용

            playerLocomotionManager = GetComponent<PlayerLocomotionManager>();
            playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
            playerNetworkManager = GetComponent<PlayerNetworkManager>();
            playerStatsManager = GetComponent<PlayerStatsManager>();
        }

        protected override void Update()
        {
            base.Update();
            // 우리가 게임 오브젝트를 소유한 게 아니라면, 컨트롤하거나 수정할 수 없음
            if (!IsOwner)
            {
                return;
            }
            // 움직임 처리
            playerLocomotionManager.HandleAllMovement();

            // 스테미나 재생
            playerStatsManager.RegenerateStamina();
        }

        protected override void LateUpdate()
        {
            
            if (!IsOwner)
            {
                return;
            }

            base.LateUpdate();

            PlayerCamera.instance.HandleAllCameraActions();
        }

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();

            // 만약 이 플레이어 오브젝트 가 이 클라이언트 쪽 소유물이라면
            if (IsOwner)
            {
                PlayerCamera.instance.player = this;
                PlayerInputManager.instance.player = this;

                playerNetworkManager.currentStamina.OnValueChanged += PlayerUIManager.instance.playerUIHudManager.SetNewStaminaValue;
                playerNetworkManager.currentStamina.OnValueChanged += playerStatsManager.ResetStaminaRegenTimer;

                // 세이브 혹은 로딩중에 추가되면 없어짐
                playerNetworkManager.maxStamina.Value = playerStatsManager.CalculateStatminaBasedOnEnduranceLevel(playerNetworkManager.endurance.Value);
                playerNetworkManager.currentStamina.Value = playerStatsManager.CalculateStatminaBasedOnEnduranceLevel(playerNetworkManager.endurance.Value);
                PlayerUIManager.instance.playerUIHudManager.SetMaxStaminaValue(playerNetworkManager.maxStamina.Value);
            }
        }
    
        public void SaveGameDataToCurrentCharacterData(ref CharacterSaveData currentCharacterData)
        {
            currentCharacterData.characterName = playerNetworkManager.characterName.Value.ToString();
            currentCharacterData.yPosition = transform.position.y;
            currentCharacterData.xPosition = transform.position.x;
            currentCharacterData.zPosition = transform.position.z;
        }

        public void LoadGameDataFromCurrentCharacterData(ref CharacterSaveData currentCharacterData)
        {
            playerNetworkManager.characterName.Value = currentCharacterData.characterName;
            Vector3 myPosition = new Vector3(currentCharacterData.xPosition, currentCharacterData.yPosition, currentCharacterData.zPosition);
            transform.position = myPosition;
        }
    }
    
}
