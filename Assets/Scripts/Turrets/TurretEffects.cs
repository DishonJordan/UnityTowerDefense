using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretEffects : MonoBehaviour
{
	public Turret turret;
	[SerializeField]
	private ParticleSystem smoke;
	[SerializeField]
	private ParticleSystem fire;
	[SerializeField]
	private GameObject explosion;
	private static float smokeHP = 0.33f;

	private void OnEnable()
	{
		turret.OnHealthChanged += this.OnHealthChanged;
	}

	private void OnDisable()
	{
		turret.OnHealthChanged -= this.OnHealthChanged;
	}

	private void OnHealthChanged(float health)
	{
		Smoke = (health / turret.maxHealth) <= smokeHP;
		if(health <= 0)
		{
			Explosion();
			Fire = true;
		}
		else
		{
			Fire = false;
		}
	}

	public bool Smoke
	{
		get => smoke.emission.enabled;
		set
		{
			ParticleSystem.EmissionModule em = smoke.emission;
			em.enabled = value;
		}
	}

	public bool Fire
	{
		get => fire.emission.enabled;
		set
		{
			ParticleSystem.EmissionModule em = fire.emission;
			em.enabled = value;
		}
	}

	public void Explosion()
	{
		GameObject exp = Instantiate(explosion, transform.position, transform.rotation);
		Destroy(exp, 3f);
	}
}
