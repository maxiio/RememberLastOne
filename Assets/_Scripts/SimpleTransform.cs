using UnityEngine;
using System.Collections;

public class SimpleTransform : MonoBehaviour {


	private Vector2 _position;
	private Vector2 _scale;
	private Quaternion _rotation;


	public Vector2 Position{

		get{return _position;}
		set{ _position = value; }


	}

	public Vector2 LocalScale{

		get{ return _scale; }
		set{ _scale = value; }


	}

	public Quaternion Rotation{

		get{ return _rotation; }
		set{ _rotation=value;}
	}

}
