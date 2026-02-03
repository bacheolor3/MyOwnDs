using UnityEngine;


namespace TSG
{
    public class PlayerStatsManager : CharacterStatManager
    {
        PlayerManager player;

        protected override void Awake()
        {
            base.Awake();

            player = GetComponent<PlayerManager>();
        }

        protected override void Start()
        {
            base.Start();

            // Q: 왜 여기서 계산하나요?
            // A: 캐릭터를 생성할 때, 클래스에 따라 스탯을 설정하고, 그곳에서 계산해야 하기 때문
            // 그때까지는, 스탯은 계산되지 않음. 그러니 우린 여기서 시작. 만약 세이브 파일이 존재한다면 씬 로딩 과정에서 덧씌워질것
            CalculateHealthBasedOnVitalityLevel(player.playerNetworkManager.vitality.Value);
            CalculateStatminaBasedOnEnduranceLevel(player.playerNetworkManager.endurance.Value);
        }
    }    
}
