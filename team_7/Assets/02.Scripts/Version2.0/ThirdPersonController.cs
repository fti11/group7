 using UnityEngine;
#if ENABLE_INPUT_SYSTEM 
using UnityEngine.InputSystem;
#endif

/* Note: animations are called via the controller for both the character and capsule using animator null checks
 */

namespace StarterAssets
{
    [RequireComponent(typeof(CharacterController))]
#if ENABLE_INPUT_SYSTEM 
    [RequireComponent(typeof(PlayerInput))]
#endif
    public class ThirdPersonController : MonoBehaviour
    {
        [Header("플레이어")]
        [Tooltip("캐릭터의 이동 속도 (m/s)")]
        public float moveSpeed = 2.0f;

        [Tooltip("달리기 속도 (m/s)")]
        public float sprintSpeed = 5.335f;

        [Tooltip("캐릭터가 이동 방향을 향해 회전하는 속도")]
        [Range(0.0f, 0.3f)]
        public float rotationSmoothTime = 0.12f;

        [Tooltip("가속도와 감속도")]
        public float speedChangeRate = 10.0f;

        public float Sensitivity = 1.0f;

        public AudioClip LandingAudioClip;
        public AudioClip[] FootstepAudioClips;
        [Range(0, 1)] public float FootstepAudioVolume = 0.5f;

        [Space(10)]
        [Tooltip("플레이어가 점프할 수 있는 높이")]
        public float JumpHeight = 1.2f;

        [Tooltip("캐릭터가 사용하는 중력 값. 엔진의 기본 값은 -9.81f")]
        public float Gravity = -15.0f;

        [Space(10)]
        [Tooltip("점프 후 다시 점프할 수 있는 시간. 0f로 설정하면 즉시 다시 점프 가능")]
        public float JumpTimeout = 0.50f;

        [Tooltip("하강 상태로 들어가기 전에 경과해야 할 시간. 계단을 내려갈 때 유용")]
        public float FallTimeout = 0.15f;

        [Header("플레이어 땅에 닿음")]
        [Tooltip("캐릭터가 땅에 닿았는지 여부. CharacterController에 내장된 grounded 확인과는 다름")]
        public bool Grounded = true;

        [Tooltip("울퉁불퉁한 지형에 유용")]
        public float GroundedOffset = -0.14f;

        [Tooltip("땅에 닿는 검사의 반지름. CharacterController의 반지름과 일치해야 함")]
        public float GroundedRadius = 0.28f;

        [Tooltip("캐릭터가 땅으로 사용하는 레이어")]
        public LayerMask GroundLayers;

        [Header("Cinemachine")]
        [Tooltip("카메라가 따라갈 Cinemachine 가상 카메라에서 설정한 추적 대상")]
        public GameObject CinemachineCameraTarget;

        [Tooltip("카메라를 위로 움직일 수 있는 최대 각도 (도 단위)")]
        public float TopClamp = 70.0f;

        [Tooltip("카메라를 아래로 움직일 수 있는 최대 각도 (도 단위)")]
        public float BottomClamp = -30.0f;

        [Tooltip("잠긴 상태에서 카메라 위치를 미세 조정할 때 유용한 추가 각도")]
        public float CameraAngleOverride = 0.0f;

        [Tooltip("모든 축에서 카메라 위치를 잠그기 위해")]
        public bool LockCameraPosition = false;


        // cinemachine
        private float _cinemachineTargetYaw;
        private float _cinemachineTargetPitch;

        // player
        private float _speed;
        private float _animationBlend;
        private float _targetRotation = 0.0f;
        private float _rotationVelocity;
        private float _verticalVelocity;
        private float _terminalVelocity = 53.0f;

        // timeout deltatime
        private float _jumpTimeoutDelta;
        private float _fallTimeoutDelta;

        // animation IDs
        private int _animIDSpeed;
        private int _animIDGrounded;
        private int _animIDJump;
        private int _animIDFreeFall;
        private int _animIDMotionSpeed;

#if ENABLE_INPUT_SYSTEM 
        private PlayerInput _playerInput;
#endif
        private Animator _animator;
        private CharacterController _controller;
        private StarterAssetsInputs _input;
        private GameObject _mainCamera;
        private bool _rotateOnMove = true;

        private const float _threshold = 0.01f;

        private bool _hasAnimator;

        private bool IsCurrentDeviceMouse
        {
            get
            {
#if ENABLE_INPUT_SYSTEM
                return _playerInput.currentControlScheme == "KeyboardMouse";
#else
				return false;
#endif
            }
        }


        private void Awake()
        {
            // 메인 카메라에 대한 참조 가져오기
            if (_mainCamera == null)
            {
                _mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
            }
        }

