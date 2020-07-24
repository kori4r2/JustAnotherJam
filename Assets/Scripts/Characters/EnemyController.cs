using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MoveToTarget), typeof(MoveAwayFromTarget), typeof(MoveToNearbyPosition))]
public class EnemyController : UnitController
{
	public Enemy so;
	[SerializeField] private bool killable;
	public bool Killable { get => killable; }

	public override void Damage(UnitController attacker){
		if(killable){
			base.Damage(attacker);
		}
	}

	protected override void Die(){
		so.Kill();
		base.Die();
	}
}
