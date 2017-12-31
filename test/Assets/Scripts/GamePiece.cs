using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePiece : MonoBehaviour {

    private int x;
    private int y;
    public int X {
       
		get { return x; }
        set {
            if (IsMovable()) {
                x = value;
            }
        }
    }

    public int Y
    {
        get { return y; }
        set
        {
            if (IsMovable())
            {
                y = value;
            }
        }
    }

	//.......................網格腳本
    private Grid.PieceType type;
    public Grid.PieceType Type
    {
        get { return type; }
    }
	//抓取grid裡的piecetype，看是NORMAL還是COUNT

    private Grid grid;
    public Grid GridRef
    {
        get { return grid; }
    }

	//.......................移動元件腳本
    private MovablePiece movableComponent;

    public MovablePiece MovableComponent {
        get { return movableComponent;}
    }
	//.......................顏色元件腳本
	private ColorPiece colorComponent;

	public ColorPiece ColorComponent 
	{
		get { return colorComponent;}
	}
	//.......................清除元件腳本
	private ClearablePiece clearableComponent;
	public ClearablePiece ClearableComponent{
		get{ return clearableComponent;}
	}


    void Awake() {
        movableComponent = GetComponent<MovablePiece>();
		colorComponent = GetComponent<ColorPiece>();
		clearableComponent=GetComponent<ClearablePiece>();
    }
 

	void Start(){ 
	} 

	void Update () {
		
	} 
	//.......................初始
    public void Init(int _x, int _y,Grid _grid,Grid.PieceType _type) {
        x = _x;
        y = _y;
        grid = _grid;
        type = _type;
    }

	//.......................滑鼠
	void OnMouseEnter()
	{
		grid.EnterPiece (this);
	}
	void OnMouseDown()
	{
		grid.PressPiece (this);

	}
	void OnMouseUp()
	{
		grid.ReleasePiece ();

	}

	//.......................判斷是否移動,顏色是否為空,是否清除
    public bool IsMovable() {
        return movableComponent != null;
    }
		
	public bool IsColored()
	{
		return colorComponent != null;
	}

	public bool IsClearable()
	{
		return clearableComponent != null;
	}
}
