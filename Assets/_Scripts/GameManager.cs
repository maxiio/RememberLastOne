using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour {
	public static GameManager Instance=null;


	public Camera MainCam;

	public GameObject ShowLevelUI;
	public Text ScoreText;
	public Text HighScoreText;
	public Text ThisTimeScoreText;

	public GameObject GameOverUI;

	public CameraFade TheCameraFade;


	public bool IsRandomSize=true;

	public bool IsRandomColor=true;

	public float ShowingLevelTime=1f;

	public float SizeMin=.5f;
	public float SizeMax=2f;

	private int _round; 
	private int _highScore;
	private string _highScoreKey= "HighScore";


	private List<SimpleTransform> _objectTransforms = new List<SimpleTransform> ();





	void Awake(){

		if (Instance == null) {

			Instance = this;
		} else if (Instance != null) {
			Destroy (gameObject);
		}

		if (MainCam == null) {

			MainCam = Camera.main;
		}

	}


	// Use this for initialization
	void Start () {

		_round = 1;
		_highScore = PlayerPrefs.GetInt (_highScoreKey, 0);


		ShowLevelUI.SetActive (false);
		GameOverUI.SetActive (false);


	}



	private bool IsInForbiddenArea(SimpleTransform tran){

		for (int i = 0; i < _objectTransforms.Count; i++) {


			if (Vector2.Distance (tran.Position, _objectTransforms[i].Position) 
				< (tran.LocalScale.x / 2f + _objectTransforms[i].LocalScale.x / 2f)) {


				return true;
			}


		}

		return false;


	}



	public void SetForbiddenArea(SimpleTransform tran){



		_objectTransforms.Add (tran);

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
		float size= Random.Range (SizeMin, SizeMax);
		Vector2 scale = new Vector2 (size, size);

		//rotation
		float zRotation=Random.Range(-180f,180f);
		Quaternion rotation = Quaternion.AngleAxis (zRotation, Vector3.back);



		//position
		float x=Random.Range(-MainCam.aspect*MainCam.orthographicSize+scale.x/2,
			MainCam.aspect*MainCam.orthographicSize-scale.x/2);

		float y=Random.Range(-MainCam.orthographicSize +scale.y/2,
			MainCam.orthographicSize -scale.y/2);

		Vector2 position = new Vector2 (x, y);


		tran.Position = position;
		tran.Rotation = rotation;
		tran.LocalScale = scale;


		return tran;
	}




	public Color GetRandomColor(){


		return Random.ColorHSV ();
	}

	public void NextRound(){


		_round++;

		//show score aka the level/ circle num with animation


		ShowLevelUI.SetActive(true);
		ScoreText.text = "Round " + _round;


		if (_round > _highScore) {

			PlayerPrefs.SetInt (_highScoreKey, _round);

		}

		Invoke ("HideRound", ShowingLevelTime);


	}

	private void HideRound(){
		ShowLevelUI.SetActive (false);

	}

	public void GameOver(){

		TheCameraFade.FadeOut (true);

		HighScoreText.text = _highScore.ToString();
		ThisTimeScoreText.text = (_round-1).ToString();

		Invoke ("ShowGameOverUI", TheCameraFade.FadeOutDuration);
		//Time.timeScale = 0f;

		//show lost
		// show score and save it to the prefab
		//and show exit restart ui
		// ui with fade animation
		//restart with fadeanimation

	}

	public void ShowGameOverUI(){

		GameOverUI.SetActive (true);





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
