using UnityEngine;
using System.Collections;

public class Piece : MonoBehaviour {

	public float floor = -6;
	private BoxCollider2D boxCollider;
	public LayerMask blockingLayer;


	// Use this for initialization
	void Awake () {
		boxCollider = GetComponent<BoxCollider2D> ();
		boxCollider.enabled = false;
	}

	public void Freeze() {
		boxCollider.enabled = true;
	}

	public void Dissolve() {
		Destroy (this.gameObject);
	}


	public void MoveDownDammit() {
		Vector2 start = transform.position;
		Vector2 end = new Vector2 (start.x, start.y - 1);
		transform.position = end;
	}
		
	public bool CanMove(string dir) {
		Vector2 start = transform.position;
		Vector2 end;

		switch (dir) {
		case "left":
			end = new Vector2 (start.x - 1, start.y);
			break;
		case "right":
			end = new Vector2 (start.x + 1, start.y);
			break;
		case "down":
			end = new Vector2 (start.x, start.y - 1);
			break;
		default:
			return false;
		}

		RaycastHit2D hit = Physics2D.Linecast (start, end, blockingLayer);

		if (hit.transform == null) {
			return true;
		} else {
			return false;
		}
	}
}
