using UnityEngine;
using System.Collections;

public class Crosshair : MonoBehaviour {

	public Texture2D image;
	Rect position;

	void Start ()
	{
		if (image == null)
		{
			Debug.LogWarning("Crosshair image not set");
		}

		position = new Rect((Screen.width - image.width) / 2, (Screen.height - image.height) / 2,
		                    image.width, image.height);
	}

	void OnGUI()
	{
		GUI.DrawTexture(position, image);
	}
}
