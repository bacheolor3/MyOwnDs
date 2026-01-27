using Unity.Netcode;
using UnityEngine;
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
        [SerializeField] Button mainMenuNewGameButton;
        [SerializeField] Button loadMenuReturnButton;
        [SerializeField] Button mainMenuLoadGameButton;
        [SerializeField] Button deleteCharacterPopUpConfirmButton;

        [Header("팝업창들")]
        [SerializeField] GameObject noCharacterSlotsPopUp;
        [SerializeField] Button nocharacterSlotsOkayButton;
        [SerializeField] GameObject deleteCharacterSlotPopUp;

        [Header("캐릭터 슬롯 관련")]
        public CharacterSlot currentSelectedSlot = CharacterSlot.NO_SLOT;

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

        public void CloseNoFreeCharcterSlotsPopUp()
        {
            noCharacterSlotsPopUp.SetActive(false);
            mainMenuNewGameButton.Select();
        }
    
        // 캐릭터 슬롯 관련
        public void SelectCharacterSlot(CharacterSlot characterSlot)
        {
            currentSelectedSlot = characterSlot;
        }
    
        public void SelectNoSlot()
        {
            currentSelectedSlot = CharacterSlot.NO_SLOT;
        }

        public void AttemptToDeleteCharacterSlot()
        {
            if(currentSelectedSlot != CharacterSlot.NO_SLOT)
            {
                deleteCharacterSlotPopUp.SetActive(true);           
                deleteCharacterPopUpConfirmButton.Select();
            }
        }

        public void DeleteCharacterSlot()
        {
            deleteCharacterSlotPopUp.SetActive(false);
            WorldSaveGameManager.instance.DeleteGame(currentSelectedSlot);

            // 불러오기 화면을 한번 해제했대가 다시 불러오기 해서 지운 슬롯들은 안보이게
            titleScreenLoadMenu.SetActive(false);
            titleScreenLoadMenu.SetActive(true);

            loadMenuReturnButton.Select();
        }

        public void CloseDeleteCharacterPopUp()
        {
            deleteCharacterSlotPopUp.SetActive(false);
            loadMenuReturnButton.Select();
        }
    }    
}