        private void Start()
        {
            _cinemachineTargetYaw = CinemachineCameraTarget.transform.rotation.eulerAngles.y;

            _hasAnimator = TryGetComponent(out _animator);
            _controller = GetComponent<CharacterController>();
            _input = GetComponent<StarterAssetsInputs>();
#if ENABLE_INPUT_SYSTEM 
            _playerInput = GetComponent<PlayerInput>();
#else
			Debug.LogError( "Starter Assets 패키지에 필요한 종속성이 없습니다. Tools/Starter Assets/Reinstall Dependencies를 사용하여 수정하십시오");
#endif

            AssignAnimationIDs();

            // reset our timeouts on start
            _jumpTimeoutDelta = JumpTimeout;
            _fallTimeoutDelta = FallTimeout;
        }

        private void Update()
        {
            _hasAnimator = TryGetComponent(out _animator);

            JumpAndGravity();
            GroundedCheck();
            Move();
        }

        private void LateUpdate()
        {
            CameraRotation();
        }

        private void AssignAnimationIDs()
        {
            _animIDSpeed = Animator.StringToHash("Speed");
            _animIDGrounded = Animator.StringToHash("Grounded");
            _animIDJump = Animator.StringToHash("Jump");
            _animIDFreeFall = Animator.StringToHash("FreeFall");
            _animIDMotionSpeed = Animator.StringToHash("MotionSpeed");
        }

        private void GroundedCheck()
        {
            // 구체의 위치 설정 (오프셋 적용)
            Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset,
                transform.position.z);
            Grounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers,
                QueryTriggerInteraction.Ignore);

