using System;
using System.Net.NetworkInformation;
using UnityEngine;

namespace TSG
{
    

    public class PlayerCamera : MonoBehaviour
    {
        public static PlayerCamera instance;
        public PlayerManager player;
        public Camera cameraObject;
        [SerializeField] public Transform cameraPivotTransform;
        
        // 카메라 성능 관련 수치들
        [Header("카메라 세팅")]
        private float cameraSmoothSpeed = 1;    // 숫자가 클수록, 카메라가 움직임을 따라가는 시간이 더 걸린다
        [SerializeField] private float leftAndRightRotationSpeed = 220;
        [SerializeField] private float upAndDownRotationSpeed = 220;
        [SerializeField] private float minimumPivot = -30;  // 플레이어가 볼 수 있는 가장 낮은 위치
        [SerializeField] private float maximumPivot = 60;  // 플레이어가 볼 수 있는 가장 높은 위치
        [SerializeField] private float cameraCollisionRadius = 0.2f;
        [SerializeField] LayerMask collideWithLayers;

        [Header("카메라 값")]
        private Vector3 cameraVelocity;
        private Vector3 cameraObjectPosition;   // 카메라 충돌을 위한 값(카메라 오브젝트를 충돌 직전까지 올림)
        [SerializeField] float leftAndRightLookAngle;
        [SerializeField] float upAndDownLookAngle;
        private float cameraZPosition;    // 카메라 충돌을 위한 값
        private float targetCameraZPosition;     // 카메라 충돌을 위한 값

        [Header("락 온")]
        [SerializeField] float lockOnRadius = 20;
        [SerializeField] float minimumViewableAngle = -50;
        [SerializeField] float maximumViewableAngle = 50;
        [SerializeField] float maximumLockOnDistance = 20;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
            cameraZPosition = cameraObject.transform.localPosition.z;
        }

        public void HandleAllCameraActions()
        {

            if(player != null)
            {
                HandleFollowTarget();  
                HandleRotation();
                HandleCollisions();              
                // 플레이어를 향해 따라다니고
                // 플레이어 주변을 돌아다니며
                // 물체에 충돌하기도 해야함
            }
            

        }

        private void HandleFollowTarget()
        {
            Vector3 targetCameraPosition = Vector3.SmoothDamp(transform.position, player.transform.position, ref cameraVelocity, cameraSmoothSpeed * Time.deltaTime);
            transform.position = targetCameraPosition;
        }
    
        private void HandleRotation()
        {
            // 만약 특정 대상을 락온 한다면, 그 특정 대상 주변으로 회전
            // 그 외의 상황에선 일상적으로 회전할것

            // 일반적인 회전
            // 왼쪽과 오른쪽 회전은 기본적으로 오른쪽 조이스틱의 수평 움직임에 따라 달렸다
            leftAndRightLookAngle += (PlayerInputManager.instance.cameraHorizontalInput * leftAndRightRotationSpeed) * Time.deltaTime;
            // 위아래 회전은 오른쪽 조이스틱의 직선 움직임에 따라 달렸다
            upAndDownLookAngle -= (PlayerInputManager.instance.cameraVerticalInput * upAndDownRotationSpeed) * Time.deltaTime;
            // 위아래의 값의 최대치를 맞춰준다
            upAndDownLookAngle = Mathf.Clamp(upAndDownLookAngle, minimumPivot, maximumPivot);

            Vector3 cameraRotation = Vector3.zero;
            Quaternion targetRotation;

            // 지정된 게임 오브젝트를 좌측/우측으로 회전
            cameraRotation.y = leftAndRightLookAngle;
            targetRotation = Quaternion.Euler(cameraRotation);
            transform.rotation = targetRotation;
            
            // 게임 오브젝트의 위아래 시점을 회전
            cameraRotation = Vector3.zero;
            cameraRotation.x = upAndDownLookAngle;
            targetRotation =Quaternion.Euler(cameraRotation);
            cameraPivotTransform.localRotation = targetRotation;
        }
    
