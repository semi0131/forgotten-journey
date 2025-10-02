using UnityEngine;

// 유닛의 타입을 정의하여 코드로 구분합니다.
public enum UnitType { Player, Enemy }

public class CombatUnit : MonoBehaviour
{
    [Header("Unit Type")]
    public UnitType unitType = UnitType.Player;

    [Header("Unit Stats")]
    public string unitName = "Unit";
    public int maxHP = 100;
    public int currentHP;
    public int attackDamage = 10; // HealthSystem이 float을 받으므로, 이 값은 float으로 변환되어 사용됨
    public int defense = 5;

    public bool isDefending = false;

    // Animator 컴포넌트 참조
    private Animator anim;

    void Awake()
    {
        currentHP = maxHP;
        // Animator 컴포넌트를 자신이나 자식 오브젝트에서 찾습니다.
        anim = GetComponentInChildren<Animator>();
    }

    public bool IsDead()
    {
        return currentHP <= 0;
    }

    public void TakeDamage(int damage)
    {
        // 이 TakeDamage 함수는 이제 플레이어에게 데미지를 줄 때 HealthSystem에 의해 대체됩니다.
        // 하지만 몬스터의 체력 계산을 위해 남겨둡니다.
        int effectiveDamage = damage - defense;

        if (isDefending)
        {
            effectiveDamage -= defense;
            isDefending = false;
        }

        effectiveDamage = Mathf.Max(1, effectiveDamage);

        currentHP -= effectiveDamage;
        Debug.Log(unitName + " took " + effectiveDamage + " damage. HP remaining: " + currentHP);

        if (unitType == UnitType.Enemy && anim != null)
        {
            anim.SetTrigger("Hit");
        }
    }

    public void PlayDieAnimation()
    {
        if (anim != null)
        {
            anim.SetTrigger("Die");
        }
    }

    public void PlayAttackAnimation()
    {
        if (anim != null)
        {
            anim.SetTrigger("Attack");
        }
    }
}