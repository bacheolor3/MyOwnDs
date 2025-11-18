using UnityEngine;
using UnityEngine.SceneManagement;

namespace TSG
{
    public class PlayerInputManager : MonoBehaviour
    {
        public static PlayerInputManager instance;  
        // 목표를 하나하나 천천히 생각하기
        // 1. 조이스틱 값을 읽을 수 있는 방법 찾기
        // 2. 캐릭터를 그 값에 따라 움직이기
        PlayerControls playerControls;
        [Header("플레이어 움직임 입력")]
        [SerializeField] Vector2 movementInput;
        public float verticalInput;
        public float horizontalInput;
        public float moveAmount;

        [Header("카메라 움직임 입력")]
        [SerializeField] Vector2 cameraInput;
        public float cameraVerticalInput;
        public float cameraHorizontalInput;
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
        }
                
        private void OnSceneChange(Scene oldScene, Scene newScene)
        {
            // 월드 씬으로 가게 될 경우, 플레이어 컨트롤러 활성화
            if (newScene.buildIndex == WorldSaveGameManager.instance.GetWorldSceneIndex())
            {
                instance.enabled = true;
            }
            // 아니면 플레이어 컨트롤러는 반드시 비활성화 이어야 한다
            // 이는 우리가 미래에 캐릭터 크리에이션 씬 같은 걸 만들때 쓰인다
            else
            {
                instance.enabled = false;
            }
        }

        private void OnEnable()
        {
            if (playerControls == null)
            {
                playerControls = new PlayerControls();

                playerControls.PlayerMovements.Movement.performed += i => movementInput = i.ReadValue<Vector2>();
                playerControls.PlayerCamera.Movement.performed += i => cameraInput = i.ReadValue<Vector2>();
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
            HandleMovementInput();
            HandleCameraMovementInput();
        }
        private void HandleMovementInput()
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
        }
    
        private void HandleCameraMovementInput()
        {
            cameraVerticalInput = cameraInput.y;
            cameraHorizontalInput = cameraInput.x;

            
        }
    }
    
}
