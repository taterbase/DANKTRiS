using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Game : MonoBehaviour {
	public GameObject piece;

	public float blockSize = 1f;
	public LayerMask blockingLayer;

	private int multiplier;
	private float blockHalf;

	private List<Piece>[] space;
	private int numOfRows;
	private int clearRowNum;

	private int width = 8;
	private int height = 10;

	private Vector2 origin = new Vector2 (-0.5f, 5.5f);

	private Text gameOverText;
	private Text titleText;
	private Text callToActionText;
	private Text scoreText;
	private int score = 0;
	private bool running = false;

	public List<Color> colors;

	public List<string> types;

	// Use this for initialization
	void Start () {
		gameOverText = GameObject.Find ("GameOverMessage").GetComponent<Text> ();
		titleText = GameObject.Find ("Title").GetComponent<Text> ();
		callToActionText = GameObject.Find ("CallToAction").GetComponent<Text> ();
		scoreText = GameObject.Find ("Score").GetComponent<Text> ();

		multiplier = Mathf.FloorToInt (1 / blockSize);
		numOfRows = height * multiplier;

		clearRowNum = multiplier * width;
		blockHalf = blockSize / 2;
	}

	void Update() {
		if (!running && Input.GetKey (KeyCode.Return) || Input.GetKey(KeyCode.KeypadEnter)) {
			ClearBoard ();
		}
	}

	void ClearBoard() {
		running = true;
		gameOverText.enabled = false;
		titleText.enabled = false;
		callToActionText.enabled = false;
		score = 0;
		SetScore ();

		foreach (Transform child in transform) {
			Destroy (child.gameObject);
		}
		space = new List<Piece>[numOfRows];

		AddPiece ();
	}

	void SetScore() {
		scoreText.text = "Score: " + score;
	}

	void AddPiece() {
		StartCoroutine (AddPieceCoroutine ());
	}

	IEnumerator AddPieceCoroutine() {
		Collider2D hit = Physics2D.OverlapPoint (origin, blockingLayer);
		if (hit != null) {
			GameOver ();
		} else {
			yield return new WaitForSeconds (.5f);
			GameObject currentPiece = Instantiate (piece);
			currentPiece.transform.position = new Vector2 (origin.x, origin.y);
			currentPiece.transform.parent = this.transform;

			AssignType (currentPiece, types[Random.Range(0, types.Count)]);
			AssignColor (currentPiece);

		}
	}

	void AssignColor(GameObject piece) {
		Color color = colors [0];
		color.a = 1;
		colors.RemoveAt (0);
		colors.Add (color);

		piece.SendMessage("AssignColor", color);

	}

	void AssignType(GameObject piece, string type) {
		piece.SendMessage ("AssignType", type);
	}

	void FreezePiece(PieceContainer container) {
		List<int> rowsToClear = new List<int> ();

		foreach (Piece  piece in container.pieces) {
			int row = Mathf.CeilToInt (piece.transform.position.y + blockHalf) + ((height/2) * multiplier) - 1;

			if (row >= numOfRows || row < 0)
				continue;
					
			if (space [row] == null) {
				space [row] = new List<Piece> ();
			}
			
			space [row].Add (piece);

			if (space [row].Count == clearRowNum) {
				if (rowsToClear.IndexOf(row) == -1) {
					rowsToClear.Add (row);
				}
			}
		}
		rowsToClear.Sort ();
		rowsToClear.Reverse ();
		Clear (rowsToClear);

		AddPiece ();
	}

	void Clear(List<int> rows) {

		foreach (int row in rows) {
			foreach (Piece piece in space[row]) {
				piece.Dissolve ();
			}

			for (int i = row; i < numOfRows - 1; ++i) {
				space [i] = space [i + 1];
				if (space [i] == null)
					break;
				else {
					foreach (Piece piece in space[i]) {
						piece.MoveDownDammit ();
					}
				}
			}

			space [numOfRows - 1] = new List<Piece> ();

			score += 10;
			SetScore ();
		}

	}

	void GameOver() {
		running = false;
		gameOverText.enabled = true;
		callToActionText.enabled = true;
		callToActionText.text = "Press ENTER to try again";
	}
}
