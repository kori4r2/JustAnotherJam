using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Movable : MonoBehaviour {
	public float moveSpeed = 5f;
	[SerializeField] private MoveTo nextPosition;
	public MoveTo NextPosition{
		get => nextPosition;
		set{
			nextPosition?.Deactivate();
			nextPosition = value;
			nextPosition?.Activate();
		}
	}
	private bool canMove;
	public bool CanMove{
		get { return canMove; }
		set {
			if(value != canMove)
				rigid2D.velocity = Vector2.zero;

			canMove = value;

			if(canMove)
				nextPosition?.Activate();
			else
				nextPosition?.Deactivate();
		}
	}
	public Vector2 Direction{ get; private set; } = Vector2.down;
	public Vector2 CurrentSpeed { get => rigid2D?.velocity ?? Vector2.zero; }
	private Rigidbody2D rigid2D;
	private Animator anim;

	public void Reset(){
		moveSpeed = 5f;
		Rigidbody2D rb2D = GetComponent<Rigidbody2D>();
		rb2D.bodyType = RigidbodyType2D.Dynamic;
		rb2D.sharedMaterial = null;
		rb2D.simulated = true;
		rb2D.useFullKinematicContacts = true;
		rb2D.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
		rb2D.sleepMode = RigidbodySleepMode2D.StartAwake;
		rb2D.interpolation = RigidbodyInterpolation2D.None;
		rb2D.constraints = RigidbodyConstraints2D.FreezeRotation;
		rb2D.gravityScale = 0f;
	}

	// Salva a referencia para o rigdigbody
	void Awake(){
		rigid2D = GetComponent<Rigidbody2D>();
		anim = GetComponent<Animator>();
	}

	// Atualiza a velocidade atual de acordo com a direcao definida pelo script de MoveTo
	void FixedUpdate(){
		if(nextPosition != null && canMove){
			Vector2 direction = nextPosition.Direction;
			// No caso da direcao ser um vetor zero ou da moveSpeed ser 0, fica parado
			rigid2D.velocity = moveSpeed * direction;
		}
		if (anim != null) {
			// anim.SetBool("moving", rigid2D.velocity != Vector2.zero);
			// anim.SetFloat("moveX", rigid2D.velocity.normalized.x);
			// anim.SetFloat("moveY", rigid2D.velocity.normalized.y);
			if(Mathf.Abs(rigid2D.velocity.x) - Mathf.Abs(rigid2D.velocity.y) > Mathf.Epsilon){
				// anim.SetFloat("directionY", 0f);
				// anim.SetFloat("directionX", rigid2D.velocity.normalized.x);
				if(CanMove){
					Direction = new Vector2((rigid2D.velocity.x > float.Epsilon)? 1 : (rigid2D.velocity.x < -float.Epsilon)? -1 : 0 , 0);
				}
			}else if(Mathf.Abs(rigid2D.velocity.y) - Mathf.Abs(rigid2D.velocity.x) > Mathf.Epsilon){
				// anim.SetFloat("directionX", 0f);
				// anim.SetFloat("directionY", rigid2D.velocity.normalized.y);
				if(CanMove){
					Direction = new Vector2(0, (rigid2D.velocity.y > float.Epsilon)? 1 : (rigid2D.velocity.y < -float.Epsilon)? -1 : 0);
				}
			}
		}
	}
}