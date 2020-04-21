using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyUI : MonoBehaviour
{
    public Enemy enemy;
    public Slider healthBar;
    private float initialHealth;

    private void Start()
    {
        initialHealth = enemy.health;
    }

    private void Update()
    {
        healthBar.value = enemy.health / initialHealth;
    }
}
