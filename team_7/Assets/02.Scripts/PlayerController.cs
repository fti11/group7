using UnityEngine;

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
        
        if (playertype == PlayerType.Player002)
        {
            GamePadType = "GamePad2_A";
            GamePadVertical = "Vertical2";
            GamePadVHorizontal = "Horizontal2";
        }
    }

    private void Update()
    {

        if(Input.GetButtonDown(GamePadType) || (Input.GetKeyDown(KeyCode.Space) && playertype == PlayerType.Player001))
        {
            if(animationActions.animationController.charState == AnimationsState.Idle)
            {
               
                animationActions.TakeAction("Attack");
            }
            else
            {
                animationActions.TakeAction("RunAttack");
            }

            GameObject temp = (GameObject)Instantiate(ProjectTile);
            temp.transform.position = FilePoint.transform.position;
            temp.transform.forward = FilePoint.transform.forward;

        }
    }

    private void FixedUpdate()
    {
       
        float verticalInput = Input.GetAxis(GamePadVertical);  // 앞뒤 이동 입력

        if (playertype == PlayerType.Player001)
        {
            if (Input.GetKey(KeyCode.UpArrow)) verticalInput = 1.0f;
            if (Input.GetKey(KeyCode.DownArrow)) verticalInput = -1.0f;
        }
       


        Vector3 movement = cameraTransform.forward * verticalInput * moveSpeed * Time.fixedDeltaTime;
        Vector3 newPosition = rb.position + movement;

        rb.MovePosition(newPosition);        

        float mouseX = Input.GetAxis(GamePadVHorizontal) * rotationSpeed * Time.fixedDeltaTime;

        if (playertype == PlayerType.Player001)
        {
            if (Input.GetKey(KeyCode.LeftArrow)) mouseX = -1.0f;
            if (Input.GetKey(KeyCode.RightArrow)) mouseX = 1.0f;
        }

        rb.MoveRotation(rb.rotation * Quaternion.Euler(Vector3.up * mouseX));
        cameraTransform.rotation = Quaternion.Euler(1, cameraTransform.rotation.eulerAngles.y, 0f);

        movementMagnitude = movement.magnitude;
        if (movementMagnitude > 0)
        {            
            animationActions.TakeAction("Run");                     
        }
        else
        {
            
            animationActions.TakeAction("Idle");


            if (mouseX > 0)
            {
                animationActions.TakeAction("TurnRight"); // 우측 회전 애니메이션을 재생하기 위한 트리거 설정
            }
            else if (mouseX < 0)
            {
                animationActions.TakeAction("TurnLeft"); // 좌측 회전 애니메이션을 재생하기 위한 트리거 설정
            }

        }

       

    }
}