using UnityEngine;

namespace TSG
{
    public class PlayerEquipmentManager : CharacterEquipmentManager
    {
        PlayerManager player;

        public WeaponModelInstantiationSlot rightHandSlot;
        public WeaponModelInstantiationSlot leftHandSlot;

        [SerializeField] WeaponManager rightWeaponManagaer;
        [SerializeField] WeaponManager leftWeaponManagaer;

        public GameObject rightHandWeaponModel;
        public GameObject leftHandWeaponModel;

        protected override void Awake()
        {
            base.Awake();

            player = GetComponent<PlayerManager>();

            // 슬롯들에서 정보 가져오기
            InitializeWeaponSlots();
        }

        protected override void Start()
        {
            base.Start();

            LoadWeaponsOnBothHands();
        }

        private void InitializeWeaponSlots()
        {
            WeaponModelInstantiationSlot[] weaponSlots = GetComponentsInChildren<WeaponModelInstantiationSlot>();

            foreach(var weaponSlot in weaponSlots)
            {
                if(weaponSlot.weaponSlot == WeaponModelSlot.RightHand)
                {
                    rightHandSlot = weaponSlot;
                }
                else if(weaponSlot.weaponSlot == WeaponModelSlot.LeftHand)
                {
                    leftHandSlot = weaponSlot;
                }
            }
        }
    
        public void LoadWeaponsOnBothHands()
        {
            LoadRightWeapon();
            LoadLeftWeapon();
        }

        // 오른손 무기

        public void LoadRightWeapon()
        {
            if(player.playerInventoryManager.currentRightHandWeapon != null)
            {
                // 기존에 들고 있던 무기는 없애고
                rightHandSlot.UnloadWeapon();

                // 새 무기 들고 오기
                rightHandWeaponModel = Instantiate(player.playerInventoryManager.currentRightHandWeapon.weaponModel);
                rightHandSlot.LoadWeapon(rightHandWeaponModel);
                rightWeaponManagaer = rightHandWeaponModel.GetComponent<WeaponManager>();
                rightWeaponManagaer.SetWeaponDamage(player, player.playerInventoryManager.currentRightHandWeapon);
            }
        }

