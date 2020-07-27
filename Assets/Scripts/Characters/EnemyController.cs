using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineUpToTarget), typeof(MoveToNearbyPosition))]
public class EnemyController : UnitController
{
	public Enemy so;
	[SerializeField] private bool killable = true;
	public bool Killable { get => killable; }
	[SerializeField, Range(0, 5f)] private float aimingTime = 2f;
	[SerializeField, Range(0, 5f)] private float attackCooldown = 3f;
	private bool aiming = false;
	private bool chillin = false;
	private float atkTimer = 0;
	private LineUpToTarget alignToPlayer = null;
	private MoveToNearbyPosition moveToPosition = null;
	private Vector2 lastDirection = Vector2.down;

	public override Vector2 Direction{
		get{
			if(PlayerController.Instance && CanMove){
				Vector3 difference = PlayerController.Instance.transform.position - transform.position;
				if(Mathf.Abs(difference.y) > Mathf.Abs(difference.x)){
					if(difference.y > 0){
						lastDirection = Vector2.up;
					}else{
						lastDirection = Vector2.down;
					}
				}else{
					if(difference.x > 0){
						lastDirection = Vector2.right;
					}else{
						lastDirection = Vector2.left;
					}
				}
			}
			return lastDirection;
		}
	}

	new void Awake(){
		base.Awake();
		gameObject.tag = "Enemy";
		aiming = true;
		chillin = false;
		atkTimer = aimingTime;
		alignToPlayer = GetComponent<LineUpToTarget>();
		alignToPlayer.target = PlayerController.Instance.transform;
		moveToPosition = GetComponent<MoveToNearbyPosition>();
		movable.NextPosition = alignToPlayer;
		CanMove = true;
	}

	public override void Equip(Arms newArms){
		base.Equip(newArms);
		alignToPlayer.idealRange = newArms.IdealRange;
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

	void OnCollisionStay2D(Collision2D collision){
		if(collision.gameObject.tag == "Player"){
			PlayerController player = collision.gameObject.GetComponent<PlayerController>();
			player.TakeDamage(this);
		}
	}

	public new void Update(){
		base.Update();
		if(atkTimer <= 0f){
			if(CanMove){
				if(aiming && !chillin){
					StartAttack();
					aiming = false;
				}else if(!aiming && !chillin){
					chillin = true;
					movable.NextPosition = moveToPosition;
					atkTimer = attackCooldown;
				}else if(!aiming && chillin){
					chillin = false;
					aiming = true;
					movable.NextPosition = alignToPlayer;
					atkTimer = aimingTime;
				}
			}
		}else{
			atkTimer -= Time.deltaTime;
		}
	}
}
