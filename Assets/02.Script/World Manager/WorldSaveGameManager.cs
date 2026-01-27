using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TSG
{
    public class WorldSaveGameManager : MonoBehaviour
    {
        public static WorldSaveGameManager instance;
        public PlayerManager player;

        [Header("SAVE/LOAD")]
        [SerializeField] bool saveGame;
        [SerializeField] bool loadGame;

        [Header("씬 번호(Index)")]
        [SerializeField] int worldSceneIndex = 1;

        [Header("세이브 파일 작성")]
        private SaveFileDataWriter saveFileDataWriter;

        [Header("현재 사용되기 있는 캐릭터 슬롯")]
        public CharacterSlot currentCharacterSlotBeingUsed;
        public CharacterSaveData currentCharacterData;
        private string saveFileName;

        [Header("캐릭터 슬롯들")]
        public CharacterSaveData characterSlot01;
        public CharacterSaveData characterSlot02;
        public CharacterSaveData characterSlot03;
        public CharacterSaveData characterSlot04;
        public CharacterSaveData characterSlot05;
        public CharacterSaveData characterSlot06;
        public CharacterSaveData characterSlot07;
        public CharacterSaveData characterSlot08;
        public CharacterSaveData characterSlot09;
        public CharacterSaveData characterSlot10;


        private void Awake()
        {
            // 하나의 Instance만 가질 수 있음, 만약 다른게 있다면 파괴할것
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
            LoadAllCharacterProfiles();
        }

        private void Update()
        {
            if (saveGame)
            {
                saveGame = false;
                SaveGame();
            }

            if (loadGame)
            {
                loadGame = false;
                LoadGame();
            }
        }

        public string DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot characterSlot)
        {
            string fileName = "";
            // switch문구에 알아서 case 들 추가하기 => switch 에 변수명 넣은 다음 커서 올리고 Ctrl+. ->Add missing case
            switch (characterSlot)
            {
                case CharacterSlot.CharacterSlot_01:
                    fileName = "CharacterSlot_01";
                    break;
                case CharacterSlot.CharacterSlot_02:
                    fileName = "CharacterSlot_02";
                    break;
                case CharacterSlot.CharacterSlot_03:
                    fileName = "CharacterSlot_03";
                    break;
                case CharacterSlot.CharacterSlot_04:
                    fileName = "CharacterSlot_04";
                    break;
                case CharacterSlot.CharacterSlot_05:
                    fileName = "CharacterSlot_05";
                    break;
                case CharacterSlot.CharacterSlot_06:
                    fileName = "CharacterSlot_06";
                    break;
                case CharacterSlot.CharacterSlot_07:
                    fileName = "CharacterSlot_07";
                    break;
                case CharacterSlot.CharacterSlot_08:
                    fileName = "CharacterSlot_08";
                    break;
                case CharacterSlot.CharacterSlot_09:
                    fileName = "CharacterSlot_09";
                    break;
                case CharacterSlot.CharacterSlot_10:
                    fileName = "CharacterSlot_10";
                    break;
                default:
                    break;
            }

            return fileName;
        }
        
        public void AttemptToCreateNewGame()
        {
            Debug.Log("세이브 찐빠 확인용");
            saveFileDataWriter = new SaveFileDataWriter();
            saveFileDataWriter.saveDataDirectoryPath = Application.persistentDataPath;
            // 새로운 세이브 파일을 만들 수 있는지 체크 (다른 파일들의 존재 유무 확인 먼저)
            saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_01);
            //  만약 이 프로필 슬롯이 비어있다면, 이 슬롯의 자리를 차지한다
            if (!saveFileDataWriter.CheckToSeeIfFileExists())
            {
                // 만약 이 프로필 슬롯이 비어있지 않다면, 이 슬롯을 쓰는 새로운 걸 만듬
                currentCharacterSlotBeingUsed = CharacterSlot.CharacterSlot_01;
                currentCharacterData = new CharacterSaveData();
                StartCoroutine(LoadWorldScene());
                return;
            }
           
            // 새로운 세이브 파일을 만들 수 있는지 체크 (다른 파일들의 존재 유무 확인 먼저)
            saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_02);

            //  만약 이 프로필 슬롯이 비어있다면, 이 슬롯의 자리를 차지한다
            if (!saveFileDataWriter.CheckToSeeIfFileExists())
            {
                // 만약 이 프로필 슬롯이 비어있지 않다면, 이 슬롯을 쓰는 새로운 걸 만듬
                currentCharacterSlotBeingUsed = CharacterSlot.CharacterSlot_02;
                currentCharacterData = new CharacterSaveData();
                StartCoroutine(LoadWorldScene());
                return;
            }

            // 새로운 세이브 파일을 만들 수 있는지 체크 (다른 파일들의 존재 유무 확인 먼저)
            saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_03);

            //  만약 이 프로필 슬롯이 비어있다면, 이 슬롯의 자리를 차지한다
            if (!saveFileDataWriter.CheckToSeeIfFileExists())
            {
                // 만약 이 프로필 슬롯이 비어있지 않다면, 이 슬롯을 쓰는 새로운 걸 만듬
                currentCharacterSlotBeingUsed = CharacterSlot.CharacterSlot_03;
                currentCharacterData = new CharacterSaveData();
                StartCoroutine(LoadWorldScene());
                return;
            }

            // 새로운 세이브 파일을 만들 수 있는지 체크 (다른 파일들의 존재 유무 확인 먼저)
            saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_04);

            //  만약 이 프로필 슬롯이 비어있다면, 이 슬롯의 자리를 차지한다
            if (!saveFileDataWriter.CheckToSeeIfFileExists())
            {
                // 만약 이 프로필 슬롯이 비어있지 않다면, 이 슬롯을 쓰는 새로운 걸 만듬
                currentCharacterSlotBeingUsed = CharacterSlot.CharacterSlot_04;
                currentCharacterData = new CharacterSaveData();
                StartCoroutine(LoadWorldScene());
                return;
            }

            // 새로운 세이브 파일을 만들 수 있는지 체크 (다른 파일들의 존재 유무 확인 먼저)
            saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_05);

            //  만약 이 프로필 슬롯이 비어있다면, 이 슬롯의 자리를 차지한다
            if (!saveFileDataWriter.CheckToSeeIfFileExists())
            {
                // 만약 이 프로필 슬롯이 비어있지 않다면, 이 슬롯을 쓰는 새로운 걸 만듬
                currentCharacterSlotBeingUsed = CharacterSlot.CharacterSlot_05;
                currentCharacterData = new CharacterSaveData();
                StartCoroutine(LoadWorldScene());
                return;
            }

            // 새로운 세이브 파일을 만들 수 있는지 체크 (다른 파일들의 존재 유무 확인 먼저)
            saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_06);

            //  만약 이 프로필 슬롯이 비어있다면, 이 슬롯의 자리를 차지한다
            if (!saveFileDataWriter.CheckToSeeIfFileExists())
            {
                // 만약 이 프로필 슬롯이 비어있지 않다면, 이 슬롯을 쓰는 새로운 걸 만듬
                currentCharacterSlotBeingUsed = CharacterSlot.CharacterSlot_06;
                currentCharacterData = new CharacterSaveData();
                StartCoroutine(LoadWorldScene());
                return;
            }

            // 새로운 세이브 파일을 만들 수 있는지 체크 (다른 파일들의 존재 유무 확인 먼저)
            saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_07);

            //  만약 이 프로필 슬롯이 비어있다면, 이 슬롯의 자리를 차지한다
            if (!saveFileDataWriter.CheckToSeeIfFileExists())
            {
                // 만약 이 프로필 슬롯이 비어있지 않다면, 이 슬롯을 쓰는 새로운 걸 만듬
                currentCharacterSlotBeingUsed = CharacterSlot.CharacterSlot_07;
                currentCharacterData = new CharacterSaveData();
                StartCoroutine(LoadWorldScene());
                return;
            }

            // 새로운 세이브 파일을 만들 수 있는지 체크 (다른 파일들의 존재 유무 확인 먼저)
            saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_08);

            //  만약 이 프로필 슬롯이 비어있다면, 이 슬롯의 자리를 차지한다
            if (!saveFileDataWriter.CheckToSeeIfFileExists())
            {
                // 만약 이 프로필 슬롯이 비어있지 않다면, 이 슬롯을 쓰는 새로운 걸 만듬
                currentCharacterSlotBeingUsed = CharacterSlot.CharacterSlot_08;
                currentCharacterData = new CharacterSaveData();
                StartCoroutine(LoadWorldScene());
                return;
            }

            // 새로운 세이브 파일을 만들 수 있는지 체크 (다른 파일들의 존재 유무 확인 먼저)
            saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_09);

            //  만약 이 프로필 슬롯이 비어있다면, 이 슬롯의 자리를 차지한다
            if (!saveFileDataWriter.CheckToSeeIfFileExists())
            {
                // 만약 이 프로필 슬롯이 비어있지 않다면, 이 슬롯을 쓰는 새로운 걸 만듬
                currentCharacterSlotBeingUsed = CharacterSlot.CharacterSlot_09;
                currentCharacterData = new CharacterSaveData();
                StartCoroutine(LoadWorldScene());
                return;
            }

            // 새로운 세이브 파일을 만들 수 있는지 체크 (다른 파일들의 존재 유무 확인 먼저)
            saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_10);

            //  만약 이 프로필 슬롯이 비어있다면, 이 슬롯의 자리를 차지한다
            if (!saveFileDataWriter.CheckToSeeIfFileExists())
            {
                // 만약 이 프로필 슬롯이 비어있지 않다면, 이 슬롯을 쓰는 새로운 걸 만듬
                currentCharacterSlotBeingUsed = CharacterSlot.CharacterSlot_10;
                currentCharacterData = new CharacterSaveData();
                StartCoroutine(LoadWorldScene());
                return;
            }

            // 충분한 슬롯이 없다면, 플레이어에게 알릴것
            TitleScreenManager.Instance.DisplayNoFreeCharacterSlotsPopUp();
        }

        public void LoadGame()
        {
            // 기존 파일 불러옴. 파일명은 사용하는 슬롯을 따라감
            saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(currentCharacterSlotBeingUsed);

            saveFileDataWriter = new SaveFileDataWriter();
            // 기본적으로 어지간한 운영체제(컴퓨터, 안드로이드, iOS등)에서 돌아감
            saveFileDataWriter.saveDataDirectoryPath = Application.persistentDataPath;
            saveFileDataWriter.saveFileName = saveFileName;
            currentCharacterData = saveFileDataWriter.LoadSaveFile();

            StartCoroutine(LoadWorldScene());
        }

        public void SaveGame()
        {
            // 현시점 사용하는 파일을 사용하는 슬롯 명에 따라 저장함
            saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(currentCharacterSlotBeingUsed);

            saveFileDataWriter = new SaveFileDataWriter();
            // 기본적으로 어지간한 운영체제(컴퓨터, 안드로이드, iOS등)에서 돌아감
            saveFileDataWriter.saveDataDirectoryPath = Application.persistentDataPath;
            saveFileDataWriter.saveFileName = saveFileName;

            // 세이브 파일에서 플레이어 정보를 받아와 게임에 반영
            player.SaveGameDataToCurrentCharacterData(ref currentCharacterData);

            // 받아온 정보를 JSON화 해 이 운영체제에 저장
            saveFileDataWriter.CreateNewCharacterSaveFile(currentCharacterData);
        }

        public void DeleteGame(CharacterSlot characterSlot)
        {
            // 이름 기반해 파일 찾기
            saveFileDataWriter = new SaveFileDataWriter();
            saveFileDataWriter.saveDataDirectoryPath = Application.persistentDataPath;
            saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);

            saveFileDataWriter.DeleteSaveFile();
        }

        // 게임 시작할 때, 모든 캐릭터의 정보를 기기에 불러오도록 설정
        private void LoadAllCharacterProfiles()
        {
            saveFileDataWriter = new SaveFileDataWriter();
            saveFileDataWriter.saveDataDirectoryPath = Application.persistentDataPath;
            
            saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_01);
            characterSlot01 = saveFileDataWriter.LoadSaveFile();

            saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_02);
            characterSlot02 = saveFileDataWriter.LoadSaveFile();

            saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_03);
            characterSlot03 = saveFileDataWriter.LoadSaveFile();

            saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_04);
            characterSlot04 = saveFileDataWriter.LoadSaveFile();

            saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_05);
            characterSlot05 = saveFileDataWriter.LoadSaveFile();

            saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_06);
            characterSlot06 = saveFileDataWriter.LoadSaveFile();

            saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_07);
            characterSlot07 = saveFileDataWriter.LoadSaveFile();

            saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_08);
            characterSlot08 = saveFileDataWriter.LoadSaveFile();

            saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_09);
            characterSlot09 = saveFileDataWriter.LoadSaveFile();

            saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_10);
            characterSlot10 = saveFileDataWriter.LoadSaveFile();
        }

        public IEnumerator LoadWorldScene()
        {
            // 그냥 씬 1개만 쓴다면 이걸 쓸 것
            AsyncOperation loadOperation = SceneManager.LoadSceneAsync(worldSceneIndex);

            // 다른 레벨마다 다른 신을 불러오고 싶다면 이걸 쓸 것
            // AsyncOperation loadOperation = SceneManager.LoadSceneAsync(currentCharacterData.sceneIndex);

            player.LoadGameDataFromCurrentCharacterData(ref currentCharacterData);

            yield return null;
        }

        // 다수의 씬을 설정하고 싶다면 쓸 것. 새 캐릭터에겐 현시점 씬의 인덱스가 없다.
        // private IEnumerator LoadWorldSceneNewGame()
        // {
            
        // }

        public int GetWorldSceneIndex()
        {
            return worldSceneIndex;
        }
    }

}
