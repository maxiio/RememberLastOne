using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;


public class CircleController : MonoBehaviour, IPointerEnterHandler {



	public Camera mainCam;
	public GameObject circlePrefab;

	public bool isRandomSize=true;

	public bool isRandomColor=true;

	public float sizeMin=.5f;
	public float sizeMax=2f;

	public float scallingToSize=24f;
	public float scallingTime=1f;


	[SerializeField]private SpriteRenderer _spriteRenderer;

	private Vector2 _originScale;
	private bool _isTheLastOne=true;

	private bool _isScaling=false;
	private float _scallingSpeed;

	private float _usingtime=0f;


	// Use this for initialization
	void Start () {
		if (mainCam == null) {

			mainCam = Camera.main;
		}



		transform.position=GetRandomPosition();

		float scale = GetRandomSize ();

		_originScale = new Vector2 (scale, scale);

		transform.localScale = _originScale;

		_spriteRenderer.color=GetRandomColor();


		_scallingSpeed = scallingToSize / scallingTime;

	}
	
	void Update(){
		

		if (_isScaling && _usingtime<=scallingTime) {
			transform.localScale += new Vector3 (_scallingSpeed, _scallingSpeed,0)*Time.deltaTime;



			_usingtime += Time.deltaTime;
		}



	}



	private Vector2 GetRandomPosition(){

		float x=Random.Range(-mainCam.aspect*mainCam.orthographicSize+transform.localScale.x/2,
			mainCam.aspect*mainCam.orthographicSize-transform.localScale.x/2);
		
		float y=Random.Range(-mainCam.orthographicSize +transform.localScale.y/2,
			mainCam.orthographicSize -transform.localScale.y/2);


		return new Vector2(x,y);


	}

	private Color GetRandomColor(){

		float r=Random.Range(0f,1f);
		float g=Random.Range(0f,1f);
		float b=Random.Range(0f,1f);

		

		return new Color(r,g,b,1f);
	}


	private float GetRandomSize(){

		return Random.Range (sizeMin, sizeMax);
	}


	private void GetBackState(){

		Debug.Log ("Return to origin scale");
		transform.localScale = _originScale;
		Instantiate (circlePrefab);

	}

	public void OnPointerEnter(PointerEventData eventData){
		Debug.Log ("Pointer enter");


		if (_isTheLastOne) {

			//be bgger to fulfill the screen
			//then islastone is false


			_isScaling = true;
			Invoke ("GetBackState", scallingTime);
			_isTheLastOne = false;


		} else {

			Debug.Log ("Game Lost");
			//game lost
			//the real last vibrating



		}

	}



}
