using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAwayFromTarget : MoveTo
{
	[SerializeField] private float maxDistance = 0.5f;
	[SerializeField] public float minDistance = 0.5f;
	public Transform target;
	private Rigidbody2D rigid2D;

	// Salva a referencia para o rigidbody
	void Awake() {
		rigid2D = GetComponent<Rigidbody2D>();
		Deactivate();
	}

	override public Vector2 Direction{
		get{
			// Calcula a distancia ate o alvo a ser seguido
			Vector2 target2Dposition = new Vector2(target.position.x, target.position.y);
			float distance = Vector2.Distance(rigid2D.position, target2Dposition);
			// Se estiver abaixo da distancia minima permitida, retorna o inverso da direcao ao alvo normalizada
			if(distance < minDistance)
				return (target2Dposition - rigid2D.position).normalized;

			// Se ja estiver a uma distancia segura, para
			if(distance > maxDistance)
				return Vector2.zero;

			// Caso contrario, mantem a velocidade atual
			return rigid2D.velocity.normalized;
		}
	}
}