        private void HandleCollisions()
        {
            targetCameraZPosition = cameraZPosition;

            RaycastHit hit;
            // (카메라랑) 충돌하는 방향 체크
            Vector3 direction = cameraObject.transform.position - cameraPivotTransform.position;
            direction.Normalize();

            // 카메라가 움직이는 방향에 물체가 있는지 체크
            if(Physics.SphereCast(cameraPivotTransform.position, cameraCollisionRadius, direction, out hit, Mathf.Abs(targetCameraZPosition), collideWithLayers))
            {
                // 물체가 있다면, 그 물체와의 거리 계산
                float distanceFromHitObject = Vector3.Distance(cameraPivotTransform.position, hit.point);
                // 계산된 거리에 맞춰서 카메라의 Z위치 따라감
                targetCameraZPosition = -(distanceFromHitObject - cameraCollisionRadius);
            }

            // 카메라가 너무 가까워지는 것을 방지 (벽을 뚫고 들어가는 것 방지)
            if(Mathf.Abs(targetCameraZPosition) < cameraCollisionRadius)
            {
                targetCameraZPosition = -cameraCollisionRadius;
            }

            // z축 위치를 부드럽게 이동 (Lerp)
            cameraObjectPosition.z = Mathf.Lerp(cameraObject.transform.localPosition.z, targetCameraZPosition, 0.2f);
            cameraObject.transform.localPosition = cameraObjectPosition;
        }
    
        public void HandleLocatingLockOnTargets()
        {
            float shortestDistance = Mathf.Infinity;   // 플레이어와 가까운 타겟 고르는 데 쓸 것
            float shortestDistanceOfRightTarget = Mathf.Infinity;  //오른쪽 축에서 가장 가까운 타겟을 찾기 위한 것(현재 타겟에서 가장 가까운 오른쪽 타겟)
            float shortestDistanceOfLeftTarget = -Mathf.Infinity;   //왼쪽 축에서 가장 가까운 타겟을 찾기 위한 것(현재 타겟에서 가장 가까운 왼쪽 타겟)

            // 해야할 거: Layermask를 쓸것
            Collider[] colliders = Physics.OverlapSphere(player.transform.position, lockOnRadius, WorldUtilityManager.Instance.GetCharacterLayers());

            for(int i = 0; i < colliders.Length; i++)
            {
                CharacterManager lockOnTarget = colliders[i].GetComponent<CharacterManager>();

                if(lockOnTarget != null)
                {
                    // 현재 시야 필드에 있나 없나 확인
                    Vector3 lockOnTargetsDirection = lockOnTarget.transform.position - player.transform.position;
                    float distanceFromTarget = Vector3.Distance(player.transform.position, lockOnTarget.transform.position);
                    float viewableAngle = Vector3.Angle(lockOnTargetsDirection, cameraObject.transform.forward);

                    // 타겟이 죽었으면, 다음 타겟을 바로 추적
                    if (lockOnTarget.isDead.Value)
                    {
                        continue;
                    }

                    // 타겟이 우리라면, 바로 다음 타겟을 찾기
                    if(lockOnTarget.transform.root == player.transform.root)
                    {
                        continue;
                    }

                    // 타겟이 너무 멀면, 다음 잠재적 타겟을 찾을것
                    if(distanceFromTarget > maximumLockOnDistance)
                    {
                        continue;
                    }

                    if(viewableAngle > minimumViewableAngle && viewableAngle < maximumViewableAngle)
                    {
                        RaycastHit hit;

                        // 해야할 거: Enviro Layer를 위한 Layer Mask 추가하기
                        if(Physics.Linecast(player.playerCombatManager.lockOnTransform.position,
                         lockOnTarget.characterCombatManager.lockOnTransform.position,
                          out hit, WorldUtilityManager.Instance.GetEnviroLayers()))
                        {
                            // 우리가 뭔가를 공격했다면, 우리가 락온 한 대상을 볼 수 없게
                            continue;
                        }
                        else
                        {
                            Debug.Log("락 온 성공!");
                        }
                    }
                }
            }
        }
    }
}
