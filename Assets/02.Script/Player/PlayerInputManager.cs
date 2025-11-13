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
        [SerializeField] Vector2 movement;
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
            // 씬이 바뀌면 이 로직 사용
            SceneManager.activeSceneChanged += OnSceneChange;
        }
        private void OnSceneChange(Scene oldScene, Scene newScene)
        {
            // 월드 씬으로 가게 될 경우, 플레이어 컨트롤러 활성화
            if (newScene.buildIndex == WorldSaveGameManager.instance.GetWorldSceneIndex())
            {
                instance.enabled = true;
            }
            // 아니면 플레이어 컨트롤러는 반드시 비활성화 이어야 한다
            // 이는 후리가 미래에 캐릭터 크리에이션 씬 같은 걸 만들때 쓰인다
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

                playerControls.PlayerMovements.Movement.performed += i => movement = i.ReadValue<Vector2>();
            }

            playerControls.Enable();
        }

        private void OnDestroy()
        {
            // 만약 이 오브젝트를 파괴한다면, 이 이벤트에서 벗어나기
            SceneManager.activeSceneChanged -= OnSceneChange;
        }
    }
    
}
