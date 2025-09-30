using UnityEngine;
using System.Collections; // Coroutine을 사용하기 위해 필요합니다.

public class TurnManager : MonoBehaviour
{
    public BattleState state; // 현재 전투 상태

    // 전투에 참여하는 플레이어 및 적 유닛 스크립트 (나중에 연결)
    // [SerializeField] private Unit playerUnit; 
    // [SerializeField] private Unit enemyUnit;

    void Start()
    {
        // 전투 시작
        state = BattleState.Start;
        StartCoroutine(SetupBattle());
    }

    /// <summary>
    /// 전투를 초기화하고 첫 턴을 시작합니다.
    /// </summary>
    IEnumerator SetupBattle()
    {
        // 1. UI 설정 (예: 전투 시작 메시지 출력)
        // battleMessageText.text = "전투 시작!";
        Debug.Log("전투를 시작합니다.");

        // 2. 캐릭터 정보 로딩 및 준비 (HP, 스탯 등)
        yield return new WaitForSeconds(1f); // 1초 대기

        // 3. 첫 턴 결정 (플레이어가 선공한다고 가정)
        state = BattleState.PlayerTurn;
        PlayerTurn();
    }

    /// <summary>
    /// 플레이어의 턴이 시작될 때 호출됩니다.
    /// </summary>
    void PlayerTurn()
    {
        // UI를 활성화하여 플레이어에게 행동 선택 기회를 제공합니다.
        // UIManager.Instance.ShowActionPanel(); 
        Debug.Log("플레이어 턴: 행동을 선택하세요.");

        // 이 상태에서 플레이어의 버튼 입력(공격, 마법 등)을 기다립니다.
    }

    /// <summary>
    /// 플레이어의 행동(예: 공격 버튼 클릭)이 들어왔을 때 외부에서 호출됩니다.
    /// </summary>
    public void OnPlayerActionSelected()
    {
        if (state != BattleState.PlayerTurn)
            return;

        state = BattleState.Action; // 상태를 행동 실행으로 변경
        StartCoroutine(PlayerActionCoroutine());
    }

    /// <summary>
    /// 플레이어의 행동을 실행하는 코루틴입니다.
    /// </summary>
    IEnumerator PlayerActionCoroutine()
    {
        // 1. 플레이어의 행동(애니메이션, 데미지 계산) 실행
        // (예: playerUnit.Attack(enemyUnit);)
        Debug.Log("플레이어가 행동을 실행합니다.");

        yield return new WaitForSeconds(2f); // 행동 애니메이션 시간 대기

        // 2. 적의 생존 확인
        // if (enemyUnit.isDead)
        // {
        //     state = BattleState.Won;
        //     EndBattle();
        //     yield break;
        // }

        // 3. 다음 턴으로 전환
        state = BattleState.EnemyTurn;
        StartCoroutine(EnemyTurnCoroutine());
    }

    /// <summary>
    /// 적의 턴을 처리하는 코루틴입니다.
    /// </summary>
    IEnumerator EnemyTurnCoroutine()
    {
        Debug.Log("적의 턴이 시작됩니다.");

        // 1. 잠시 대기 (생각하는 시간)
        yield return new WaitForSeconds(1f);

        // 2. 적의 행동(AI) 결정 및 실행
        // (예: enemyUnit.DecideAndAct(playerUnit);)
        Debug.Log("적이 행동을 실행합니다.");

        yield return new WaitForSeconds(2f); // 적 행동 시간 대기

        // 3. 플레이어의 생존 확인
        // if (playerUnit.isDead)
        // {
        //     state = BattleState.Lost;
        //     EndBattle();
        //     yield break;
        // }

        // 4. 다음 턴(플레이어 턴)으로 전환
        state = BattleState.PlayerTurn;
        PlayerTurn();
    }

    /// <summary>
    /// 전투 종료를 처리합니다.
    /// </summary>
    void EndBattle()
    {
        if (state == BattleState.Won)
        {
            Debug.Log("전투에서 승리했습니다!");
        }
        else if (state == BattleState.Lost)
        {
            Debug.Log("전투에서 패배했습니다.");
        }
    }

    public void StartPlayerAction(string actionType)
    {
        if (state != BattleState.PlayerTurn)
        {
            Debug.Log("지금은 플레이어 턴이 아닙니다. 행동 취소.");
            return;
        }

        // 턴을 Action 상태로 변경하여 다른 입력을 막습니다.
        state = BattleState.Action;
        Debug.Log($"턴 소비 행동: {actionType} 시작. 상태: Action");

        // **[핵심]** 코루틴은 아직 시작하지 않습니다. PlayerAction이 애니메이션을 재생한 후 호출할 겁니다.
    }

    public void FinishPlayerAction()
    {
        // 애니메이션이 끝났으니 이제 다음 턴으로 넘어갈 수 있습니다.
        StartCoroutine(PlayerActionCoroutine());
    }

    public enum BattleState
    {
        Start,          // 전투 시작 (초기화 단계)
        PlayerTurn,     // 플레이어가 행동을 선택할 차례
        EnemyTurn,      // 적이 행동을 하는 차례
        Action,         // 플레이어나 적의 행동이 실제로 실행되는 중
        Won,            // 전투 승리
        Lost            // 전투 패배
    }
}