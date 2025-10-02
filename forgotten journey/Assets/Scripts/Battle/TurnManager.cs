using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

// ❗ BattleState는 클래스 밖에 정의하거나, 클래스 맨 위로 옮겨야 합니다.
public enum BattleState
{
    Start,
    PlayerTurn,
    EnemyTurn,
    Action,
    Won,
    Lost
}

public class TurnManager : MonoBehaviour
{
    public BattleState state;

    [SerializeField] private ButtonPageManager buttonPageManager;


    void Start()
    {
        if (buttonPageManager == null)
        {
            buttonPageManager = FindFirstObjectByType<ButtonPageManager>();
        }

        state = BattleState.Start;
        StartCoroutine(SetupBattle());
    }

    // ----------------------------------------------------
    //  전투 흐름 관리 (코루틴)
    // ----------------------------------------------------

    IEnumerator SetupBattle()
    {
        Debug.Log("전투를 시작합니다.");
        yield return new WaitForSeconds(1f);

        state = BattleState.PlayerTurn;
        PlayerTurn();
    }

    // 턴 시작 알림
    void PlayerTurn()
    {
        Debug.Log("플레이어 턴: 행동을 선택하세요.");
    }

    // 턴 종료 처리 (적 턴으로 전환)
    IEnumerator PlayerActionFinishCoroutine() // PlayerActionCoroutine 이름 변경
    {
        Debug.Log("턴매니저: 플레이어 행동 처리 중... (턴 종료 대기)");

        // 여기에 데미지 계산, 효과 적용 등의 로직이 들어갑니다.
        yield return null; // 한 프레임 대기

        // 턴 상태 변경 및 다음 턴 시작
        state = BattleState.EnemyTurn;
        Debug.Log("상태: EnemyTurn. 적 턴 시작.");
        StartCoroutine(EnemyTurnCoroutine());
    }

    IEnumerator EnemyTurnCoroutine()
    {
        Debug.Log("턴매니저: 적의 턴이 시작됩니다.");
        yield return new WaitForSeconds(1.5f); // 적 행동 대기 시간

        // 턴 상태 변경 및 플레이어 턴으로 복귀
        state = BattleState.PlayerTurn;

        if (buttonPageManager != null)
        {
            buttonPageManager.ResetToBattlePage();
        }
        Debug.Log("상태: PlayerTurn. 플레이어 턴 재시작.");
        PlayerTurn();
    }

    // ... (EndBattle 함수는 유지)

    // ----------------------------------------------------
    //  PlayerAction 스크립트 연동 함수
    // ----------------------------------------------------

    /// <summary>
    /// PlayerAction이 애니메이션 재생 전에 호출합니다.
    /// </summary>
    public void StartPlayerAction(string actionType)
    {
        if (state != BattleState.PlayerTurn)
        {
            Debug.Log("지금은 플레이어 턴이 아닙니다. 행동 취소.");
            return;
        }

        state = BattleState.Action;
        Debug.Log($"턴 소비 행동: {actionType} 시작. 상태: Action");
    }

    /// <summary>
    /// PlayerAction이 애니메이션 재생 후 지연 시간(Coroutine)이 끝났을 때 호출합니다.
    /// </summary>
    public void FinishPlayerAction()
    {
        // PlayerActionFinishCoroutine을 호출하여 적 턴으로 넘어갑니다.
        StartCoroutine(PlayerActionFinishCoroutine());
    }

    /// <summary>
    /// 도망가기 버튼에 연결될 함수. Forest_1 씬으로 전환합니다.
    /// </summary>
    public void RunFromBattle()
    {
        Debug.Log("전투에서 도망갑니다! Forest_1 씬으로 이동.");

        // **[핵심]** 씬을 로드합니다. (이름을 정확히 확인하세요)
        SceneManager.LoadScene("Forest_1");
    }
}