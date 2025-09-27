using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("이동 설정")]
    // 캐릭터의 이동 속도를 설정합니다. (Inspector에서 조절 가능)
    [SerializeField] private float moveSpeed = 5f;

    // ⭐ 필수: Rigidbody2D 컴포넌트 참조
    private Rigidbody2D rb;

    // 이동 벡터를 저장할 변수
    private Vector2 movement;

    // ⭐ 추가: 캐릭터의 원래 Y축 스케일 값을 저장할 변수 (원래 X, Y 스케일 모두 저장)
    private float originalScaleValue;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        if (rb == null)
        {
            Debug.LogError("PlayerMovement 스크립트에는 Rigidbody2D 컴포넌트가 필요합니다!");
        }

        // ⭐ Awake에서 현재 설정된 X, Y 스케일 값(10)을 저장합니다.
        // X, Y 스케일이 동일하다는 전제 하에 Y 값을 사용합니다.
        originalScaleValue = transform.localScale.y;
    }

    // Update는 입력 감지에 사용됩니다. (프레임 단위)
    void Update()
    {
        // 1. A, D 키 입력 감지
        float horizontalInput = Input.GetAxisRaw("Horizontal");

        movement.x = horizontalInput;
        movement.y = 0f;

        // ⭐ 2. 캐릭터 방향 뒤집기 (논리 반전 적용)

        // 오른쪽으로 이동 중이면 (D키, horizontalInput > 0)
        if (horizontalInput > 0)
        {
            // ⭐ 논리 반전: 오른쪽으로 이동 시 X축을 음수(-10)로 설정하여 왼쪽을 바라보도록 합니다.
            transform.localScale = new Vector3(-originalScaleValue, originalScaleValue, 1f);
        }
        // 왼쪽으로 이동 중이면 (A키, horizontalInput < 0)
        else if (horizontalInput < 0)
        {
            // ⭐ 논리 반전: 왼쪽으로 이동 시 X축을 양수(10)로 설정하여 오른쪽을 바라보도록 합니다.
            transform.localScale = new Vector3(originalScaleValue, originalScaleValue, 1f);
        }
    }

    // FixedUpdate는 물리 계산에 사용됩니다. (고정된 시간 간격)
    void FixedUpdate()
    {
        // 3. Rigidbody를 이용한 이동 처리
        if (rb != null)
        {
            rb.linearVelocity = new Vector2(movement.x * moveSpeed, rb.linearVelocity.y);
        }
    }
}
