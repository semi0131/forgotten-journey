using System.Collections;
using UnityEngine;
using static SPUM_Prefabs; // PlayerState Enum을 사용하기 위해 필요

// UnityEditor.Build.Content; 는 빌드 오류를 유발할 수 있으므로 제거했습니다.

public class PlayerAction : MonoBehaviour
{
    // -------------------------------------------------------------------------
    // 컴포넌트 참조 (Inspector에서 연결되어야 합니다)
    // -------------------------------------------------------------------------

    [SerializeField] private SPUM_Prefabs spumPrefabs;
    [SerializeField] private TurnManager turnManager;

    // -------------------------------------------------------------------------
    // 유니티 라이프사이클
    // -------------------------------------------------------------------------

    void Start()
    {
        // Inspector 연결 확인 및 미연결 시 FindObjectOfType으로 대체 시도
        if (spumPrefabs == null)
        {
            spumPrefabs = GetComponent<SPUM_Prefabs>();
            if (spumPrefabs == null) Debug.LogError("SPUM_Prefabs 컴포넌트를 찾을 수 없습니다.");
        }

        if (turnManager == null)
        {
            turnManager = FindObjectOfType<TurnManager>();
            if (turnManager == null) Debug.LogError("TurnManager를 씬에서 찾을 수 없습니다.");
        }
    }

    // -------------------------------------------------------------------------
    // 턴 소비 행동 실행 (OnClick 이벤트에 연결)
    // -------------------------------------------------------------------------

    /// <summary>
    /// 플레이어의 모든 턴 소비 행동(공격, 스킬, 방어) 실행을 시작합니다.
    /// 이 함수가 호출되면 턴이 넘어갑니다.
    /// </summary>
    /// <param name="actionType">실행할 행동의 종류("ATTACK", "MAGIC_A", "DEFENSE" 등)</param>
    public void ExecutePlayerAction(string actionType)
    {
        if (turnManager == null || spumPrefabs == null) return;

        // 1. 턴 상태 체크: 플레이어 턴이 아닐 때는 즉시 함수를 종료합니다.
        // 턴매니저에서 BattleState를 클래스 밖으로 뺐다면 TurnManager. 없이 BattleState 사용 가능
        if (turnManager.state != BattleState.PlayerTurn)
        {
            Debug.Log("플레이어 턴이 아닙니다. 행동 취소.");
            return;
        }

        // 2. 턴 매니저에게 '행동 시작'을 알립니다. (턴 상태 Action으로 변경)
        turnManager.StartPlayerAction(actionType);

        // 3. actionType에 따라 다른 애니메이션을 재생합니다.
        ExecuteAnimation(actionType);

        // 4. 애니메이션 재생이 끝날 때까지 기다린 후 턴을 넘깁니다.
        // ⭐ 이 duration 값은 애니메이션 길이에 맞게 설정해야 합니다.
        float animationDuration = 1.0f;
        StartCoroutine(WaitForActionEnd(animationDuration));
    }

    /// <summary>
    /// 행동 타입에 따라 SPUM 애니메이션을 재생합니다.
    /// </summary>
    private void ExecuteAnimation(string actionType)
    {
        switch (actionType)
        {
            case "ATTACK":
                spumPrefabs.PlayAnimation(PlayerState.ATTACK, 0); // 기본 공격 모션
                break;
            case "DEFENSE":
                // 방어 행동: 애니메이션이 없으므로 대기만 합니다.
                Debug.Log("방어 행동: 애니메이션이 없으므로 대기만 합니다.");
                break;

            // -------------------------------------------------------------------
            // ⭐ [핵심 수정] 마법 스킬은 모션 재생을 완전히 막고 로그만 출력
            // -------------------------------------------------------------------
            case "MAGIC_A": // Button(0) 마법 스킬
            case "MAGIC_M": // Button(1) 마법 스킬
            case "MAGIC_B": // Button(2) 마법 스킬
                            // ⭐ 이 세 가지 마법 스킬에 대해서는 PlayAnimation 코드를 실행하지 않습니다.
                            // 대신 캐릭터의 IDLE 모션이 유지됩니다.
                Debug.Log($"마법 시전: {actionType} 모션 재생 없이 턴만 진행.");

                // 만약 캐릭터가 이전에 취했던 모션이 IDLE이 아니라면, 
                // IDLE 모션을 강제로 재생해주는 것이 좋습니다.
                spumPrefabs.PlayAnimation(PlayerState.IDLE, 0);
                break;

            default:
                Debug.LogWarning($"알 수 없는 행동 타입: {actionType}. 기본 공격 애니메이션으로 대체합니다.");
                spumPrefabs.PlayAnimation(PlayerState.ATTACK, 0);
                break;
        }
    }

    /// <summary>
    /// 애니메이션 재생 시간만큼 기다린 후 턴을 넘기는 코루틴입니다.
    /// </summary>
    private IEnumerator WaitForActionEnd(float duration)
    {
        // 애니메이션 재생 시간만큼 기다립니다.
        yield return new WaitForSeconds(duration);

        // 4. 애니메이션이 끝났으니 턴 매니저에게 알리고 턴을 종료합니다.
        if (turnManager != null)
        {
            turnManager.FinishPlayerAction();
        }
    }
}

// ⚠️ 참고: BattleState Enum은 TurnManager.cs 파일의 클래스 밖에 정의되어야 합니다.