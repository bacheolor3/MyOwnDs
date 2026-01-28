using UnityEngine;

namespace TSG
{
    public class CharacterLocomotionManager : MonoBehaviour
    {
        CharacterManager character;

        [Header("지면 확인 & 점프")]
        [SerializeField] float gravityForce = -5.55f;
        [SerializeField] LayerMask groundLayer;
        [SerializeField] float groundCheckSphereRadius = 1;
        [SerializeField] protected Vector3 yVelocity;   // 캐릭터를 위 혹은 아래로 끌어당길 힘(점프를 하거나 떨어지거나)
        [SerializeField] protected float groundedYVelocity = -20;   // 캐릭터가 땅에 있는 동안, 받고 있는 힘
        [SerializeField] protected float fallStartYVelocity = -5;   // 캐릭터가 작하하는중이면 받는 중력의 힘(지속적으로 올라감)
        protected bool fallingVelocityHasBeenSet = false;
        protected float inAirTimer = 0;
        protected virtual void Awake()
        {
            character = GetComponent<CharacterManager>();
        }

        protected virtual void Update()
        {
            HandleGroundCheck();

            if (character.isGrounded)
            {
                // 만약 점프를 시도하지 않거나 움직이는 중이라면
                if(yVelocity.y < 0)
                {
                    inAirTimer = 0;
                    fallingVelocityHasBeenSet = false;
                    yVelocity.y = groundedYVelocity;
                }
            }
            else
            {
                // 만약 점프중도 아니고, 떨어지는 힘도 받고 있지 않다면
                if(character.isJumping && !fallingVelocityHasBeenSet)
                {
                    fallingVelocityHasBeenSet = true;
                    yVelocity.y = fallStartYVelocity;
                }

                inAirTimer = inAirTimer + Time.deltaTime;
                character.animator.SetFloat("InAirTimer", inAirTimer);
                
                yVelocity.y += gravityForce * Time.deltaTime;                
            }

            // 언제나 Y축(아래쪽)으로 내려지려는 힘은 적용되어야 함
            character.characterController.Move(yVelocity * Time.deltaTime);
        }

        protected void HandleGroundCheck()
        {
            character.isGrounded = Physics.CheckSphere(character.transform.position, groundCheckSphereRadius, groundLayer);
        }

        // 씬에서 바닥 확인용 구체를 그려줌
        protected void OnDrawGizmosSelected()
        {
            Gizmos.DrawSphere(character.transform.position, groundCheckSphereRadius);
        }
    }
}
