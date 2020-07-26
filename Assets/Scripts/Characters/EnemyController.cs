using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MoveToTarget), typeof(MoveAwayFromTarget), typeof(MoveToNearbyPosition))]
public class EnemyController : UnitController
{
	public Enemy so;
	[SerializeField] private bool killable = true;
	public bool Killable { get => killable; }

	new void Awake(){
		base.Awake();
		gameObject.tag = "Enemy";
	}

	public override void TakeDamage(UnitController attacker){
		if(killable){
			base.TakeDamage(attacker);
		}
	}

	protected override void Die(){
		so.Kill();
		base.Die();
	}

	void OnCollisionEnter2D(Collision2D collision){
		if(collision.gameObject.tag == "Player"){
			PlayerController player = collision.gameObject.GetComponent<PlayerController>();
			player.TakeDamage(this);
		}
	}
}
