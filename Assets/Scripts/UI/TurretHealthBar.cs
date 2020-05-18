using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurretHealthBar : MonoBehaviour
{
    public Turret turret;
    public Slider healthBar;

    private void Update()
    {
        healthBar.value = turret.health / turret.maxHealth;
    }
}
