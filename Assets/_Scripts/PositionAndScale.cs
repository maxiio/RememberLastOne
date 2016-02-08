using UnityEngine;
using System.Collections;

public class PositionAndScale : MonoBehaviour {

	private Vector2 _position;
	private Vector2 _scale;


	public Vector2 Position{

		get{return _position;}
		set{ _position = value; }


	}

	public Vector2 Scale{

		get{ return _scale; }
		set{ _scale = value; }


	}


}

