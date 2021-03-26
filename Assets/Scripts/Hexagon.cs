using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hexagon : SuperClass {

	GridManager GridManagerObject;
	public int x;
	public int y;
	private int bombTimer;

	public Color color;

	public Vector2 lerpPosition;
	public Vector2 speed;

	public bool Islerp;
	
	private TextMesh text;


	/* Birbirine yakın hexagonları yapılandırmak için */
	public struct NearHexes
	{
		public Vector2 up;
		public Vector2 upLeft;
		public Vector2 upRight;
		public Vector2 down;
		public Vector2 downLeft;
		public Vector2 downRight;
	}



	void Start() 
	{
		GridManagerObject = GridManager.instance;
		Islerp = false;
	}

	void Update() 
	{
		if (Islerp)
		{
			float newX = Mathf.Lerp(transform.position.x, lerpPosition.x, Time.deltaTime*HEXAGON_ROTATE_CONSTANT);
			float newY = Mathf.Lerp(transform.position.y, lerpPosition.y, Time.deltaTime*HEXAGON_ROTATE_CONSTANT);
			transform.position = new Vector2(newX, newY);
			
			if (Vector3.Distance(transform.position, lerpPosition) < HEXAGON_ROTATE_THRESHOLD) 
			{
				transform.position = lerpPosition;
				Islerp = false;
			}
		}
	}


	/* Rotation olayları burada kaydediliyor */
	public void Rotate(int newX, int newY, Vector2 newPos) {
		lerpPosition = newPos;
		SetX(newX);
		SetY(newY);
		Islerp = true;
	}

	public bool IsRotating() {
		return Islerp;
	}

	public bool IsMoving() {
		return !(GetComponent<Rigidbody2D>().velocity == Vector2.zero);
	}

	public void Exploded() {
		GetComponent<Collider2D>().isTrigger = true;
	}

	/* griddeki komşu hexagonlar için yapılandırma işlemi yapılıyor */
	public NearHexes GetNearHexes() {
		NearHexes nearHexes;
		bool onStepper = GridManagerObject.OnStepper(GetX());

		nearHexes.down = new Vector2(x, y-1);
		nearHexes.up = new Vector2(x, y+1);
		nearHexes.upLeft = new Vector2(x-1, onStepper ? y+1 : y);
		nearHexes.upRight = new Vector2(x+1, onStepper ? y+1 : y);
		nearHexes.downLeft = new Vector2(x-1, onStepper ? y : y-1);
		nearHexes.downRight = new Vector2(x+1, onStepper ? y : y-1);


		return nearHexes;
	}



	/* hexagon için yeni pozisyon ayarlaması */
	public void ChangeWorldPosition(Vector2 newPosition) {
		lerpPosition = newPosition;
		Islerp = true;
	}



	/* hexagon için gridde yeni pozisyon ayarlaması */
	public void ChangeGridPosition(Vector2 newPosition) {
		x = (int)newPosition.x;
		y = (int)newPosition.y;
	}

	public void SetBomb() {
		text = new GameObject().AddComponent<TextMesh>();
		text.alignment = TextAlignment.Center;
		text.anchor = TextAnchor.MiddleCenter;
		text.transform.localScale = transform.localScale;
		text.transform.position = new Vector3(transform.position.x, transform.position.y, -4);
		text.color = Color.black;
		text.transform.parent = transform;
		bombTimer = BOMB_TIMER_START;
		text.text = bombTimer.ToString();
	}


	/* Set ve get işlemleri burada yapılıyor */
	public void SetX(int value) { x = value; }
	public void SetY(int value) { y = value; }
	public void SetColor(Color newColor) { GetComponent<SpriteRenderer>().color = newColor; color=newColor; }
	public void Tick() { --bombTimer; text.text = bombTimer.ToString(); }

	public int GetX() { return x; }
	public int GetY() { return y; }
	public Color GetColor() { return GetComponent<SpriteRenderer>().color; }
	public int GetTimer () { return bombTimer; }
}
