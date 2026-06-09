using UnityEngine;
using UnityEngine.SceneManagement;

namespace TSG
{
    public class PlayerInputManager : MonoBehaviour
    {
        public static PlayerInputManager instance;  
        public PlayerManager player;
        // 목표를 하나하나 천천히 생각하기
        // 1. 조이스틱 값을 읽을 수 있는 방법 찾기
        // 2. 캐릭터를 그 값에 따라 움직이기
        PlayerControls playerControls;


        [Header("카메라 움직임 입력")]
        [SerializeField] Vector2 cameraInput;
        public float cameraVerticalInput;
        public float cameraHorizontalInput;

        [Header("락온 입력")]
        [SerializeField] bool lockOn_Input;
        [SerializeField] bool lockOn_Left_Input;
        [SerializeField] bool lockOn_Right_Input;
        private Coroutine lockOnCoroutine;

        [Header("플레이어 움직임 입력")]
        [SerializeField] Vector2 movementInput;
        public float verticalInput;
        public float horizontalInput;
        public float moveAmount;

        [Header("플레이어 액션 입력")]
        [SerializeField] bool dodge_Input = false;
        [SerializeField] bool sprint_Input = false;
        [SerializeField] bool jump_Input = false;
        [SerializeField] bool switch_Right_Weapon_Input = false;
        [SerializeField] bool switch_Left_Weapon_Input = false;
        
        [Header("범퍼 입력")]
        [SerializeField] bool RB_Input = false;

