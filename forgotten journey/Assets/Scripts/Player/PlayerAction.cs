using UnityEditor.Build.Content;
using UnityEngine;
using System.Collections;

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

        // 1. [핵심] 턴 매니저에게 '행동 시작'만 알립니다. (턴 종료 코루틴이 시작되지 않음)
        // 이 함수가 state = BattleState.Action 으로 변경합니다.
        turnManager.StartPlayerAction("ATTACK");

        // 2. 공격 애니메이션 재생
        spumPrefabs.PlayAnimation(PlayerState.ATTACK, 0);

        // 3. 애니메이션 재생이 끝날 때까지 기다립니다.
        // SPUM_Prefabs의 애니메이션 길이를 알아내거나, 이벤트 함수를 통해 처리해야 합니다.
        float animationDuration = 1.0f; // 애니메이션 길이에 맞게 설정하세요!
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
