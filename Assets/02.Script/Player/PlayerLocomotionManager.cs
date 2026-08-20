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
        [SerializeField] float rotationSpeed = 15;   
        [SerializeField] int sprintingStaminaCost = 2;

        [Header("Jump")]
        [SerializeField] float jumpStaminaCost = 25;
        [SerializeField] float jumpHeight = 4;
        [SerializeField] float jumpFowardSpeed = 5;
        [SerializeField] float freeFallSpeed = 2;
        private Vector3 jumpDirection;


        [Header("Dodge")]
        private Vector3 rollDirection;
        [SerializeField] float dodgeStaminaCost = 25;

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
                if (!player.playerNetworkManager.isLockedOn.Value || player.playerNetworkManager.isSprinting.Value)
                {
                    player.playerAnimatorManager.UpdateAnimatorMovementParameters(0, moveAmount, player.playerNetworkManager.isSprinting.Value);
                }
                // 만약 락온 되어 있다면, 가로세로 움직임을 더함
                else
                {
                    player.playerAnimatorManager.UpdateAnimatorMovementParameters(horizontalMovement, verticalMovement, player.playerNetworkManager.isSprinting.Value);
                }
            }
        }

        public void HandleAllMovement()
        {            
            HandleGroundedMovement();
            HandleRotation();
            HandleJumpingMovement();
            HandleFreeFallMoveMent();
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

            if (!player.characterLocomotionManager.canMove)
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

        private void HandleJumpingMovement()
        {
            if (player.playerNetworkManager.isJumping.Value)
            {
                player.characterController.Move(jumpDirection * jumpFowardSpeed * Time.deltaTime);
            }
        }

        private void HandleFreeFallMoveMent()
        {
            if (!player.characterLocomotionManager.isGrounded)
            {
                Vector3 freeFallDirection;

                freeFallDirection = PlayerCamera.instance.transform.forward * PlayerInputManager.instance.verticalInput;
                freeFallDirection = freeFallDirection + PlayerCamera.instance.transform.right * PlayerInputManager.instance.horizontalInput;
                freeFallDirection.y = 0;

                player.characterController.Move(freeFallDirection * freeFallSpeed * Time.deltaTime);
            }
        }
        
        private void HandleRotation()
        {
            if (player.isDead.Value)
            {
                return;
            }

            if (!player.characterLocomotionManager.canRotate)
            {
                return;
            }

            if (player.playerNetworkManager.isLockedOn.Value)
            {
                if(player.playerNetworkManager.isSprinting.Value || player.playerLocomotionManager.isRolling)
                {
                    Vector3 targetDirection = Vector3.zero;
                    targetDirection = PlayerCamera.instance.cameraObject.transform.forward * verticalMovement;
                    targetDirection += PlayerCamera.instance.cameraObject.transform.right * horizontalMovement;
                    targetDirection.Normalize();
                    targetDirection.y = 0;

                    if(targetDirection == Vector3.zero)
                    {
                        targetDirection = transform.forward;
                    }

                    Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
                    Quaternion finalRotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
                    transform.rotation = finalRotation;
                }
                else
                {
                    if(player.playerCombatManager.currentTarget == null)
                    {
                        return;
                    }

                    Vector3 targetDirection;
                    targetDirection = player.playerCombatManager.currentTarget.transform.position - transform.position;
                    targetDirection.y = 0;
                    targetDirection.Normalize();

                    Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
                    Quaternion finalRotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
                    transform.rotation = finalRotation;
                }
            }
            else
            {
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
                Quaternion targetRotation = Quaternion.Slerp(transform.rotation, newRotation, rotationSpeed * Time.deltaTime);
                transform.rotation = targetRotation;
            }

        }
    
        public void HandleSprinting()
        {
            if (player.isPerformingAction)
            {
                // 기본적인 상태에선 질주하지 않는 모양세로
                player.playerNetworkManager.isSprinting.Value = false;
            }

            // 스테미나가 충분하지 않다면, 질주를 안 하도록
            if(player.playerNetworkManager.currentStamina.Value <= 0)
            {
                player.playerNetworkManager.isSprinting.Value = false;
                return;
            }

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

            if (player.playerNetworkManager.isSprinting.Value)
            {
                player.playerNetworkManager.currentStamina.Value -= sprintingStaminaCost * Time.deltaTime;
            }
        }

        public void AttemptToPerformDodge()
        {
            if (player.isPerformingAction)
            {
                return;
            }
            if(player.playerNetworkManager.currentStamina.Value <= 0)
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
                player.playerLocomotionManager.isRolling = true;
            }
            // 만약 정지되어 있는 상태에서 회피를 실행하려 한다면, 백스텝 실행
            else
            {
                // 뒷걸음(백스텝) 애니메이션
                player.playerAnimatorManager.PlayTargetActionAnimation("Back_Step_01", true, true);
            }

            player.playerNetworkManager.currentStamina.Value -= dodgeStaminaCost;
        }
    
        public void AttemptToPerformJump()
        {
            // 액션을 다른 걸 행하는 중이라면, 점프가 되지 않게 설정(전투 애니메이션 실행중이라던지...)
            if (player.isPerformingAction)
            {
                return;
            }

            // 스테미나가 없다면 점프할 수 없도록 설정
            if(player.playerNetworkManager.currentStamina.Value <= 0)
            {
                return;
            }

            // 이미 점프중이라면, 현재 점프가 끝나기 전엔 점프가 안 되게 설정
            if (player.playerNetworkManager.isJumping.Value)
            {
                return;
            }

            // 만약 땅에 있는 게 아니라면, 점프를 하게 허가하지 않음
            if (!player.characterLocomotionManager.isGrounded)
            {
                return;
            }

            // 만약 두손 무기를 들 고 있다면, 두손 무기를 든 채 점프하는 모션, 아니라면 한손으로 애니메이션 실행
            player.playerAnimatorManager.PlayTargetActionAnimation("Main_Jump_01", false);

            player.playerNetworkManager.isJumping.Value = true;

            player.playerNetworkManager.currentStamina.Value -= jumpStaminaCost;

            jumpDirection = PlayerCamera.instance.cameraObject.transform.forward * PlayerInputManager.instance.verticalInput;
            jumpDirection += PlayerCamera.instance.cameraObject.transform.right * PlayerInputManager.instance.horizontalInput;
            jumpDirection.y = 0;

            if(jumpDirection != Vector3.zero)
            {
                // 질주 중이면, 점프하는 방향으로 최대로 뜀
                if (player.playerNetworkManager.isSprinting.Value)
                {
                    jumpDirection *= 1;
                }
                // 달리는 중이면, 점프하는 방향으로 절반정도 뜀
                else if(PlayerInputManager.instance.moveAmount > 0.5)
                {
                    jumpDirection *= 0.5f;
                }
                // 걷는 중이면, 점프하는 방향으로 4분의1정도로 뜀
                else if(PlayerInputManager.instance.moveAmount <= 0.5)
                {
                    jumpDirection *= 0.25f;
                }                
            }
        }
    
        public void ApplyJumpingVelocity()
        {
            // 위로 올라가는 힘을 적용
            yVelocity.y = Mathf.Sqrt(jumpHeight * -2 * gravityForce);
        }
    }
    
}
