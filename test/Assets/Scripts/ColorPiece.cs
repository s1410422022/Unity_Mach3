using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//上次做到顏色4分鐘
public class ColorPiece : MonoBehaviour {
	public enum ColorType
	{
		YELLOW,
		PURPLE,
		RED,
		BLUE,
		GREEN,
		PINK,
		ANY,
		COUNT
	};


	[System.Serializable]
	public struct ColorSprite
	{
		public ColorType color;
		public Sprite sprite;
	}
		

	public ColorSprite[] colorSprites;
	//..............................................
	private ColorType color;
	public ColorType Color
	{
		get{ return color;}
		set{ SetColor (value);}
	}
	//..............................................
	private SpriteRenderer sprite;
	//..............................................
	private Dictionary<ColorType,Sprite> colorSpriteDict;


	void Awake()
	{	
		sprite = transform.Find ("piece").GetComponent<SpriteRenderer> ();

		colorSpriteDict = new Dictionary<ColorType,Sprite> ();
		for(int i=0; i < colorSprites.Length; i++){
			if(!colorSpriteDict.ContainsKey(colorSprites [i].color))//是否colorSpriteDict<ColorType,Sprite>裡有color的值>>此行為了檢查作用
			{
				colorSpriteDict.Add(colorSprites [i].color,colorSprites [i].sprite);
			}
		}
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SetColor(ColorType newColor){
		color = newColor;

		if (colorSpriteDict.ContainsKey (newColor)) {
			sprite.sprite = colorSpriteDict [newColor];
		}
	}
}
