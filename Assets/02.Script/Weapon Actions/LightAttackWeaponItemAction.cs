using UnityEngine;


namespace TSG
{
    [CreateAssetMenu(menuName ="Character Actions/Weapon Actions/Light Attack Action")]
    public class LightAttackWeaponItemAction : WeaponItemAction
    {
        [SerializeField] string light_Attack_01 = "Main_Light_Attack_01";   // Main = 주 손
        public override void AttemptToPerformAction(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)
        {
            base.AttemptToPerformAction(playerPerformingAction, weaponPerformingAction);

            // 멈추기 위해 확인
            
            if (!playerPerformingAction.IsOwner)
            {
                return;
            }            

            if(playerPerformingAction.playerNetworkManager.currentStamina.Value <= 0)
            {
                return;
            }

            if (!playerPerformingAction.isGrounded)
            {
                return;
            }

            PerformLightAttack(playerPerformingAction, weaponPerformingAction);
        }

        private void PerformLightAttack(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)
        {
            if (playerPerformingAction.playerNetworkManager.isUsingRightHand.Value)
            {
                playerPerformingAction.playerAnimatorManager.PlayTargetAttackActionAnimation(AttackType.LightAttack01, light_Attack_01, true);

                CheckForCinematicFocus(playerPerformingAction);
            }
            if (playerPerformingAction.playerNetworkManager.isUsingLeftHand.Value)
            {
                
            }
        }

        private void CheckForCinematicFocus(PlayerManager player)
        {
            if(player.playerCombatManager.currentTarget != null)
            {
                if (player.playerCombatManager.currentTarget.CompareTag("Boss"))
                {
                    if(UnityEngine.Random.value <= 0.5f)
                    {
                        if(PlayerCamera.instance != null)
                        {
                            PlayerCamera.instance.TriggerCinematicFocus(player.playerCombatManager.currentTarget.transform, 0.4f);
                        }
                    }
                }
            }
        }
    }    
}