        public void SwitchRightWeapon()
        {
            if (!player.IsOwner)
            {
                return;
            }

            player.playerAnimatorManager.PlayTargetActionAnimation("Swap_Right_Weapon_01", false, false, true, true);

            // 엘든링 무기 스왑 시스템
            // 1. 기본으로 들고 있는 무기 말고 다른 무기가 있는지 확인하기. 만약 있다면, '절대로' 맨손으로 바꾸지 않음. 무조건 무기끼리만 바뀔 것
            // 2. 만약 무기가 없다면, 맨손으로 바꾸고 다른 빈 슬롯들 확인하는 걸 넘기고 돌아오기. 기본 무기로 돌아가기 전에 빈 슬롯들 사이로 돌아가지 말것

            WeaponItem selectedWeapon = null;

            // 두손으로 들 수 없는 무기면 두손 들기 막아두기

            // 무기 교체를 위해 인덱스에 번호 추가해주기
            player.playerInventoryManager.rightHandWeaponIndex += 1;

            // 무기 인덱스 확인할것(슬롯이 3개, 즉 숫자는 3까지*컴퓨터식 숫자 3!!)
            // 인덱스 지정 범위를 넘어가면 첫번째 번호로 초기화할것(*컴퓨터식 숫자 1이니까 0)
            if(player.playerInventoryManager.rightHandWeaponIndex < 0 || player.playerInventoryManager.rightHandWeaponIndex > 2)
            {
                player.playerInventoryManager.rightHandWeaponIndex = 0;

                // 무기를 하나 이상 들고 있는지 확인
                float weaponCount = 0;
                WeaponItem firstWeapon = null;
                int fisrtWeaponPosition = 0;

                for(int i = 0; i < player.playerInventoryManager.weaponsInRightHandSlots.Length; i++)
                {
                    if(player.playerInventoryManager.weaponsInRightHandSlots[i].itemID != WorldItemDatabase.Instance.unarmedWeapon.itemID)
                    {
                        weaponCount += 1;

                        if(firstWeapon == null)
                        {
                            firstWeapon = player.playerInventoryManager.weaponsInRightHandSlots[i];
                            fisrtWeaponPosition = i;
                        }
                    }
                }

                if(weaponCount <= 1)
                {
                    player.playerInventoryManager.rightHandWeaponIndex= -1;
                    selectedWeapon = WorldItemDatabase.Instance.unarmedWeapon;
                    player.playerNetworkManager.currentRightHandWeaponID.Value = selectedWeapon.itemID;
                }
                else
                {
                    player.playerInventoryManager.rightHandWeaponIndex = fisrtWeaponPosition;
                    player.playerNetworkManager.currentRightHandWeaponID.Value = firstWeapon.itemID;
                }

                return;
            }

            foreach(WeaponItem weapon in player.playerInventoryManager.weaponsInRightHandSlots)
            {
                // 이 무기가 "맨손 무기"가 아닌지 확인
                // 만약 다음에 올 무기가 맨손무기가 아니라면 진행
                if(player.playerInventoryManager.weaponsInRightHandSlots[player.playerInventoryManager.rightHandWeaponIndex].itemID != 
                WorldItemDatabase.Instance.unarmedWeapon.itemID)
                {
                    selectedWeapon = player.playerInventoryManager.weaponsInRightHandSlots[player.playerInventoryManager.rightHandWeaponIndex];
                    // 네트워크에 무기 ID 를 할당해 연결된 모든 접속자들도 무기 바꾸는 게 보이게 설정
                    player.playerNetworkManager.currentRightHandWeaponID.Value = 
                    player.playerInventoryManager.weaponsInRightHandSlots[player.playerInventoryManager.rightHandWeaponIndex].itemID;
                    return;
                }
            }

            if(selectedWeapon == null && player.playerInventoryManager.rightHandWeaponIndex <= 2)
            {
                SwitchRightWeapon();
            }           
        }

        // 왼손 무기

        public void LoadLeftWeapon()
        {
            if(player.playerInventoryManager.currentLeftHandWeapon != null)
            {
                leftHandSlot.UnloadWeapon();

                leftHandWeaponModel = Instantiate(player.playerInventoryManager.currentLeftHandWeapon.weaponModel);
                leftHandSlot.LoadWeapon(leftHandWeaponModel);
                leftWeaponManagaer = leftHandWeaponModel.GetComponent<WeaponManager>();
                leftWeaponManagaer.SetWeaponDamage(player, player.playerInventoryManager.currentLeftHandWeapon);
            }
        }

