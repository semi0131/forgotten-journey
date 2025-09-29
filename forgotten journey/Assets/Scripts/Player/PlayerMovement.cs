using UnityEditor.Build.Content;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("이동 설정")]
    [SerializeField] private float moveSpeed = 5f;

    // ⭐ 1. SPUM_Prefabs 스크립트 참조 변수 추가
    private SPUM_Prefabs spumPrefabs;

    // 필수: Rigidbody2D 컴포넌트 참조
    private Rigidbody2D rb;

    private Vector2 movement;
    private float originalScaleValue;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        // ⭐ 2. SPUM_Prefabs 컴포넌트를 가져와 할당
        spumPrefabs = GetComponent<SPUM_Prefabs>();

        if (rb == null)
        {
            Debug.LogError("PlayerMovement 스크립트에는 Rigidbody2D 컴포넌트가 필요합니다!");
        }
        // ⭐ 3. SPUM_Prefabs 참조 실패 시 에러 메시지 출력
        if (spumPrefabs == null)
        {
            Debug.LogError("PlayerMovement 스크립트에는 SPUM_Prefabs 컴포넌트가 필요합니다! 같은 오브젝트에 붙어있는지 확인하세요.");
        }

        // 스케일 값 초기화 (이전 로직 유지)
        originalScaleValue = transform.localScale.y;
    }

    void Update()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");

        movement.x = horizontalInput;
        movement.y = 0f;

        // ===============================================
        // 🌟 방향 전환 로직 추가 🌟
        // ===============================================
        if (horizontalInput > 0) // 오른쪽 이동 (원래 스케일)
        {
            transform.localScale = new Vector3(originalScaleValue, transform.localScale.y, transform.localScale.z);
        }
        else if (horizontalInput < 0) // 왼쪽 이동 (X 스케일 반전)
        {
            transform.localScale = new Vector3(-originalScaleValue, transform.localScale.y, transform.localScale.z);
        }
        // ===============================================

        // ⭐ 4. 애니메이션 상태 변경 로직 추가
        if (spumPrefabs != null)
        {
            if (horizontalInput != 0) // 움직임이 있을 때
            {
                // MOVE 애니메이션 재생 (인덱스 0 사용)
                spumPrefabs.PlayAnimation(PlayerState.MOVE, 0);
            }
            else // 멈춰있을 때
            {
                // IDLE 애니메이션 재생 (인덱스 0 사용)
                spumPrefabs.PlayAnimation(PlayerState.IDLE, 0);
            }
        }
    }

    void FixedUpdate()
    {
        // Rigidbody 이동 처리
        if (rb != null)
        {
            // 이미 velocity를 사용하고 있으므로 이대로 유지합니다.
            rb.linearVelocity = new Vector2(movement.x * moveSpeed, rb.linearVelocity.y);
        }
    }
}
