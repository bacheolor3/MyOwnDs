using UnityEngine;
using System;
using System.IO;
using UnityEditor.Experimental.GraphView;

namespace TSG
{
    public class SaveFileDataWriter
    {
        public string saveDataDirectoryPath = "";
        public string saveFileName = "";

        // 파일을 저장하기 전에, 이 캐릭터가 전에 만들어진 세이브 슬롯이 있냐 없냐를 체크해야 함(최대 10개의 캐릭터 슬롯)
        public bool CheckToSeeIfFileExists()
        {
            if(File.Exists(Path.Combine(saveDataDirectoryPath, saveFileName)))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        // 미래에 캐릭터 슬롯을 지우기 위한 스크립트
        public void DeleteSaveFile()
        {
            File.Delete(Path.Combine(saveDataDirectoryPath, saveFileName));
        }

        // 새 게임 시작하면서 세이브 파일 만들기 용 스크립트
        public void CreateNewCharacterSaveFile(CharacterSaveData characterData)
        {
            // 세이브 파일을 저장할 경로를 생성(로컬 머신에 저장)
            string savePath = Path.Combine(saveDataDirectoryPath, saveFileName);

            try
            {
                // 지정된 경로에 세이브 폴더가 없다면 세이브 폴더 생성
                Directory.CreateDirectory(Path.GetDirectoryName(savePath));
                Debug.Log("세이브 파일 생성 중, 저장 경로: "+ savePath);

                // C#게임 데이터 오브젝트를 JSON으로 직렬화
                string datatToString = JsonUtility.ToJson(characterData, true);

                // 파일을 적용하도록 설정
                using(FileStream stream = new FileStream(savePath, FileMode.Create))
                {
                    using(StreamWriter fileWriter = new StreamWriter(stream))
                    {
                        fileWriter.Write(datatToString);
                    }
                }
            }catch (Exception ex)
            {
                Debug.LogError("저장 중 오류 발생, 저장되지 않았음" + savePath +"\n"+ ex);
            }
        }
    
        // 이전 게임에서 저장된 세이브 파일 불러오기 용 스크립트
        public CharacterSaveData LoadSaveFile()
        {
            CharacterSaveData characterData = null;
            
            // 파일을 로드하기 위한 경로 생성(로컬 머신에서)
            string loadPath = Path.Combine(saveDataDirectoryPath, saveFileName);

            if (File.Exists(loadPath))
            {
                try
                {
                    string dataToLoad = "";

                using (FileStream stream = new FileStream(loadPath, FileMode.Open))
                {
                    using(StreamReader reader = new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }

                // 디시리얼라즈(비직렬화) 한 데이터를 JSON에서 유니티로 옮기기
                characterData = JsonUtility.FromJson<CharacterSaveData>(dataToLoad);
                }
                catch(Exception ex)
                {
                    
                }
                
            }

            return characterData;
        }
    }
}
