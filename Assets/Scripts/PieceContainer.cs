using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PieceContainer : MonoBehaviour {

	public string type = "block";
	public Color color;
	public Piece buildingBlock;
	public LayerMask blockingLayer;
	public float FallRate = 1f;
	public float CoolDownRate = .3f;
	public List<Piece> pieces;

	private bool InCoolDown = false;
	private int state = 0;

	// Use this for initialization
	void Start () {
		CreateNew ();
		StartCoroutine (Fall ());
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetKey (KeyCode.Space)) {
			Rotate ();
		} else if (Input.GetKey (KeyCode.LeftArrow)) {
			Move ("left");
		} else if (Input.GetKey (KeyCode.RightArrow)) {
			Move ("right");
		} else if (Input.GetKey (KeyCode.DownArrow)) {
			Slam ();
		}
	}

	public bool Move(string dir) {
		if (InCoolDown && dir != "down")
			return false;

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

		bool canMove = true;

		foreach (Piece piece in pieces) {
			if (piece.CanMove (dir))
				continue;
			else {
				canMove = false;
				break;
			}
		}

		if (canMove) {
			transform.position = end;
			StartCoroutine (CoolDown ());
			return true;
		} else {
			return false;
		}
	}

	void Freeze() {
		foreach (Piece piece in pieces) {
			piece.Freeze ();
		}
		this.InCoolDown = false;
		this.StopAllCoroutines ();
		this.enabled = false;
		this.transform.parent.SendMessage ("FreezePiece", this);
	}

	void Rotate() {
		if (InCoolDown)
			return;

		state = (state + 1) % 4;

		switch (type) {
		case "L":
			RotateLToState (state);
			break;
		case "T":
			RotateTToState (state);
			break;
		case "S":
			RotateSToState (state);
			break;
		case "Z":
			RotateZToState (state);
			break;
		case "I":
			RotateIToState (state);
			break;
		}


		StartCoroutine(CoolDown ());
	}

	void RotateLToState(int state) {
		List<Vector2> positions = new List<Vector2> ();
		switch (state) {
		case 0:
			positions.Add (new Vector2 (0, 0));
			positions.Add (new Vector2 (0, -1));
			positions.Add (new Vector2 (0, -2));
			positions.Add (new Vector2 (1, -2));
			break;
		case 1:
			positions.Add (new Vector2 (0, 0));
			positions.Add (new Vector2 (1, 0));
			positions.Add (new Vector2 (2, 0));
			positions.Add (new Vector2 (2, 1));
			break;
		case 2:
			positions.Add (new Vector2 (0, 0));
			positions.Add (new Vector2 (1, 0));
			positions.Add (new Vector2 (1, -1));
			positions.Add (new Vector2 (1, -2));
			break;
		case 3:
			positions.Add (new Vector2 (0, 0));
			positions.Add (new Vector2 (1, 0));
			positions.Add (new Vector2 (2, 0));
			positions.Add (new Vector2 (0, -1));
			break;
		}

		Collider2D hit = null;
		Vector2 containerPosition = (Vector2)transform.position;

		foreach (Vector2 position in positions) {
			hit = Physics2D.OverlapPoint (containerPosition + position, blockingLayer);
			if (hit != null) {
				if (--state < 0 ) {
					state = 4;
				}
				return;
			}
		}
		for (int i = 0; i < pieces.Count; ++i) {
			pieces [i].transform.localPosition = positions [i];
		}
	}

	void RotateTToState(int state) {
		List<Vector2> positions = new List<Vector2> ();
		switch (state) {
		case 0:
			positions.Add (new Vector2 (0, 0));
			positions.Add (new Vector2 (1, 0));
			positions.Add (new Vector2 (2, 0));
			positions.Add (new Vector2 (1, -1));
			break;
		case 1:
			positions.Add (new Vector2 (1, 0));
			positions.Add (new Vector2 (0, -1));
			positions.Add (new Vector2 (1, -1));
			positions.Add (new Vector2 (1, -2));
			break;
		case 2:
			positions.Add (new Vector2 (1, 0));
			positions.Add (new Vector2 (0, -1));
			positions.Add (new Vector2 (1, -1));
			positions.Add (new Vector2 (2, -1));
			break;
		case 3:
			positions.Add (new Vector2 (0, 0));
			positions.Add (new Vector2 (0, -1));
			positions.Add (new Vector2 (1, -1));
			positions.Add (new Vector2 (0, -2));
			break;
		}

		Collider2D hit = null;
		Vector2 containerPosition = (Vector2)transform.position;

		foreach (Vector2 position in positions) {
			hit = Physics2D.OverlapPoint (containerPosition + position, blockingLayer);
			if (hit != null) {
				if (--state < 0 ) {
					state = 4;
				}
				return;
			}
		}
		for (int i = 0; i < pieces.Count; ++i) {
			pieces [i].transform.localPosition = positions [i];
		}
	}

	void RotateSToState(int state) {
		List<Vector2> positions = new List<Vector2> ();
		switch (state) {
		case 0:
			positions.Add (new Vector2 (1, 0));
			positions.Add (new Vector2 (2, 0));
			positions.Add (new Vector2 (1, -1));
			positions.Add (new Vector2 (0, -1));
			break;
		case 1:
			positions.Add (new Vector2 (0, 0));
			positions.Add (new Vector2 (0, -1));
			positions.Add (new Vector2 (1, -1));
			positions.Add (new Vector2 (1, -2));
			break;
		case 2:
			positions.Add (new Vector2 (1, 0));
			positions.Add (new Vector2 (2, 0));
			positions.Add (new Vector2 (1, -1));
			positions.Add (new Vector2 (0, -1));
			break;
		case 3:
			positions.Add (new Vector2 (0, 0));
			positions.Add (new Vector2 (0, -1));
			positions.Add (new Vector2 (1, -1));
			positions.Add (new Vector2 (1, -2));
			break;
		}

		Collider2D hit = null;
		Vector2 containerPosition = (Vector2)transform.position;

		foreach (Vector2 position in positions) {
			hit = Physics2D.OverlapPoint (containerPosition + position, blockingLayer);
			if (hit != null) {
				if (--state < 0 ) {
					state = 4;
				}
				return;
			}
		}
		for (int i = 0; i < pieces.Count; ++i) {
			pieces [i].transform.localPosition = positions [i];
		}
	}

	void RotateZToState(int state) {
		List<Vector2> positions = new List<Vector2> ();
		switch (state) {
		case 0:
			positions.Add (new Vector2 (0, 0));
			positions.Add (new Vector2 (1, 0));
			positions.Add (new Vector2 (1, -1));
			positions.Add (new Vector2 (2, -1));
			break;
		case 1:
			positions.Add (new Vector2 (2, 0));
			positions.Add (new Vector2 (2, -1));
			positions.Add (new Vector2 (1, -1));
			positions.Add (new Vector2 (1, -2));
			break;
		case 2:
			positions.Add (new Vector2 (0, 0));
			positions.Add (new Vector2 (1, 0));
			positions.Add (new Vector2 (1, -1));
			positions.Add (new Vector2 (2, -1));
			break;
		case 3:
			positions.Add (new Vector2 (2, 0));
			positions.Add (new Vector2 (2, -1));
			positions.Add (new Vector2 (1, -1));
			positions.Add (new Vector2 (1, -2));
			break;
		}

		Collider2D hit = null;
		Vector2 containerPosition = (Vector2)transform.position;

		foreach (Vector2 position in positions) {
			hit = Physics2D.OverlapPoint (containerPosition + position, blockingLayer);
			if (hit != null) {
				if (--state < 0 ) {
					state = 4;
				}
				return;
			}
		}
		for (int i = 0; i < pieces.Count; ++i) {
			pieces [i].transform.localPosition = positions [i];
		}
	}

	void RotateIToState(int state) {
		List<Vector2> positions = new List<Vector2> ();
		switch (state) {
		case 0:
			positions.Add (new Vector2 (0, 0));
			positions.Add (new Vector2 (0, -1));
			positions.Add (new Vector2 (0, -2));
			positions.Add (new Vector2 (0, -3));
			break;
		case 1:
			positions.Add (new Vector2 (0, 0));
			positions.Add (new Vector2 (1, 0));
			positions.Add (new Vector2 (2, 0));
			positions.Add (new Vector2 (3, 0));
			break;
		case 2:
			positions.Add (new Vector2 (0, 0));
			positions.Add (new Vector2 (0, -1));
			positions.Add (new Vector2 (0, -2));
			positions.Add (new Vector2 (0, -3));
			break;
		case 3:
			positions.Add (new Vector2 (0, 0));
			positions.Add (new Vector2 (1, 0));
			positions.Add (new Vector2 (2, 0));
			positions.Add (new Vector2 (3, 0));
			break;
		}

		Collider2D hit = null;
		Vector2 containerPosition = (Vector2)transform.position;

		foreach (Vector2 position in positions) {
			hit = Physics2D.OverlapPoint (containerPosition + position, blockingLayer);
			if (hit != null) {
				if (--state < 0 ) {
					state = 4;
				}
				return;
			}
		}
		for (int i = 0; i < pieces.Count; ++i) {
			pieces [i].transform.localPosition = positions [i];
		}
	}

	void Slam() {
		while(Move("down")){}
		Freeze ();
	}

	void CreateNew() {
		pieces = new List<Piece> ();
		switch (type) {
		case "L":
			CreateL ();
			break;
		case "T":
			CreateT ();
			break;
		case "S":
			CreateS ();
			break;
		case "Z":
			CreateZ ();
			break;
		case "I":
			CreateI ();
			break;
		case "BLOCK":
			CreateBLOCK ();
			break;
		}
	}

	void CreateL() {
		CreateBlock (0, 0);
		CreateBlock (0, -1);
		CreateBlock (0, -2);
		CreateBlock (1, -2);
	}

	void CreateT() {
		CreateBlock (0, 0);
		CreateBlock (1, 0);
		CreateBlock (2, 0);
		CreateBlock (1, -1);
	}

	void CreateS() {
		CreateBlock (2, 0);
		CreateBlock (1, 0);
		CreateBlock (1, -1);
		CreateBlock (0, -1);
	}

	void CreateZ() {
		CreateBlock (0, 0);
		CreateBlock (1, 0);
		CreateBlock (1, -1);
		CreateBlock (2, -1);
	}

	void CreateI() {
		CreateBlock (0, 0);
		CreateBlock (0, -1);
		CreateBlock (0, -2);
		CreateBlock (0, -3);
	}

	void CreateBLOCK() {
		CreateBlock (0, 0);
		CreateBlock (1, 0);
		CreateBlock (0, -1);
		CreateBlock (1, -1);
	}

	void CreateBlock(int x, int y) {
		Piece block1 = Instantiate (buildingBlock);
		block1.transform.parent = transform;
		block1.transform.localPosition = new Vector2 (x, y);

		SpriteRenderer spr = block1.GetComponent<SpriteRenderer> ();
		spr.color = color;

		pieces.Add (block1);
	}

	void AssignColor(Color assignedColor) {
		color = assignedColor;
	}

	void AssignType(string assignedType) {
		type = assignedType;
	}


	IEnumerator Fall() {
		while (Move("down")) {
			yield return new WaitForSeconds (FallRate);
		}
		Freeze ();
	}

	IEnumerator CoolDown () {
		InCoolDown = true;
		yield return new WaitForSeconds (CoolDownRate);
		InCoolDown = false;
	}
}
