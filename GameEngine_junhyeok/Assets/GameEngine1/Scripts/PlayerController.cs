using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("캐릭터 설정")]
    public string playerName = "준혁백";
    public float moveSpeed = 5.0f;
    
    // Animator 컴포넌트 참조 (private - Inspector에 안 보임)
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    void Start()
    {
        // 게임 시작 시 한 번만 - Animator 컴포넌트 찾아서 저장
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        Debug.Log("안녕하세요, " + playerName + "님!");
        Debug.Log("이동 속도: " + moveSpeed);
        
        // 디버그: 제대로 찾았는지 확인
        if (animator != null)
        {
            Debug.Log("Animator 컴포넌트를 찾았습니다!");
        }
        else
        {
            Debug.LogError("Animator 컴포넌트가 없습니다!");
        }
    }
    
    void Update()
    {
        // 이동 벡터 계산
        Vector3 movement = Vector3.zero;
        if (Input.GetKey(KeyCode.W))
        {
            movement += Vector3.up;
        }

        if (Input.GetKey(KeyCode.A))
        {
            movement += Vector3.left;
            spriteRenderer.flipX = true; // X축 뒤집기
        }

        if (Input.GetKey(KeyCode.S))
        {
            movement += Vector3.down;
        }

        if (Input.GetKey(KeyCode.D))
        {
            movement += Vector3.right;
            spriteRenderer.flipX = false; // 원래 크기
        }

        // 실제 이동 적용
        float currentMoveSpeed = moveSpeed;
        if (Input.GetKey(KeyCode.LeftShift))
        {
            currentMoveSpeed = 2f * moveSpeed;
            Debug.Log("달리기 모드 활성화!");
            transform.Translate(movement * currentMoveSpeed * Time.deltaTime);
        }

        if (movement != Vector3.zero)
        {
            transform.Translate(movement * currentMoveSpeed * Time.deltaTime);
        }
        
        // 속도 계산: 이동 중이면 moveSpeed, 아니면 0
        float currentSpeed = movement != Vector3.zero ? currentMoveSpeed : 0f;

        // Animator에 속도 전달
        if (animator != null)
        {
            animator.SetFloat("Speed", currentSpeed);
            Debug.Log("Current Speed: " + currentSpeed);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (animator != null)
            {
                animator.SetBool("isJumping", true);
                Debug.Log("점프!");
            }
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            if (animator != null)
            {
                animator.SetBool("isJumping", false);
            }
        }
    }
}