        [Header("트리거 입력")]
        [SerializeField] bool RT_Input = false;
        [SerializeField] bool Hold_RT_Input = false;

        
        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }            
        }

        private void Start()
        {
            DontDestroyOnLoad(gameObject);

            // 씬이 바뀌면 이 로직 사용
            SceneManager.activeSceneChanged += OnSceneChange;

            instance.enabled = false;

            if(playerControls != null)
            {
                playerControls.Disable();                
            }

        }
                
        private void OnSceneChange(Scene oldScene, Scene newScene)
        {
            // 월드 씬으로 가게 될 경우, 플레이어 컨트롤러 활성화
            if (newScene.buildIndex == WorldSaveGameManager.instance.GetWorldSceneIndex())
            {
                instance.enabled = true;

                if(playerControls != null)
                {
                    playerControls.Enable();
                }
            }
            // 아니면 플레이어 컨트롤러는 반드시 비활성화 이어야 한다
            // 이는 우리가 미래에 캐릭터 크리에이션 씬 같은 걸 만들때 쓰인다
            else
            {
                instance.enabled = false;

                if(playerControls != null)
                {
                    playerControls.Disable();                
                }
            }
        }

        private void OnEnable()
        {
            if (playerControls == null)
            {
                playerControls = new PlayerControls();

                playerControls.PlayerMovements.Movement.performed += i => movementInput = i.ReadValue<Vector2>();
                playerControls.PlayerCamera.Movement.performed += i => cameraInput = i.ReadValue<Vector2>();

                // 액션들                
                playerControls.PlayerActions.Dodge.performed += i => dodge_Input = true;
                playerControls.PlayerActions.Jump.performed += i => jump_Input = true;
                playerControls.PlayerActions.SwitchRightWeapon.performed += i => switch_Right_Weapon_Input = true;
                playerControls.PlayerActions.SwitchLeftWeapon.performed += i => switch_Left_Weapon_Input = true;

                // 범퍼들 
                playerControls.PlayerActions.RB.performed += i => RB_Input = true;

                // 트리거들
                playerControls.PlayerActions.RT.performed += i => RT_Input = true;
                playerControls.PlayerActions.HoldRT.performed += i => Hold_RT_Input = true;
                playerControls.PlayerActions.HoldRT.canceled += i => Hold_RT_Input = false;

                // 락온 
                playerControls.PlayerActions.LockOn.performed += i => lockOn_Input = true;
                playerControls.PlayerActions.SeekLeftLockOnTarget.performed += i => lockOn_Left_Input = true;
                playerControls.PlayerActions.SeekRightLockOnTarget.performed += i => lockOn_Right_Input = true;

                // 설정된 버튼을 누르고 있으면(여기서는 L Shift, 패드라면) bool값을 true로
                playerControls.PlayerActions.Sprint.performed += i => sprint_Input = true;
                // 누르던 버튼을 뗀다면, bool 값을 false로
                playerControls.PlayerActions.Sprint.canceled += i => sprint_Input = false;
            }

            playerControls.Enable();
        }

        private void OnDestroy()
        {
            // 만약 이 오브젝트를 파괴한다면, 이 이벤트에서 벗어나기
            SceneManager.activeSceneChanged -= OnSceneChange;
        }

        // 윈도우 창을 내리거나 낮추면, 움직임 받아들이기를 멈춤
        private void OnApplicationFocus(bool focus)
        {
            if (enabled)
            {
                if (focus)
                {
                    playerControls.Enable();
                }
                else
                {
                    playerControls.Disable();
                }
            }
        }
        
        private void Update()
        {
            HandleAllInputs();
        }

        private void HandleAllInputs()
        {
            HandleLockOnInput();
            HandleLockOnSwitchTargetInput();
            HandlePlayerMovementInput();
            HandleCameraMovementInput();
            HandleDodgeInput();
            HandleSprintInput();
            HandleJumpInput();
            HandleRBInput();
            HandleRTInput();
            HandleChargeRTInput();
            HandleSwitchRightWeaponInput();
            HandleSwitchLeftWeaponInput();
        }

        // 락온

        private void HandleLockOnInput()
        {
            // 죽은 타겟들을 확인
            if (player.playerNetworkManager.isLockedOn.Value)
            {
                if(player.playerCombatManager.currentTarget == null)
                {
                    return;
                }
                
                if (player.playerCombatManager.currentTarget.isDead.Value)
                {
                    player.playerNetworkManager.isLockedOn.Value = false;
                }

                // 새로운 타겟 찾으려고 시도

                // 이게 있어야 Coroutine이 몇번씩 실행되면서 스스로 반복하는 걸 막을 수 있음
                if(lockOnCoroutine != null)
                {
                    StopCoroutine(lockOnCoroutine);
                }

                lockOnCoroutine = StartCoroutine(PlayerCamera.instance.WaitThenFindNewTarget());
            }

            if (lockOn_Input && player.playerNetworkManager.isLockedOn.Value)
            {
                lockOn_Input = false;
                PlayerCamera.instance.ClearLockOnTargets();
                player.playerNetworkManager.isLockedOn.Value = false;

                // 락온 불가능
                return;
            }

            if (lockOn_Input && !player.playerNetworkManager.isLockedOn.Value)
            {
                lockOn_Input = false;

                // 만약 원거리 무기로 조준징이라면 돌아옴(락온 상태로 조준을 허용하지 않음)

                PlayerCamera.instance.HandleLocatingLockOnTargets();

                if(PlayerCamera.instance.nearestLockOnTarget != null)
                {
                    player.playerCombatManager.SetTarget(PlayerCamera.instance.nearestLockOnTarget);
                    player.playerNetworkManager.isLockedOn.Value = true;
                }
            }
        }


        private void HandleLockOnSwitchTargetInput()
        {
            if (lockOn_Left_Input)
            {
                lockOn_Left_Input = false;

                if (player.playerNetworkManager.isLockedOn.Value)
                {
                    PlayerCamera.instance.HandleLocatingLockOnTargets();

                    if (PlayerCamera.instance.leftLockOnTarget != null)
                    {
                        player.playerCombatManager.SetTarget(PlayerCamera.instance.leftLockOnTarget);
                    }
                }
            }

            if (lockOn_Right_Input)
            {
                lockOn_Right_Input = false;

                if (player.playerNetworkManager.isLockedOn.Value)
                {
                    PlayerCamera.instance.HandleLocatingLockOnTargets();

                    if (PlayerCamera.instance.rightLockOnTarget != null)
                    {
                        player.playerCombatManager.SetTarget(PlayerCamera.instance.rightLockOnTarget);
                    }
                }
            }
        }
        // 이동
        private void HandlePlayerMovementInput()
        {
            verticalInput = movementInput.y;
            horizontalInput = movementInput.x;

            // Mathf.Clamp01 = 움직임이 0~1사이란 뜻
            // 절대값을 다시 받는다.(음수를 사인으로 받지 않는다는 뜻. 항상 양수만)
            moveAmount = Mathf.Clamp01(Mathf.Abs(verticalInput) + Mathf.Abs(horizontalInput));

            // 값을 0, 0.5, 1 중 하나로 고정해준다
            if (moveAmount <= 0.5 && moveAmount > 0)
            {
                moveAmount = 0.5f;
            }
            else if (moveAmount > 0.5 && moveAmount <= 1)
            {
                moveAmount = 1;
            }

            // 왜 수평값을 0으로? = 기본적으로 측면 이동을 하지 않을 때의 애니메이션을 로드해야해서
            // 측면 이동은 측면으로만 이동하거나 락온 기능 쓸 때에만 적용할것

            // 락온 하지 않았다면, 오로지 움직일 때의 애니메이션만 사용
            if(player == null)
            {
                return;
            }

            if(moveAmount != 0)
            {
                player.playerNetworkManager.isMoving.Value = true;
            }
            else
            {
                player.playerNetworkManager.isMoving.Value = false;
            }

            if (!player.playerNetworkManager.isLockedOn.Value || player.playerNetworkManager.isSprinting.Value)
            {
                player.playerAnimatorManager.UpdateAnimatorMovementParameters(0, moveAmount, player.playerNetworkManager.isSprinting.Value);
            }
            else
            {
                player.playerAnimatorManager.UpdateAnimatorMovementParameters(horizontalInput, verticalInput, player.playerNetworkManager.isSprinting.Value);
            }

            // 만약 락온을 했다면 측면 이동값도 같이 보낸다
        }
    
        private void HandleCameraMovementInput()
        {
            cameraVerticalInput = cameraInput.y;
            cameraHorizontalInput = cameraInput.x;

            
        }

        // 액션
        private void HandleDodgeInput()
        {
            if (dodge_Input)
            {
                dodge_Input = false;
                // 미래에 구현 할 것: 메뉴 혹은 UI창이 열려 있을 경우 입력 무시하는 것(RETURN 구현)
                // 회피 동작 구현
                player.playerLocomotionManager.AttemptToPerformDodge();
            }
        }

        private void HandleSprintInput()
        {
            if (sprint_Input)
            {
                // 질주
                player.playerLocomotionManager.HandleSprinting();
            }
            else
            {
                player.playerNetworkManager.isSprinting.Value = false;
            }
        }

        private void HandleJumpInput()
        {
            if (jump_Input)
            {
                jump_Input = false;

                // 만약 UI 창이 열려있다면, 아무것도 하지 않게 설정

                // 점프 하려고 시도
                player.playerLocomotionManager.AttemptToPerformJump();
            }
        }

        private void HandleRBInput()
        {
            if (RB_Input)
            {
                RB_Input = false;

                // 해야할 거: 나중에 ui창 열려있으면, 돌아오고 아무것도 안되어야함
                player.playerNetworkManager.SetCharacterActionHand(true);

                // 해야할 거: 만약 우리가 양손무기를 들고 있다면 양손무기 모션 적용

                player.playerCombatManager.PerformWeaponBasedAction(player.playerInventoryManager.currentRightHandWeapon.oh_RB_Action, player.playerInventoryManager.currentRightHandWeapon);
            }
        }
    
        private void HandleRTInput()
        {
            if (RT_Input)
            {
                RT_Input = false;

                // 해야할 거: 나중에 ui창 열려있으면, 돌아오고 아무것도 안되어야함
                player.playerNetworkManager.SetCharacterActionHand(true);

                // 해야할 거: 만약 우리가 양손무기를 들고 있다면 양손무기 모션 적용

                player.playerCombatManager.PerformWeaponBasedAction(
                    player.playerInventoryManager.currentRightHandWeapon.oh_RT_Action, 
                    player.playerInventoryManager.currentRightHandWeapon);
            }
        }

        private void HandleChargeRTInput()
        {
            // We only want to check for a charge if we are in an Action that requires it (Attacking)
            if (player.isPerformingAction)
            {
                if (player.playerNetworkManager.isUsingRightHand.Value)
                {
                    player.playerNetworkManager.isChargingAttack.Value = Hold_RT_Input;
                }
            }
        }
    
        private void HandleSwitchRightWeaponInput()
        {
            if (switch_Right_Weapon_Input)
            {
                switch_Right_Weapon_Input = false;
                player.playerEquipmentManager.SwitchRightWeapon();
            }
        }

        private void HandleSwitchLeftWeaponInput()
        {
            if (switch_Left_Weapon_Input)
            {
                switch_Left_Weapon_Input = false;
                player.playerEquipmentManager.SwitchLeftWeapon();
            }
        }
    }    
}
