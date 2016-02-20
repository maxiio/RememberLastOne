using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour {
	public static GameManager instance=null;


	public Camera mainCam;

	public GameObject ShowLevelUI;
	public Text scoreText;

	public GameObject gameOverUI;


	public bool isRandomSize=true;

	public bool isRandomColor=true;

	public float showingLevelTime=1f;

	public float sizeMin=.5f;
	public float sizeMax=2f;

	private int _objectNum;


	private List<SimpleTransform> objectTransforms = new List<SimpleTransform> ();





	void Awake(){

		if (instance == null) {

			instance = this;
		} else if (instance != null) {
			Destroy (gameObject);
		}

		if (mainCam == null) {

			mainCam = Camera.main;
		}

	}


	// Use this for initialization
	void Start () {

		_objectNum = 1;

		ShowLevelUI.SetActive (false);
	}



	private bool IsInForbiddenArea(SimpleTransform tran){

		for (int i = 0; i < objectTransforms.Count; i++) {


			if (Vector2.Distance (tran.Position, objectTransforms[i].Position) 
				< (tran.LocalScale.x / 2f + objectTransforms[i].LocalScale.x / 2f)) {


				return true;
			}


		}

		return false;


	}



	public void SetForbiddenArea(SimpleTransform tran){



		objectTransforms.Add (tran);

	}



	public SimpleTransform GetUniqueRandomTransform(){


		SimpleTransform tran = GetRandomTransform ();

		while (IsInForbiddenArea (tran)) {

			tran = GetRandomTransform ();

		}

		return tran;


	}

	private SimpleTransform GetRandomTransform(){

		SimpleTransform tran=new SimpleTransform();

		//scale
		float size= Random.Range (sizeMin, sizeMax);
		Vector2 scale = new Vector2 (size, size);

		//rotation
		float zRotation=Random.Range(-180f,180f);
		Quaternion rotation = Quaternion.AngleAxis (zRotation, Vector3.back);



		//position
		float x=Random.Range(-mainCam.aspect*mainCam.orthographicSize+scale.x/2,
			mainCam.aspect*mainCam.orthographicSize-scale.x/2);

		float y=Random.Range(-mainCam.orthographicSize +scale.y/2,
			mainCam.orthographicSize -scale.y/2);

		Vector2 position = new Vector2 (x, y);


		tran.Position = position;
		tran.Rotation = rotation;
		tran.LocalScale = scale;


		return tran;
	}




	public Color GetRandomColor(){


		return Random.ColorHSV ();
	}

	public void ShowRound(){
		_objectNum++;

		//show score aka the level/ circle num with animation
		ShowLevelUI.SetActive(true);
		scoreText.text = "Round " + _objectNum;

		Invoke ("HideRound", showingLevelTime);


	}

	private void HideRound(){
		ShowLevelUI.SetActive (false);

	}

	public void GameOver(){

		gameOverUI.SetActive (true);

		Time.timeScale = 0f;

		//show lost
		// show score and save it to the prefab
		//and show exit restart ui
		// ui with fade animation
		//restart with fadeanimation



	}


	//for buttons
	public void RestartGame(){

		SceneManager.LoadScene (Application.loadedLevel,LoadSceneMode.Single);

		Time.timeScale = 1f;
	}

	public void ExitGame(){

		Application.Quit ();


		#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying=false;


		#endif


	}
}
