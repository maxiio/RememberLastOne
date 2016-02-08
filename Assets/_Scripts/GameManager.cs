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

	private int _circleNum;


	private List<PositionAndScale> circlePNSes = new List<PositionAndScale> ();





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

		_circleNum = 1;

		ShowLevelUI.SetActive (false);
	}



	private bool IsInForbiddenArea(PositionAndScale pns){

		for (int i = 0; i < circlePNSes.Count; i++) {


			if (Vector2.Distance (pns.Position, circlePNSes[i].Position) 
				< (pns.Scale.x / 2f + circlePNSes[i].Scale.x / 2f)) {


				return true;
			}


		}

		return false;


	}





	public void SetForbiddenArea(PositionAndScale pas){



		circlePNSes.Add (pas);

	}



	public PositionAndScale GetOriginalRandomPositionAndScale(){


		PositionAndScale pns = GetRandomPositionAndScale ();

		while (IsInForbiddenArea (pns)) {

			pns = GetRandomPositionAndScale ();

		}

		return pns;


	}

	private PositionAndScale GetRandomPositionAndScale(){

		PositionAndScale pns = new PositionAndScale ();

		float s= Random.Range (sizeMin, sizeMax);
		Vector2 scale = new Vector2 (s, s);



		float x=Random.Range(-mainCam.aspect*mainCam.orthographicSize+scale.x/2,
			mainCam.aspect*mainCam.orthographicSize-scale.x/2);

		float y=Random.Range(-mainCam.orthographicSize +scale.y/2,
			mainCam.orthographicSize -scale.y/2);


		Vector2 position = new Vector2 (x, y);


		pns.Position = position;
		pns.Scale = scale;

		return pns;
	}




	public Color GetRandomColor(){


		return Random.ColorHSV ();
	}

	public void ShowRound(){
		_circleNum++;

		//show score aka the level/ circle num with animation
		ShowLevelUI.SetActive(true);
		scoreText.text = "Round " + _circleNum;

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
