using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    [Header("이동 설정")]
    public float moveSpeed = 5.0f;

    [Header("점프 설정")]
    public float jumpForce = 10.0f;

    private Animator animator;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    public float jumpCooldown = 0.2f;
    private bool isGrounded = false;  // 바닥에 닿아있는지 여부
    private int score = 0;
    // 리스폰용 시작 위치 - 새로 추가!
    private Vector3 startPosition;
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        // 게임 시작 시 위치를 저장 - 새로 추가!
        startPosition = transform.position;
        Debug.Log("시작 위치 저장: " + startPosition);

        if (rb == null)
        {
            Debug.LogError("Rigidbody2D가 없습니다!");
        }
    }

    void Update()
    {
        // 좌우 이동 입력
        float moveX = 0f;
        if (Input.GetKey(KeyCode.A)) moveX = -1f;
        if (Input.GetKey(KeyCode.D)) moveX = 1f;

        // 물리 기반 이동
        rb.linearVelocity = new Vector2(moveX * moveSpeed, rb.linearVelocity.y);

        // 방향 전환: 이동 입력이 있을 때만 방향을 바꿉니다.
        if (moveX != 0)
        {
            if (moveX > 0)
            {
                transform.localScale = new Vector3(1, 1, 1); // 오른쪽을 보도록 원래대로
            }
            else
            {
                transform.localScale = new Vector3(-1, 1, 1); // 왼쪽을 보도록 반전
            }
        }

        // 점프 처리
        HandleJump();

        // 애니메이션
        float currentSpeed = Mathf.Abs(rb.linearVelocity.x);
        animator.SetFloat("Speed", currentSpeed);
    }

    /// <summary>
    /// 캐릭터의 점프 로직을 처리합니다.
    /// </summary>
    private void HandleJump()
    {
        // Space 키를 눌렀고, 캐릭터가 바닥에 있을 때 점프 실행
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            Debug.Log("점프!");
        }
    }
    // 바닥 충돌 감지 (Collision)
    void OnCollisionEnter2D(Collision2D collision)
    {
        // 충돌한 오브젝트가 "Ground" Tag를 가지고 있는지 확인
        if (collision.gameObject.CompareTag("Ground"))
        {
            Debug.Log("바닥에 착지!");
            isGrounded = true;
        }
        // 장애물 충돌 감지 - 새로 추가!
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            Debug.Log("⚠️ 장애물 충돌! 시작 지점으로 돌아갑니다.");

            // 시작 위치로 순간이동
            transform.position = startPosition;

            // 속도 초기화 (안 하면 계속 날아감)
            rb.linearVelocity = new Vector2(0, 0);
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            Debug.Log("바닥에서 떨어짐");
            isGrounded = false;
        }
    }
    // 아이템 수집 감지 (Trigger)
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Coin"))
        {
            score++;  // 점수 증가
            Debug.Log("코인 획득! 현재 점수: " + score);
            Destroy(other.gameObject);  // 코인 제거
        }
            // 골 도달 - 새로 추가!
        if (other.CompareTag("Goal"))
        {
            Debug.Log("🎉🎉🎉 게임 클리어! 🎉🎉🎉");
            Debug.Log("최종 점수: " + score + "점");
            
            // 캐릭터 조작 비활성화
            enabled = false;
        }
    }
}