using UnityEngine;

namespace TSG
{
    public class PlayerLocomotionManager : CharacterLocomotionManager
    {
        PlayerManager player;

        [HideInInspector] public float verticalMovement;        
        [HideInInspector] public float horizontalMovement;
        [HideInInspector] public float moveAmount;

        [Header("움직임 관련 설정")]
        private Vector3 moveDirection;
        private Vector3 targetRotationDirection;
        [SerializeField] float walkingSpeed = 2;
        [SerializeField] float runningSpeed = 5;
        [SerializeField] float sprintingSpeed = 6.5f;
        [SerializeField] float rotationspeed = 15;   


        [Header("Dodge")]
        private Vector3 rollDirection;

        protected override void Awake()
        {
            base.Awake();

            player = GetComponent<PlayerManager>();
        }

        protected override void Update()
        {
            base.Update();

            if (player.IsOwner)
            {
                player.characterNetworkManager.verticalMovement.Value = verticalMovement;
                player.characterNetworkManager.horizontalMovement.Value = horizontalMovement;
                player.characterNetworkManager.moveAmount.Value = moveAmount;
            }
            else
            {
                verticalMovement = player.characterNetworkManager.verticalMovement.Value;
                horizontalMovement = player.characterNetworkManager.horizontalMovement.Value;
                moveAmount = player.characterNetworkManager.moveAmount.Value;

                // 락온 되어 있지 않다면, moveAmount를 전함
                player.playerAnimatorManager.UpdateAnimatorMovementParameters(0, moveAmount, player.playerNetworkManager.isSprinting.Value);
                // 만약 락온 되어 있다면, 가로세로 움직임을 더함
            }
        }

        public void HandleAllMovement()
        {            
            HandleGroundedMovement();
            HandleRotation();
            // 땅에서 움직임
            // 점프 움직임
            // 회전
            // 낙하
        }

        private void GetMovementValues()
        {
            verticalMovement = PlayerInputManager.instance.verticalInput;
            horizontalMovement = PlayerInputManager.instance.horizontalInput;
            moveAmount = PlayerInputManager.instance.moveAmount;
            // 움직이는 범위 제한하기
        }

        private void HandleGroundedMovement()
        {
            GetMovementValues();

            if (!player.canMove)
            {
                return;
            }
            // 움직임의 방향성은 카메라가 보는 곳과 입력에 달렸다
            moveDirection = PlayerCamera.instance.transform.forward * verticalMovement;
            moveDirection = moveDirection + PlayerCamera.instance.transform.right * horizontalMovement;
            moveDirection.Normalize();
            moveDirection.y = 0;

            if (player.playerNetworkManager.isSprinting.Value)
            {
                player.characterController.Move(moveDirection * sprintingSpeed * Time.deltaTime);
            }
            else
            {
                if (PlayerInputManager.instance.moveAmount > 0.5f)
                {
                    // 달리는 속도로 움직임
                    player.characterController.Move(moveDirection * runningSpeed * Time.deltaTime);
                }
                else if (PlayerInputManager.instance.moveAmount <= 0.5f)
                {
                    // 걷는 속도로 움직임
                    player.characterController.Move(moveDirection * walkingSpeed * Time.deltaTime);
                }
            }

        }

        private void HandleRotation()
        {
            if (!player.canRotate)
            {
                return;
            }
            targetRotationDirection  = Vector3.zero;
            targetRotationDirection = PlayerCamera.instance.cameraObject.transform.forward * verticalMovement;
            targetRotationDirection = targetRotationDirection + PlayerCamera.instance.cameraObject.transform.right * horizontalMovement;
            targetRotationDirection.Normalize();
            targetRotationDirection.y = 0;

            if (targetRotationDirection == Vector3.zero)
            {
                targetRotationDirection = transform.forward;
            }

            Quaternion newRotation = Quaternion.LookRotation(targetRotationDirection);
            Quaternion targetRotation = Quaternion.Slerp(transform.rotation, newRotation, rotationspeed * Time.deltaTime);
            transform.rotation = targetRotation;
        }
    
        public void HandleSprinting()
        {
            if (player.isPerformingAction)
            {
                // 기본적인 상태에선 질주하지 않는 모양세로
                player.playerNetworkManager.isSprinting.Value = false;
            }

            // 스테미나가 충분하지 않다면, 질주를 안 하도록

            // 만약 움직인다면 질주 자세 전환 가능하도록
            if(moveAmount >= 0.5)
            {
                player.playerNetworkManager.isSprinting.Value = true;
            }
            // 만약 정지해 있거나 느리게 움직이고 있다면 질주 자세 안 하도록
            else
            {
                player.playerNetworkManager.isSprinting.Value = false;
            }

        }

        public void AttemptToPerformDodge()
        {
            if (player.isPerformingAction)
            {
                return;
            }
            // 만약 움직이는 중에 회피를 실행하려 한다면, 구르기 시전
            if(PlayerInputManager.instance.moveAmount > 0)
            {
                rollDirection = PlayerCamera.instance.cameraObject.transform.forward * PlayerInputManager.instance.verticalInput; // 마지막에 *verticalMovement로 해도 거의 근사값. 다만 이게 더 확실
                rollDirection += PlayerCamera.instance.cameraObject.transform.right * PlayerInputManager.instance.horizontalInput;
                rollDirection.y = 0;
                rollDirection.Normalize();

                Quaternion playerRotation = Quaternion.LookRotation(rollDirection);
                player.transform.rotation = playerRotation;                

                // 회피(구르기)애니메이션 실행
                player.playerAnimatorManager.PlayTargetActionAnimation("Roll_Foward_01", true, true);
            }
            // 만약 정지되어 있는 상태에서 회피를 실행하려 한다면, 백스텝 실행
            else
            {
                // 뒷걸음(백스텝) 애니메이션
                player.playerAnimatorManager.PlayTargetActionAnimation("Back_Step_01", true, true);
            }
        }
    }
    
}
