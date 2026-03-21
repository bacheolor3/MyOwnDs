using UnityEngine;


namespace TSG
{
    [CreateAssetMenu(menuName ="Character Actions/Weapon Actions/Light Attack Action")]
    public class LightAttackWeaponItemAction : WeaponItemAction
    {
        [SerializeField] string light_Attack_01 = "Main_Light_Attack_01";   // Main = 주 손
        [SerializeField] string light_Attack_02 = "Main_Light_Attack_02";
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
            // 만약 공격중이고, 콤보를 넣을 수 있다면, 콤보 공격 실행
            if(playerPerformingAction.playerCombatManager.canComboWithMainHandWeapon && playerPerformingAction.isPerformingAction)
            {
                playerPerformingAction.playerCombatManager.canComboWithMainHandWeapon = false;

                // 이전에 실행한 공격에 기반한 공격을 실행
                if(playerPerformingAction.characterCombatManager.lastAttackAnimationPerformed == light_Attack_01)
                {
                    playerPerformingAction.playerAnimatorManager.PlayTargetAttackActionAnimation(AttackType.LightAttack02, light_Attack_02, true);
                }
                else
                {
                    playerPerformingAction.playerAnimatorManager.PlayTargetAttackActionAnimation(AttackType.LightAttack01, light_Attack_01, true);
                }
            }
            // 만약 공격중이 아니라면 그냥 일반 공격 실행
            else if(!playerPerformingAction.isPerformingAction)
            {
                playerPerformingAction.playerAnimatorManager.PlayTargetAttackActionAnimation(AttackType.LightAttack01, light_Attack_01, true);
            }
        }

        // 카메라 줌 인 되는 효과 넣어둔거....나중에 손댈것!
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