        public void SwitchLeftWeapon()
        {
            if (!player.IsOwner)
            {
                return;
            }

            player.playerAnimatorManager.PlayTargetActionAnimation("Swap_Left_Weapon_01", false, false, true, true);

            // 엘든링 무기 스왑 시스템
            // 1. 기본으로 들고 있는 무기 말고 다른 무기가 있는지 확인하기. 만약 있다면, '절대로' 맨손으로 바꾸지 않음. 무조건 무기끼리만 바뀔 것
            // 2. 만약 무기가 없다면, 맨손으로 바꾸고 다른 빈 슬롯들 확인하는 걸 넘기고 돌아오기. 기본 무기로 돌아가기 전에 빈 슬롯들 사이로 돌아가지 말것

            WeaponItem selectedWeapon = null;

            // 두손으로 들 수 없는 무기면 두손 들기 막아두기

            // 무기 교체를 위해 인덱스에 번호 추가해주기
            player.playerInventoryManager.leftHandWeaponIndex += 1;

            // 무기 인덱스 확인할것(슬롯이 3개, 즉 숫자는 3까지*컴퓨터식 숫자 3!!)
            // 인덱스 지정 범위를 넘어가면 첫번째 번호로 초기화할것(*컴퓨터식 숫자 1이니까 0)
            if(player.playerInventoryManager.leftHandWeaponIndex < 0 || player.playerInventoryManager.leftHandWeaponIndex > 2)
            {
                player.playerInventoryManager.leftHandWeaponIndex = 0;

                // 무기를 하나 이상 들고 있는지 확인
                float weaponCount = 0;
                WeaponItem firstWeapon = null;
                int fisrtWeaponPosition = 0;

                for(int i = 0; i < player.playerInventoryManager.weaponsInLeftHandSlots.Length; i++)
                {
                    if(player.playerInventoryManager.weaponsInLeftHandSlots[i].itemID != WorldItemDatabase.Instance.unarmedWeapon.itemID)
                    {
                        weaponCount += 1;

                        if(firstWeapon == null)
                        {
                            firstWeapon = player.playerInventoryManager.weaponsInLeftHandSlots[i];
                            fisrtWeaponPosition = i;
                        }
                    }
                }

                if(weaponCount <= 1)
                {
                    player.playerInventoryManager.leftHandWeaponIndex= -1;
                    selectedWeapon = WorldItemDatabase.Instance.unarmedWeapon;
                    player.playerNetworkManager.currentLeftHandWeaponID.Value = selectedWeapon.itemID;
                }
                else
                {
                    player.playerInventoryManager.leftHandWeaponIndex = fisrtWeaponPosition;
                    player.playerNetworkManager.currentLeftHandWeaponID.Value = firstWeapon.itemID;
                }

                return;
            }

            foreach(WeaponItem weapon in player.playerInventoryManager.weaponsInLeftHandSlots)
            {
                // 이 무기가 "맨손 무기"가 아닌지 확인
                // 만약 다음에 올 무기가 맨손무기가 아니라면 진행
                if(player.playerInventoryManager.weaponsInLeftHandSlots[player.playerInventoryManager.leftHandWeaponIndex].itemID !=
                     WorldItemDatabase.Instance.unarmedWeapon.itemID)
                {
                    selectedWeapon = player.playerInventoryManager.weaponsInLeftHandSlots[player.playerInventoryManager.leftHandWeaponIndex];
                    // 네트워크에 무기 ID 를 할당해 연결된 모든 접속자들도 무기 바꾸는 게 보이게 설정
                    player.playerNetworkManager.currentLeftHandWeaponID.Value = 
                    player.playerInventoryManager.weaponsInLeftHandSlots[player.playerInventoryManager.leftHandWeaponIndex].itemID;
                }
            }

            if(selectedWeapon == null && player.playerInventoryManager.leftHandWeaponIndex <= 2)
            {
                SwitchLeftWeapon();
            }   
        }
    
        // 데미지 충돌 판정
        public void OpenDamageCollider()
        {
            // 오른손 무기의 충돌 판정 활성화
            if (player.playerNetworkManager.isUsingRightHand.Value)
            {
                rightWeaponManagaer.meleeDamageCollider.EnableDamageCollider();
            }
            // 왼손 무기의 충돌 판정 활성화
            else if (player.playerNetworkManager.isUsingLeftHand.Value)
            {
                leftWeaponManagaer.meleeDamageCollider.EnableDamageCollider();
            }

            // 무기 이펙트 재생
        }

        public void CloseDamageCollider()
        {
            // 오른손 무기의 충돌 판정 활성화
            if (player.playerNetworkManager.isUsingRightHand.Value)
            {
                rightWeaponManagaer.meleeDamageCollider.DisableDamageCollider();
            }
            // 왼손 무기의 충돌 판정 활성화
            else if (player.playerNetworkManager.isUsingLeftHand.Value)
            {
                leftWeaponManagaer.meleeDamageCollider.DisableDamageCollider();
            }
        }
    }    
}
