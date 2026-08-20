using UnityEngine;

namespace TSG
{
    public class CharacterLocomotionManager : MonoBehaviour
    {
        CharacterManager character;

        [Header("지면 확인 & 점프")]
        [SerializeField] protected float gravityForce = -5.55f;
        [SerializeField] LayerMask groundLayer;
        [SerializeField] float groundCheckSphereRadius = 1;
        [SerializeField] protected Vector3 yVelocity;   // 캐릭터를 위 혹은 아래로 끌어당길 힘(점프를 하거나 떨어지거나)
        [SerializeField] protected float groundedYVelocity = -20;   // 캐릭터가 땅에 있는 동안, 받고 있는 힘
        [SerializeField] protected float fallStartYVelocity = -5;   // 캐릭터가 작하하는중이면 받는 중력의 힘(지속적으로 올라감)
        protected bool fallingVelocityHasBeenSet = false;
        protected float inAirTimer = 0;

        [Header("플래그들")]
        public bool isRolling = false;
        public bool canRotate = true;
        public bool canMove = true;
        public bool isGrounded = true;
        public bool applyRootMotion = false;    
        
        protected virtual void Awake()
        {
            character = GetComponent<CharacterManager>();
        }

        protected virtual void Update()
        {
            HandleGroundCheck();

            if (character.characterLocomotionManager.isGrounded)
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
                if(!character.characterNetworkManager.isJumping.Value&& !fallingVelocityHasBeenSet)
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
            character.characterLocomotionManager.isGrounded = Physics.CheckSphere(character.transform.position, groundCheckSphereRadius, groundLayer);
        }

        // 씬 로딩/세이브 로딩으로 위치를 강제로 옮길 때, 그동안 쌓인 낙하 속도를 제거하기 위해 사용
        public void ResetVelocity()
        {
            yVelocity = Vector3.zero;
            inAirTimer = 0;
            fallingVelocityHasBeenSet = false;
        }

        // 씬에서 바닥 확인용 구체를 그려줌
        protected void OnDrawGizmosSelected()
        {
            if(character == null)
            {
                return;
            }
            Gizmos.DrawSphere(character.transform.position, groundCheckSphereRadius);
        }

        public void EnableCanRotate()
        {
            canRotate = true;
        }

        public void DisableCanRotate()
        {
            canRotate = false;
        }
    }
}
