using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

namespace TSG
{
    public class AICharacterCombatManager : CharacterCombatManager
    {
        [Header("Detection")]
        [SerializeField] float detectionRadius = 15;
        [SerializeField] float minimumDetectionAngle = -35;
        [SerializeField] float maximumDetectionAngle = 35;
        public void FindATargetViaLineOfSight(AICharacterManager aiCharacter)
        {
            if(currentTarget != null)
            {
                return;
            }

            Collider[] colliders = Physics.OverlapSphere(aiCharacter.transform.position, detectionRadius, WorldUtilityManager.Instance.GetCharacterLayers());

            for(int i = 0; i < colliders.Length; i++)
            {
                CharacterManager targetCharacter = colliders[i].transform.GetComponent<CharacterManager>();

                if(targetCharacter == null)
                {
                    continue;
                }

                if(targetCharacter == aiCharacter)
                {
                    continue;
                }

                if (targetCharacter.isDead.Value)
                {
                    continue;
                }

                // 내가 이 캐릭터를 공격할 수 있는지 확인, 만약 가능하다면 그 캐릭터를 타겟으로 확정
                if(WorldUtilityManager.Instance.CanIDamageThisTarget(aiCharacter.characterGroup, targetCharacter.characterGroup))
                {
                    // 만약 잠재적인 타겟이 발견된다면, 그 타겟이 앞에 있어야 함
                    Vector3 targetsDirection = targetCharacter.transform.position - aiCharacter.transform.position;
                    float viewableAngle = Vector3.Angle(targetsDirection, aiCharacter.transform.forward);

                    if(viewableAngle > minimumDetectionAngle && viewableAngle < maximumDetectionAngle)
                    {
                        // 마지막으로 환경 블록들 확인
                        if(Physics.Linecast(
                            aiCharacter.characterCombatManager.lockOnTransform.position,
                            targetCharacter.characterCombatManager.lockOnTransform.position, 
                            WorldUtilityManager.Instance.GetEnviroLayers()))
                        {
                            Debug.Log($"현재 각도: {viewableAngle} / 제한 범위: {minimumDetectionAngle} ~ {maximumDetectionAngle}");
                            Debug.DrawLine(aiCharacter.characterCombatManager.lockOnTransform.position, targetCharacter.characterCombatManager.lockOnTransform.position);
                            Debug.Log("BLOCKED");
                        }
                        else
                        {
                            aiCharacter.characterCombatManager.SetTarget(targetCharacter);
                        }
                    }
                }
            }
        }
    }    
}
