using System.Collections;
using UnityEditor.Build.Content;
using UnityEngine;
using static TurnManager;

public class PlayerAction : MonoBehaviour
{
    public TalkManager manager;
    GameObject scanObject;

    [SerializeField] private SPUM_Prefabs spumPrefabs;
    [SerializeField] private TurnManager turnManager; // 턴 관리 스크립트 (선택 사항)

    void Start()
    {
        // 만약 Inspector에서 연결하지 않았다면, 여기서 찾아서 연결합니다.
        if (spumPrefabs == null)
        {
            spumPrefabs = GetComponent<SPUM_Prefabs>();
        }
        if (turnManager == null)
        {
            turnManager = FindObjectOfType<TurnManager>();
        }
    }

    /// <summary>
    /// 공격 버튼의 OnClick 이벤트에 연결될 함수. 
    /// 애니메이션을 실행하고 턴 매니저에게 행동 완료를 알립니다.
    /// </summary>
    public void ExecuteAttackAction()
    {
        if (turnManager == null || spumPrefabs == null) return;

        // =========================================================
        // ⭐ [핵심] 턴 상태 체크: 플레이어 턴이 아닐 때는 즉시 함수를 종료합니다.
        // =========================================================
        if (turnManager.state != BattleState.PlayerTurn)
        {
            Debug.Log("플레이어 턴이 아닙니다. 애니메이션 재생 및 행동을 취소합니다.");
            return; // 턴이 아니므로 여기서 바로 종료!
        }

        // 턴이 맞는 경우에만 아래 로직이 실행됩니다.

        // 1. [핵심] 턴 매니저에게 '행동 시작'을 알리고, 턴 상태를 Action으로 변경합니다.
        turnManager.StartPlayerAction("ATTACK");

        // 2. 공격 애니메이션 재생 (이제 턴 체크를 통과한 후 실행됨)
        spumPrefabs.PlayAnimation(PlayerState.ATTACK, 0);

        // 3. 애니메이션 재생이 끝날 때까지 기다린 후 턴을 넘깁니다.
        float animationDuration = 1.0f; // 애니메이션 길이에 맞게 설정
        StartCoroutine(WaitForActionEnd(animationDuration));
    }

    IEnumerator WaitForActionEnd(float duration)
    {
        // 애니메이션 재생 시간만큼 기다립니다.
        yield return new WaitForSeconds(duration);

        // 4. 애니메이션이 끝났으니 턴 매니저에게 알리고 턴을 종료합니다.
        if (turnManager != null)
        {
            turnManager.FinishPlayerAction();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && scanObject != null)
            manager.Action(scanObject);
    }
}
