using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToTarget : MoveTo{
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
			// Se estiver acima da distancia maxima permitida, retorna a direcao ao alvo normalizada
			if(distance > maxDistance)
				return (rigid2D.position - target2Dposition).normalized;

			// Se ja estiver proximo demais, para
			if(distance < minDistance)
				return Vector2.zero;

			// Caso contrario, mantem a velocidade atual
			return rigid2D.velocity.normalized;
		}
	}
}