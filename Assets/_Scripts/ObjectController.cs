using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;


public class ObjectController : MonoBehaviour, IPointerDownHandler {



	public Camera mainCam;
	public GameObject circlePrefab;



	public float scallingToSize=24f;
	public float scallingTime=1f;


	private float _showingLevelTime=1f;


	[SerializeField]private SpriteRenderer _spriteRenderer;

	private Vector2 _originScale;



	private bool _isTheLastOne=true;

	private bool _isScaling=false;
	private float _scallingSpeed;

	private float _usingtime=0f;




	// Use this for initialization
	void Start () {

		SimpleTransform tran = GetRandomTransform ();
		_originScale = tran.LocalScale;

		transform.localScale = _originScale;
		transform.position = tran.Position;
		transform.rotation = tran.Rotation;

		GameManager.instance.SetForbiddenArea (tran);

		_spriteRenderer.color=GetRandomColor();

		_scallingSpeed = scallingToSize / scallingTime;

		_showingLevelTime = GameManager.instance.showingLevelTime;

	}
	
	void Update(){

		//scale to screen edges

		if (_isScaling && _usingtime<=scallingTime) {
			transform.localScale += new Vector3 (_scallingSpeed, _scallingSpeed,0)*Time.deltaTime;


			_usingtime += Time.deltaTime;
		}



	}

	private SimpleTransform GetRandomTransform(){


		return GameManager.instance.GetUniqueRandomTransform ();

	}




	private Color GetRandomColor(){



		return GameManager.instance.GetRandomColor ();
	}





	private void GetBackState(){

		Debug.Log (name+" Return to origin scale");
		transform.localScale = _originScale;
		Instantiate (circlePrefab);

	}

	public void ShowRound(){

		GameManager.instance.ShowRound ();

	}

	public void GameOver(){


		GameManager.instance.GameOver ();
	}

	public void OnPointerDown(PointerEventData eventData){
		//Debug.Log ("Pointer enter");


		if (_isTheLastOne) {

			//be bgger to fulfill the screen
			//then islastone is false


			_isScaling = true;
			Invoke ("GetBackState", scallingTime+_showingLevelTime);
			Invoke ("ShowRound", scallingTime);


			_isTheLastOne = false;


		} 
			else {

			GameOver ();
			//game lost
			//the real last vibrating



		}

	}



}
