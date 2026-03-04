using UnityEngine;

namespace TSG
{
    [CreateAssetMenu(menuName ="Character Actions/Weapon Actions/Test Action")]
    public class WeaponItemAction : ScriptableObject
    {
        public int actionID;
        
        public virtual void AttemptToPerformAction(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)
        {
            // 무슨 무기들이 공통적인 액션을 가지고 있는가?
            // 1. 항상 어떤 무기를 쓰고 있는지 확인할것
            if (playerPerformingAction.IsOwner)
            {
                playerPerformingAction.playerNetworkManager.currentWeaponBeingUsed.Value = weaponPerformingAction.itemID;
            }

            Debug.Log("액션이 실행됨!");
        }
    }    
}
