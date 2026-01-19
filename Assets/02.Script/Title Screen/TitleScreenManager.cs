using Unity.Netcode;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;

namespace TSG
{
    public class TitleScreenManager : MonoBehaviour
    {
        public static TitleScreenManager Instance;
        [Header("메뉴")]
        [SerializeField] GameObject titleScreenMainMenu;
        [SerializeField] GameObject titleScreenLoadMenu;
        [Header("버튼들")]
        [SerializeField] Button loadMenuReturnButton;
        [SerializeField] Button mainMenuLoadGameButton;
        [Header("팝업창들")]
        [SerializeField] GameObject noCharacterSlotsPopUp;
        [SerializeField] Button nocharacterSlotsOkayButton;

        private void Awake()
        {
            if(Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void StartNetworkAsHost()
        {
            NetworkManager.Singleton.StartHost();
        }

        public void StartNewGame()
        {
            WorldSaveGameManager.instance.AttemptToCreateNewGame();            
        }
    
        public void OpenLoadGameMenu()
        {
            // 메인 메뉴 닫기
            titleScreenMainMenu.SetActive(false);

            // 불러오기 메뉴 열기
            titleScreenLoadMenu.SetActive(true);

            // 첫번째로 로드할 슬롯 찾고 자동적으로 고르기
            loadMenuReturnButton.Select();
        }
        
        public void CloseLoadGameMenu()
        {
            // 불러오기 메뉴 닫기
            titleScreenLoadMenu.SetActive(false);

            // 메인 메뉴 열기
            titleScreenMainMenu.SetActive(true);

            // 로드 버튼 선택
            mainMenuLoadGameButton.Select();
        }
    
        public void DisplayNoFreeCharacterSlotsPopUp()
        {
            noCharacterSlotsPopUp.SetActive(true);
            nocharacterSlotsOkayButton.Select();
        }
    }    
}
