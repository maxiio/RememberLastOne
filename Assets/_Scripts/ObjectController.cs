using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;


public class ObjectController : MonoBehaviour, IPointerDownHandler {



	public Camera MainCam;
	public GameObject BaseElemetnPrefab;
	public GameObject BaseElementParent;



	public float ScallingToSize=24f;
	public float ScallingTime=1f;


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

		GameManager.Instance.SetForbiddenArea (tran);

		_spriteRenderer.color=GetRandomColor();

		_scallingSpeed = ScallingToSize / ScallingTime;

		_showingLevelTime = GameManager.Instance.ShowingLevelTime;

	}
	
	void Update(){

		//scale to screen edges

		if (_isScaling && _usingtime<=ScallingTime) {
			transform.localScale += new Vector3 (_scallingSpeed, _scallingSpeed,0)*Time.deltaTime;


			_usingtime += Time.deltaTime;
		}



	}

	private SimpleTransform GetRandomTransform(){


		return GameManager.Instance.GetUniqueRandomTransform ();

	}




	private Color GetRandomColor(){

		Color color=GameManager.Instance.GetRandomColor ();


		if (color.r -MainCam.backgroundColor.r <= .05f &&
			color.g -MainCam.backgroundColor.g <= .05f && 
			color.b -MainCam.backgroundColor.b <= .05f) //
		{

			color=GameManager.Instance.GetRandomColor ();
		} //do not return a color near the bg color



		return color;
	}





	private void GetBackState(){

		Debug.Log (name+" Return to origin scale");
		transform.localScale = _originScale;

		GameObject go =
			Instantiate (BaseElemetnPrefab) as GameObject;

		go.transform.parent = BaseElementParent.transform;
	}

	public void NextRound(){

		GameManager.Instance.NextRound ();

	}

	public void GameOver(){


		GameManager.Instance.GameOver ();
	}

	public void OnPointerDown(PointerEventData eventData){
		//Debug.Log ("Pointer enter");


		if (_isTheLastOne) {

			//be bgger to fulfill the screen
			//then islastone is false


			GetComponent<AudioSource> ().Play ();

			_isScaling = true;
			Invoke ("GetBackState", ScallingTime+_showingLevelTime);
			Invoke ("NextRound", ScallingTime);


			_isTheLastOne = false;


		} 
			else {

			GameOver ();
			//game lost
			//the real last vibrating



		}

	}



}
