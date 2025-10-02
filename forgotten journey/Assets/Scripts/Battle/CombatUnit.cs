using UnityEngine;

// ������ Ÿ���� �����Ͽ� �ڵ�� �����մϴ�.
public enum UnitType { Player, Enemy }

public class CombatUnit : MonoBehaviour
{
    [Header("Unit Type")]
    public UnitType unitType = UnitType.Player;

    [Header("Unit Stats")]
    public string unitName = "Unit";
    public int maxHP = 100;
    public int currentHP;
    public int attackDamage = 10; // HealthSystem�� float�� �����Ƿ�, �� ���� float���� ��ȯ�Ǿ� ����
    public int defense = 5;

    public bool isDefending = false;

    // Animator ������Ʈ ����
    private Animator anim;

    void Awake()
    {
        currentHP = maxHP;
        // Animator ������Ʈ�� �ڽ��̳� �ڽ� ������Ʈ���� ã���ϴ�.
        anim = GetComponentInChildren<Animator>();
    }

    public bool IsDead()
    {
        return currentHP <= 0;
    }

    public void TakeDamage(int damage)
    {
        // �� TakeDamage �Լ��� ���� �÷��̾�� �������� �� �� HealthSystem�� ���� ��ü�˴ϴ�.
        // ������ ������ ü�� ����� ���� ���ܵӴϴ�.
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