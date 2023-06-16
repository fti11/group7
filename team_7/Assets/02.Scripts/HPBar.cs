using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class HPBar : MonoBehaviour
{
    Image healthBar;
    float maxHealth = 100f;
    public static float health;

    //Start is called before the first frame update
    private void Start()
    {
        healthBar = GetComponent<Image>();
        health = maxHealth;
    }

    //Update is called once per frame
    private void Update()
    {
        healthBar.fillAmount = health / maxHealth;
    }
}
