using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToInput : MoveTo {
	private Vector2 input;

	// O objeto deve estar parado inicialmente
	void Awake(){
		Deactivate();
	}

	// A direcao a ser seguida e determinada apenas pelo input
	override public Vector2 Direction{
		get{
			return input.normalized;
		}
	}

	public override void Activate(){
		input = Vector2.zero;
		base.Activate();
	}

	public override void Deactivate(){
		input = Vector2.zero;
		base.Deactivate();
	}

	// Update is called once per frame
	void Update() {
		// if (!PauseMenu.IsPaused)
		// {
			input.x = Input.GetAxisRaw("Horizontal");
			input.y = Input.GetAxisRaw("Vertical");
		// }
		// else
		// 	input = Vector2.zero;
	}
}