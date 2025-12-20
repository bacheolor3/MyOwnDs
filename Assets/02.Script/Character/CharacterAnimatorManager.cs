using UnityEngine;

namespace TSG
{
    public class CharacterAnimatorManager : MonoBehaviour
    {
        CharacterManager character;

        float vertical;
        float horizontal;

        protected virtual void Awake()
        {
            character = GetComponent<CharacterManager>();
        }

        public void UpdateAnimatorMovementParameters(float horizontalMovement, float verticalMovement)
        {
            // Option 1 (애니메이션 품질이 괜찮을 때)
            character.animator.SetFloat("Horizontal", horizontalMovement, 0.1f, Time.deltaTime);
            character.animator.SetFloat("Vertical", verticalMovement, 0.1f, Time.deltaTime);

            // Option 2 (애니메이션 품질이 영 별로거나, 직접 만들었을 때)

            float snappedHorizontal = 0;
            float snappedVertical = 0;

            #region Horizontal
            // This if chain will round the horizontal movement to -1, -0.5, 0, 0.5 or 1

            if(horizontalMovement > 0 && horizontalMovement <= 0.5f)
            {
                snappedHorizontal = 0.5f;
            }
            else if(horizontalMovement > 0.5f && horizontalMovement <= 1f)
            {
                snappedHorizontal = 1;
            }else if (horizontalMovement < 0 && horizontalMovement >= -0.5f)
            {
                snappedHorizontal = -0.5f;
            }else if (horizontalMovement < -0.5 && horizontalMovement >= -1f)
            {
                snappedHorizontal = -1;
            }
            else
            {
                snappedHorizontal = 0;
            }

            #endregion

            #region Vertical
            // This if chain will round the vertical movement to -1, -0.5, 0, 0.5 or 1

            if(verticalMovement > 0 && verticalMovement <= 0.5f)
            {
                snappedVertical = 0.5f;
            }
            else if (verticalMovement > 0.5f && verticalMovement <= 1)
            {
                snappedVertical = 1;
            }else if (verticalMovement < 0 && verticalMovement >=  -0.5f)
            {
                snappedVertical = -0.5f;
            }else if (verticalMovement < -0.5f && verticalMovement >= -1)
            {
                snappedVertical = -1;
            }
            else
            {
                snappedVertical = 0;
            }
            #endregion
        }
    }
    
}
