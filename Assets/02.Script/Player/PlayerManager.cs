using UnityEngine;

namespace TSG
{
    public class PlayerManager : CharacterManager
    {
        PlayerLocomotionManager playerLocomotionManager;

        protected override void Awake()
        {
            base.Awake();

            // 플레이어만을 위한 기능을 추가할 때 사용

            playerLocomotionManager = GetComponent<PlayerLocomotionManager>();
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
            }
        }
    }
    
}
