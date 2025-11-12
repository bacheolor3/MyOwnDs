using UnityEngine;
using Unity.Netcode;

namespace TSG
{
    public class PlayerUIManager : MonoBehaviour
    {

        public static PlayerUIManager instance;

        [Header("네트워크 참가용")]
        [SerializeField] bool startGameAsClient;

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
        }
        private void Update()
        {
            if (startGameAsClient)
            {
                startGameAsClient = false;
                // 일단 매니저를 셧다운 시켜야 함. 왜냐하면 타이틀 스크린에선 모두 호스트취급
                NetworkManager.Singleton.Shutdown();
                // 그리고 다시 시작, 이번엔 클라이언트로서
                NetworkManager.Singleton.StartClient();

            }
        }


    }
    
}
