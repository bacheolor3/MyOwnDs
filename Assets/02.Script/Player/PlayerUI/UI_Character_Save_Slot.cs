using UnityEngine;
using TMPro;

namespace TSG
{
    public class UI_Character_Save_Slot : MonoBehaviour
    {
        SaveFileDataWriter saveFileWriter;

        [Header("게임 슬롯")]
        public CharacterSlot characterSlot;

        [Header("캐릭터 정보")]
        public TextMeshProUGUI characterName;
        public TextMeshProUGUI timedPlayed;

        private void OnEnable()
        {
            LoadSaveSlots();
        }

        private void LoadSaveSlots()
        {
            saveFileWriter = new SaveFileDataWriter();
            saveFileWriter.saveDataDirectoryPath = Application.persistentDataPath;

            // 세이브 슬롯 01번 참조
            if(characterSlot == CharacterSlot.CharacterSlot_01)
            {
                saveFileWriter.saveFileName = WorldSaveGameManager.instance.DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);

                // 만약 파일이 있다면, 파일의 정보를 가져옴
                if (saveFileWriter.CheckToSeeIfFileExists())
                {
                    characterName.text = WorldSaveGameManager.instance.characterSlot01.characterName;
                }
                // 없다면, 게임 오브젝트 비활성화
                else
                {
                    gameObject.SetActive(false);
                }
            }
            // 세이브 슬롯 02번 참조
            else if(characterSlot == CharacterSlot.CharacterSlot_02)
            {
                saveFileWriter.saveFileName = WorldSaveGameManager.instance.DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);

                // 만약 파일이 있다면, 파일의 정보를 가져옴
                if (saveFileWriter.CheckToSeeIfFileExists())
                {
                    characterName.text = WorldSaveGameManager.instance.characterSlot02.characterName;
                }
                // 없다면, 게임 오브젝트 비활성화
                else
                {
                    gameObject.SetActive(false);
                }
            }
            // 세이브 슬롯 03번 참조
            else if(characterSlot == CharacterSlot.CharacterSlot_03)
            {
                saveFileWriter.saveFileName = WorldSaveGameManager.instance.DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);

                // 만약 파일이 있다면, 파일의 정보를 가져옴
                if (saveFileWriter.CheckToSeeIfFileExists())
                {
                    characterName.text = WorldSaveGameManager.instance.characterSlot03.characterName;
                }
                // 없다면, 게임 오브젝트 비활성화
                else
                {
                    gameObject.SetActive(false);
                }
            }
            // 세이브 슬롯 04번 참조
            else if(characterSlot == CharacterSlot.CharacterSlot_04)
            {
                saveFileWriter.saveFileName = WorldSaveGameManager.instance.DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);

                // 만약 파일이 있다면, 파일의 정보를 가져옴
                if (saveFileWriter.CheckToSeeIfFileExists())
                {
                    characterName.text = WorldSaveGameManager.instance.characterSlot04.characterName;
                }
                // 없다면, 게임 오브젝트 비활성화
                else
                {
                    gameObject.SetActive(false);
                }
            }
            // 세이브 슬롯 05번 참조
            else if(characterSlot == CharacterSlot.CharacterSlot_05)
            {
                saveFileWriter.saveFileName = WorldSaveGameManager.instance.DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);

                // 만약 파일이 있다면, 파일의 정보를 가져옴
                if (saveFileWriter.CheckToSeeIfFileExists())
                {
                    characterName.text = WorldSaveGameManager.instance.characterSlot05.characterName;
                }
                // 없다면, 게임 오브젝트 비활성화
                else
                {
                    gameObject.SetActive(false);
                }
            }
            // 세이브 슬롯 06번 참조
            else if(characterSlot == CharacterSlot.CharacterSlot_06)
            {
                saveFileWriter.saveFileName = WorldSaveGameManager.instance.DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);

                // 만약 파일이 있다면, 파일의 정보를 가져옴
                if (saveFileWriter.CheckToSeeIfFileExists())
                {
                    characterName.text = WorldSaveGameManager.instance.characterSlot06.characterName;
                }
                // 없다면, 게임 오브젝트 비활성화
                else
                {
                    gameObject.SetActive(false);
                }
            }
            // 세이브 슬롯 07번 참조
            else if(characterSlot == CharacterSlot.CharacterSlot_07)
            {
                saveFileWriter.saveFileName = WorldSaveGameManager.instance.DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);

                // 만약 파일이 있다면, 파일의 정보를 가져옴
                if (saveFileWriter.CheckToSeeIfFileExists())
                {
                    characterName.text = WorldSaveGameManager.instance.characterSlot07.characterName;
                }
                // 없다면, 게임 오브젝트 비활성화
                else
                {
                    gameObject.SetActive(false);
                }
            }
            // 세이브 슬롯 08번 참조
            else if(characterSlot == CharacterSlot.CharacterSlot_08)
            {
                saveFileWriter.saveFileName = WorldSaveGameManager.instance.DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);

                // 만약 파일이 있다면, 파일의 정보를 가져옴
                if (saveFileWriter.CheckToSeeIfFileExists())
                {
                    characterName.text = WorldSaveGameManager.instance.characterSlot08.characterName;
                }
                // 없다면, 게임 오브젝트 비활성화
                else
                {
                    gameObject.SetActive(false);
                }
            }
            // 세이브 슬롯 09번 참조
            else if(characterSlot == CharacterSlot.CharacterSlot_09)
            {
                saveFileWriter.saveFileName = WorldSaveGameManager.instance.DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);

                // 만약 파일이 있다면, 파일의 정보를 가져옴
                if (saveFileWriter.CheckToSeeIfFileExists())
                {
                    characterName.text = WorldSaveGameManager.instance.characterSlot09.characterName;
                }
                // 없다면, 게임 오브젝트 비활성화
                else
                {
                    gameObject.SetActive(false);
                }
            }
            // 세이브 슬롯 10번 참조
            else if(characterSlot == CharacterSlot.CharacterSlot_10)
            {
                saveFileWriter.saveFileName = WorldSaveGameManager.instance.DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);

                // 만약 파일이 있다면, 파일의 정보를 가져옴
                if (saveFileWriter.CheckToSeeIfFileExists())
                {
                    characterName.text = WorldSaveGameManager.instance.characterSlot10.characterName;
                }
                // 없다면, 게임 오브젝트 비활성화
                else
                {
                    gameObject.SetActive(false);
                }
            }

            // switch (characterSlot)
            // {
            //     case CharacterSlot.CharacterSlot_01:
            //         break;
            //     case CharacterSlot.CharacterSlot_02:
            //         break;
            //     case CharacterSlot.CharacterSlot_03:
            //         break;
            //     case CharacterSlot.CharacterSlot_04:
            //         break;
            //     case CharacterSlot.CharacterSlot_05:
            //         break;
            //     case CharacterSlot.CharacterSlot_06:
            //         break;
            //     case CharacterSlot.CharacterSlot_07:
            //         break;
            //     case CharacterSlot.CharacterSlot_08:
            //         break;
            //     case CharacterSlot.CharacterSlot_09:
            //         break;
            //     case CharacterSlot.CharacterSlot_10:
            //         break;
            //     default:
            //         break;
            // }
            //강사는 이 상황에서는 IF문을 선호한다고 함. 하나하나 뜯어보기 수월하다고...
        }
    
        public void LoadGameFromCharacterSlot()
        {
            WorldSaveGameManager.instance.currentCharacterSlotBeingUsed = characterSlot;
            WorldSaveGameManager.instance.LoadGame();
        }
    }    
}
