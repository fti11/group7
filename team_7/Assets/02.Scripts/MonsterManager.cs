using UnityEngine;
using UnityEngine.SceneManagement;

public class MonsterManager : MonoBehaviour
{
    public GameObject[] monsters;  // 1�� ���� �ִ� ���͵��� ������ �迭

    private void Update()
    {
        // 1�� ���� �ִ� ��� ���Ͱ� �׾����� Ȯ��
        bool allMonstersDead = true;

        foreach (GameObject monster in monsters)
        {
            if (monster != null)
            {
                // ���Ͱ� ���� ��������� allMonstersDead�� false�� ����
                allMonstersDead = false;
                break;
            }
        }

        // ��� ���Ͱ� �׾����� 2�� ������ ��ȯ
        if (allMonstersDead)
        {
            SceneManager.LoadScene("GameScene");  // 2�� ������ ��ȯ (�� �̸��� ���� ���� �̸��� �Է��ؾ� �մϴ�)
        }
    }
}