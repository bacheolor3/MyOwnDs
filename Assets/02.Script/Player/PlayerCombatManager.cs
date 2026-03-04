using UnityEngine;

namespace TSG
{
    public class PlayerCombatManager : CharacterCombatManager
    {
        PlayerManager player;

        public WeaponItem currentWeaponBeingUsed;

        protected override void Awake()
        {
            base.Awake();

            player = GetComponent<PlayerManager>();
        }

        public void PerformWeaponBasedAction(WeaponItemAction weaponAction, WeaponItem weaponPerformingAction)
        {
            // 액션할때 실행
            weaponAction.AttemptToPerformAction(player,weaponPerformingAction);

            // 액션을 실행한 서버는, 다른 클라이언트들의 액션도 실행해야함
        }
    }    
}
