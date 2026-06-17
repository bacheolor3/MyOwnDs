using UnityEngine;


namespace TSG
{
    public class AICharacterAnimatorManager : CharacterAnimatorManager
    {
        AICharacterManager aICharacter;

        protected override void Awake()
        {
            base.Awake();

            aICharacter = GetComponent<AICharacterManager>();
        }

        private void OnAnimatorMove()
        {
            // 호스트
            if (aICharacter.IsOwner)
            {
                if (!aICharacter.isGrounded)
                {
                    return;
                }

                Vector3 velocity = aICharacter.animator.deltaPosition;

                aICharacter.characterController.Move(velocity);
                aICharacter.transform.rotation *= aICharacter.animator.deltaRotation;
            }
            // 클라이언트
            else
            {
                if (!aICharacter.isGrounded)
                {
                    return;
                }

                Vector3 velocity = aICharacter.animator.deltaPosition;

                aICharacter.characterController.Move(velocity);
                aICharacter.transform.position = Vector3.SmoothDamp(transform.position, 
                    aICharacter.characterNetworkManager.networkPosition.Value, 
                    ref aICharacter.characterNetworkManager.networkPositionVelocity,
                    aICharacter.characterNetworkManager.networkPositionSmoothTime);
                aICharacter.transform.rotation *= aICharacter.animator.deltaRotation;
            }
        }
    }    
}
