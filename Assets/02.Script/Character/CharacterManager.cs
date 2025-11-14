using UnityEngine;
using Unity.Netcode;

namespace TSG
{
    public class CharacterManager : NetworkBehaviour
    {
        public CharacterController characterController;

        CharacterNetworkManager characterNetworkManager;
        protected virtual void Awake()
        {
            DontDestroyOnLoad(this);

            characterController = GetComponent<CharacterController>();
            characterNetworkManager = GetComponent<CharacterNetworkManager>();
        }

        protected virtual void Update()
        {
            if (IsOwner)
            {
                // 만약 캐릭터가 우리쪽에서 컨트롤 되고 있다면, 그러면 그 물체의 네트워크 포지션을  우리의 포지션으로 동일시한다
                characterNetworkManager.networkPosition.Value = transform.position;
                characterNetworkManager.networkRotation.Value = transform.rotation;
            }
                // 만약 이 캐릭터가 다른데에서 컨트롤 되고 있다면, 그 다음, 이 오브젝트의 로컬 위치를 네트워크 트랜스폼(NetworkTransform)의 위치로 설정해라
            else
            {
                // 포지션
                transform.position = Vector3.SmoothDamp
                    (transform.position,
                    characterNetworkManager.networkPosition.Value,
                    ref characterNetworkManager.networkPositionVelocity,
                    characterNetworkManager.networkPositionSmoothTime);
                // 로테이션
                transform.rotation = Quaternion.Slerp
                    (transform.rotation,
                    characterNetworkManager.networkRotation.Value,
                    characterNetworkManager.networkRotationSmoothTime);
            }
        }
    }
    
}
