using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int monsterID; // 몬스터 고유 식별자(ID)

    public int Health = 10;

    public GameObject DestroyedEffect;

    public void Damage(int attackpower)
    {
        Health -= attackpower;

        if(Health <= 0)
        {
            Die();         
        }
    }

    public void Die()
    {
        GameObject Temp = Instantiate(DestroyedEffect);
        Temp.transform.position = this.gameObject.transform.position;
        Destroy(Temp, 3.0f);
        Destroy(this.gameObject);
        StageManager.Instance.OnMonsterDeath(monsterID);
    }
}
