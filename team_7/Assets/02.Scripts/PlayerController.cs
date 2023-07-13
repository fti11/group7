using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public enum PlayerType
    {
        Player001,
        Player002
    }

    public float moveSpeed = 5f;  // 플레이어 이동 속도
    public float rotationSpeed = 200f;  // 플레이어 회전 속도
    public Transform cameraTransform;  // 카메라 Transform 컴포넌트

    public int maxHp;
    public int nowHp;
    public Image nowHpbar;

    private Rigidbody rb;  // Rigidbody 컴포넌트 참조

    public AnimationActions animationActions;

    public bool RunFlag = false;
    public GameObject ProjectTile;
    public Transform FilePoint;
    public float movementMagnitude = 0.0f;

    public PlayerType playertype = PlayerType.Player001;
    string GamePadType = "GamePad1_A";
    string GamePadVertical = "Vertical1";
    string GamePadVHorizontal = "Horizontal1";

    private void Start()
    {
        rb = GetComponent<Rigidbody>();  // Rigidbody 컴포넌트 할당

        nowHpbar = GameObject.FindWithTag("HP_BAR").GetComponent<Image>();
        //rb.MoveRotation(Quaternion.LookRotation(cameraTransform.forward));
        

        if (playertype == PlayerType.Player002)
        {
            GamePadType = "GamePad2_A";
            GamePadVertical = "Vertical2";
            GamePadVHorizontal = "Horizontal2";
        }

        // 마우스를 화면 가운데에 고정하고 숨김
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        maxHp = 50;
        nowHp = 50;

        this.gameObject.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
    }

    private void Update()
    {

        if (Input.GetButtonDown("Fire1")) // 마우스 왼쪽 버튼을 눌렀을 때
        {
            if (animationActions.animationController.charState == AnimationsState.Idle)
            {
                animationActions.TakeAction("Attack");
            }
            else
            {
                animationActions.TakeAction("RunAttack");
            }

            GameObject temp = Instantiate(ProjectTile);
            temp.transform.position = FilePoint.transform.position;
            temp.transform.forward = FilePoint.transform.forward;
        }

        nowHpbar.fillAmount = (float)nowHp / (float)maxHp;
    }

    private void FixedUpdate()
    {

        float verticalInput = Input.GetAxis("Vertical"); // 앞뒤 이동 입력
        float horizontalInput = Input.GetAxis("Horizontal"); // 좌우 이동 입력

        Vector3 movement = cameraTransform.forward * verticalInput * moveSpeed * Time.fixedDeltaTime +
            cameraTransform.right * horizontalInput * moveSpeed * Time.fixedDeltaTime;
        Vector3 newPosition = rb.position + movement;

        rb.MovePosition(newPosition);

        float mouseX = Input.GetAxis("Mouse X") * rotationSpeed * Time.fixedDeltaTime;
        //float mouseY = Input.GetAxis("Mouse Y") * rotationSpeed * Time.fixedDeltaTime; // 마우스의 Y 축 움직임

        rb.MoveRotation(rb.rotation * Quaternion.Euler(Vector3.up * mouseX));
        //cameraTransform.rotation *= Quaternion.Euler(-mouseY, 0f, 0f); // 카메라의 회전에 마우스의 Y 축 움직임을 추가

        movementMagnitude = movement.magnitude;
        if (movementMagnitude > 0)
        {
            animationActions.TakeAction("Run");
        }
        else
        {
            animationActions.TakeAction("Idle");
        }
    }
}