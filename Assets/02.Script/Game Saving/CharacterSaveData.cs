using UnityEngine;


namespace TSG
{
    [System.Serializable]
    // 모든 세이브 파일에 이 데이터 파일이 있을 것이기에, 이 스크립트는 모노비헤이비어가 아니라 시리어라이즈블이어야 함
    public class CharacterSaveData
    {
        [Header("SCENE INDEX")]
        public int sceneIndex =1 ;

        [Header("Character Name")]
        public string characterName = "Character";

        [Header("Time Played")]
        public float secondsPlayed;

        // Q:왜 그냥 Vector3 안 써요?
        // A:데이터로 저장할 때에는 "기본"적인 변수만 저장할 수 있어서(Float, Int, String, Bool, 등...)
        [Header("World Coordinates")]
        public float xPosition;
        public float yPosition;
        public float zPosition;

        [Header("Resources")]
        public int currentHealth;
        public float currentStamina;

        [Header("Stats")]
        public int vitality;
        public int endurance;
    }    
}
