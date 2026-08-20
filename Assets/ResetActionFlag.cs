using UnityEngine;
using UnityEngine.TextCore.Text;

namespace TSG
{
    public class ResetActionFlag : StateMachineBehaviour
    {
        CharacterManager character;

        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        // 지정된 상태(State)에 들어갈 시 항상 이 로직을 실행한다는 뜻
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
           if(character == null)
            {
                character = animator.GetComponent<CharacterManager>();
            }

            // 액션이 끝나면 이 기준점을 리셋
            character.isPerformingAction = false;
            character.characterAnimatorManager.applyRootMotion = false;
            character.characterLocomotionManager.canRotate = true;
            character.characterLocomotionManager.canMove = true;
            character.characterLocomotionManager.isRolling = false;
            character.characterAnimatorManager.DisableCanDoCombo();

            if (character.IsOwner)
            {
                character.characterNetworkManager.isJumping.Value = false;                
            }
        }

        // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
        //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        //{
        //    
        //}

        // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
        //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        //{
        //    
        //}

        // OnStateMove is called right after Animator.OnAnimatorMove()
        //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        //{
        //    // Implement code that processes and affects root motion
        //}

        // OnStateIK is called right after Animator.OnAnimatorIK()
        //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        //{
        //    // Implement code that sets up animation IK (inverse kinematics)
        //}
    }
    
}
