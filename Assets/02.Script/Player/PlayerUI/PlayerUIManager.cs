using UnityEngine;
using Unity.Netcode;
using System.Collections;

namespace TSG
{
    public class PlayerUIManager : MonoBehaviour
    {

        public static PlayerUIManager instance;

        [Header("네트워크 참가용")]
        [SerializeField] bool startGameAsClient;
        [HideInInspector] public PlayerUIHudManager playerUIHudManager;
        [HideInInspector] public PlayerUIPopUpManager playerUIPopUpManager;

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

            playerUIHudManager = GetComponentInChildren<PlayerUIHudManager>();
            playerUIPopUpManager = GetComponentInChildren<PlayerUIPopUpManager>();
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
                 StartCoroutine(RestartAsClientCoroutine());
                
                // 일단 매니저를 셧다운 시켜야 함. 왜냐하면 타이틀 스크린에선 모두 호스트취급
                //NetworkManager.Singleton.Shutdown();
                // 그리고 다시 시작, 이번엔 클라이언트로서
                //NetworkManager.Singleton.StartClient();                
            }
        }

        private IEnumerator RestartAsClientCoroutine()
        {
            NetworkManager.Singleton.Shutdown();
            while (NetworkManager.Singleton.IsListening) yield return null;

            // 모든 프로필 로드
            WorldSaveGameManager.instance.LoadAllCharacterProfiles();

            // 중요: 여기서 클라이언트가 사용할 슬롯의 데이터를 currentCharacterData에 넣어줘야 합니다.
            // (예: 슬롯 1번을 사용한다고 가정할 때)
            WorldSaveGameManager.instance.currentCharacterData = WorldSaveGameManager.instance.characterSlot01; 

            yield return new WaitForSeconds(1f);
            NetworkManager.Singleton.StartClient();
        }
    }
    
}