            // 캐릭터를 사용하는 경우 애니메이터 업데이트
            if (_hasAnimator)
            {
                _animator.SetBool(_animIDGrounded, Grounded);
            }
        }

        private void CameraRotation()
        {
            // 입력이 있고 카메라 위치가 고정되지 않은 경우
            if (_input.look.sqrMagnitude >= _threshold && !LockCameraPosition)
            {
                // 마우스 입력에 Time.deltaTime을 곱하지 않음
                float deltaTimeMultiplier = IsCurrentDeviceMouse ? 1.0f : Time.deltaTime;

                _cinemachineTargetYaw += _input.look.x * deltaTimeMultiplier * Sensitivity;
                _cinemachineTargetPitch += _input.look.y * deltaTimeMultiplier * Sensitivity;
            }

            // 회전 값을 360도로 제한
            _cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
            _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

            // Cinemachine이 이 타겟을 따라갑니다
            CinemachineCameraTarget.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch + CameraAngleOverride,
                _cinemachineTargetYaw, 0.0f);
        }

        private void Move()
        {
            // 이동 속도, 달리기 속도 및 달리기 버튼을 기반으로 목표 속도 설정
            float targetSpeed = _input.sprint ? sprintSpeed : moveSpeed;

            // 간단한 가속도 및 감속도. 제거, 교체 또는 반복하기 쉬운 형태로 설계되었습니다.

            // 참고: Vector2의 == 연산자는 근사치를 사용하므로 부동 소수점 오차가 없으며, 크기보다 저렴합니다.
            // 입력이 없는 경우 목표 속도를 0으로 설정합니다.
            if (_input.move == Vector2.zero) targetSpeed = 0.0f;

            // 플레이어의 현재 수평 속도에 대한 참조
            float currentHorizontalSpeed = new Vector3(_controller.velocity.x, 0.0f, _controller.velocity.z).magnitude;

            float speedOffset = 0.1f;
            float inputMagnitude = _input.analogMovement ? _input.move.magnitude : 1f;

            // 목표 속도로 가속 또는 감속
            if (currentHorizontalSpeed < targetSpeed - speedOffset ||
                currentHorizontalSpeed > targetSpeed + speedOffset)
            {
                // 결과를 곡선 형태로 만들어 선형적인 속도 변경보다 유기적인 속도 변경을 제공합니다.
                // Lerp의 T는 클램프되므로 속도를 클램프 할 필요가 없습니다.
                _speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude,
                    Time.deltaTime * speedChangeRate);

                // 속도를 세 번째 소수 자리까지 반올림
                _speed = Mathf.Round(_speed * 1000f) / 1000f;
            }
            else
            {
                _speed = targetSpeed;
            }

            _animationBlend = Mathf.Lerp(_animationBlend, targetSpeed, Time.deltaTime * speedChangeRate);
            if (_animationBlend < 0.01f) _animationBlend = 0f;

            // 입력 방향을 정규화
            Vector3 inputDirection = new Vector3(_input.move.x, 0.0f, _input.move.y).normalized;

            // 참고: Vector2의 != 연산자는 근사치를 사용하므로 부동 소수점 오차가 없으며, 크기보다 저렴합니다.
            // 이동 입력이 있는 경우 플레이어를 회전시킵니다.
            if (_input.move != Vector2.zero)
            {
                _targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg + _mainCamera.transform.eulerAngles.y;
                float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity, rotationSmoothTime);

                if(_rotateOnMove)
                {
                    transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
                }
                
            }


            Vector3 targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;

            // 플레이어 이동
            _controller.Move(targetDirection.normalized * (_speed * Time.deltaTime) +
                             new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);

            // 캐릭터를 사용하는 경우 애니메이터 업데이트
            if (_hasAnimator)
            {
                _animator.SetFloat(_animIDSpeed, _animationBlend);
                _animator.SetFloat(_animIDMotionSpeed, inputMagnitude);
            }
        }


        private void JumpAndGravity()
        {
            if (Grounded)
            {
                // 하강 타임아웃 타이머 재설정
                _fallTimeoutDelta = FallTimeout;

                // 캐릭터를 사용하는 경우 애니메이터 업데이트
                if (_hasAnimator)
                {
                    _animator.SetBool(_animIDJump, false);
                    _animator.SetBool(_animIDFreeFall, false);
                }

                // 땅에 닿을 때 무한하게 속도가 떨어지지 않도록 중지
                if (_verticalVelocity < 0.0f)
                {
                    _verticalVelocity = -2f;
                }

                // 점프
                if (_input.jump && _jumpTimeoutDelta <= 0.0f)
                {
                    // H * -2 * G의 제곱근 = 목표 높이에 도달하기 위해 필요한 속도
                    _verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity);

                    // 캐릭터를 사용하는 경우 애니메이터 업데이트
                    if (_hasAnimator)
                    {
                        _animator.SetBool(_animIDJump, true);
                    }
                }

                // 점프 타임아웃
                if (_jumpTimeoutDelta >= 0.0f)
                {
                    _jumpTimeoutDelta -= Time.deltaTime;
                }
            }
            else
            {
                // 점프 타임아웃 타이머 재설정
                _jumpTimeoutDelta = JumpTimeout;

                // 하강 타임아웃
                if (_fallTimeoutDelta >= 0.0f)
                {
                    _fallTimeoutDelta -= Time.deltaTime;
                }
                else
                {
                    // 캐릭터를 사용하는 경우 애니메이터 업데이트
                    if (_hasAnimator)
                    {
                        _animator.SetBool(_animIDFreeFall, true);
                    }
                }

                // 땅에 닿지 않은 경우 점프하지 않음
                _input.jump = false;
            }

            // 시간에 따라 중력을 적용합니다 (중력을 두 번 곱해서 시간이 지남에 따라 선형적으로 가속)
            if (_verticalVelocity < _terminalVelocity)
            {
                _verticalVelocity += Gravity * Time.deltaTime;
            }
        }

        private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
        {
            if (lfAngle < -360f) lfAngle += 360f;
            if (lfAngle > 360f) lfAngle -= 360f;
            return Mathf.Clamp(lfAngle, lfMin, lfMax);
        }

        private void OnDrawGizmosSelected()
        {
            Color transparentGreen = new Color(0.0f, 1.0f, 0.0f, 0.35f);
            Color transparentRed = new Color(1.0f, 0.0f, 0.0f, 0.35f);

            if (Grounded) Gizmos.color = transparentGreen;
            else Gizmos.color = transparentRed;

            // 선택된 경우, 땅 충돌체의 위치에 gizmo 그리기
            Gizmos.DrawSphere(
                new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z),
                GroundedRadius);
        }

        public void SetSensitivity(float aimSensitivity)
        {
            Sensitivity = aimSensitivity;
        }

        public void SetRotateOnMove(bool newRotateOnMove)
        {
            _rotateOnMove = newRotateOnMove;
        }



        private void OnFootstep(AnimationEvent animationEvent)
        {
            if (animationEvent.animatorClipInfo.weight > 0.5f)
            {
                if (FootstepAudioClips.Length > 0)
                {
                    var index = Random.Range(0, FootstepAudioClips.Length);
                    AudioSource.PlayClipAtPoint(FootstepAudioClips[index], transform.TransformPoint(_controller.center), FootstepAudioVolume);
                }
            }
        }

        private void OnLand(AnimationEvent animationEvent)
        {
            if (animationEvent.animatorClipInfo.weight > 0.5f)
            {
                //AudioSource.PlayClipAtPoint(LandingAudioClip, transform.TransformPoint(_controller.center), FootstepAudioVolume);
            }
        }
    }
}