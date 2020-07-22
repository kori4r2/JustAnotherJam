using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Movable))]
public abstract class MoveTo : MonoBehaviour {
	// Funcao que retorna a direcao na qual o objeto deve se mover
	// Apropriada para movimentacao baseada em velocidade de Rigidbody2D
	public abstract Vector2 Direction { get; }

	public virtual void Activate(){
		enabled = true;
	}
	public virtual void Deactivate(){
		enabled = false;
	}
}