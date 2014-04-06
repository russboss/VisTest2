using UnityEngine;
using System.Collections;

// put 'bg1' and 'bg2' to Resources folder

public class DynamicGUIStyle : MonoBehaviour
{
	public Rect rect;
	public string label;
	Texture2D _img;
	Texture2D _img2;
	GUIStyle style;
	void Start()
	{
		_img = (Texture2D)Resources.Load("bg1");
		_img2 = (Texture2D)Resources.Load("bg2");

		style = new GUIStyle();

		style.font = (Font)Resources.Load("Fonts/Arial");

		style.active.background = _img2; // not working
		style.hover.background = _img2; // not working
		style.normal.background = _img; // not working

		style.active.textColor = Color.red; // not working
		style.hover.textColor = Color.blue; // not working
		style.normal.textColor = Color.white;

		int border = 30;

		style.border.left = border; // not working, since backgrounds aren't showing
		style.border.right = border; // ---
		style.border.top = border; // ---
		style.border.bottom = border; // ---

		style.stretchWidth = true; // ---
		style.stretchHeight = true; // not working, since backgrounds aren't showing

		style.alignment = TextAnchor.MiddleCenter;
	}
	void OnGUI()
	{
		GUI.Button(rect, label, style);
	}
}