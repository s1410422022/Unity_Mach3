using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections.Generic;

public class Grid : MonoBehaviour {

    public enum PieceType {
        EMPTY,
		NORMAL,
		BUBLE,	//!!!!!!!!!!!!!!!!!!障礙物新增
        COUNT,
    };



    [System.Serializable]
    public struct PiecePrefab {
        public PieceType type;
        public GameObject prefab;
    };

    public int xDim;
    public int yDim;
	public float fillTime;

    public PiecePrefab[] piecePrefabs;
    public GameObject backgroundPrefab;

    private Dictionary<PieceType, GameObject> piecePrefabDict;


    private GamePiece[,] pieces;//--06
	private bool inverse =false;//!!!!!!!!!!障礙物填充新增

	private GamePiece pressedPiece;
	private GamePiece enteredPiece;



    void Start () {
        piecePrefabDict = new Dictionary<PieceType, GameObject> ();

        for (int i=0; i < piecePrefabs.Length; i++) {
            if (!piecePrefabDict.ContainsKey(piecePrefabs[i].type)) {
                piecePrefabDict.Add(piecePrefabs[i].type, piecePrefabs[i].prefab);
            }
        }
        for (int x = 0; x < xDim; x++) {
            for (int y = 0; y < yDim; y++)
            {
                GameObject background = (GameObject)Instantiate(backgroundPrefab, GetWorldPosition(x, y), Quaternion.identity);//--
                background.transform.parent = transform;
            }
        }

        pieces = new GamePiece[xDim, yDim];
        for (int x = 0; x < xDim; x++)
        {
            for (int y = 0; y < yDim; y++)
            {
				SpawNewPiece (x, y, PieceType.EMPTY);
            }
        }
		//.............................
		Destroy (pieces[1,4].gameObject);//!!!!!!!!!!!!!這是放障礙物的地方改變即可不同位置，障礙物新增
		SpawNewPiece(1,4,PieceType.BUBLE);//!!!!!!!!!!!!!障礙物新增

		Destroy (pieces[2,4].gameObject);//!!!!!!!!!!!!!這是放障礙物的地方改變即可不同位置，障礙物新增
		SpawNewPiece(2,4,PieceType.BUBLE);//!!!!!!!!!!!!!障礙物新增

		Destroy (pieces[3,4].gameObject);//!!!!!!!!!!!!!這是放障礙物的地方改變即可不同位置，障礙物新增
		SpawNewPiece(3,4,PieceType.BUBLE);//!!!!!!!!!!!!!障礙物新增

		Destroy (pieces[5,4].gameObject);//!!!!!!!!!!!!!這是放障礙物的地方改變即可不同位置，障礙物新增
		SpawNewPiece(5,4,PieceType.BUBLE);//!!!!!!!!!!!!!障礙物新增

		Destroy (pieces[6,4].gameObject);//!!!!!!!!!!!!!這是放障礙物的地方改變即可不同位置，障礙物新增
		SpawNewPiece(6,4,PieceType.BUBLE);//!!!!!!!!!!!!!障礙物新增

		Destroy (pieces[7,4].gameObject);//!!!!!!!!!!!!!這是放障礙物的地方改變即可不同位置，障礙物新增
		SpawNewPiece(7,4,PieceType.BUBLE);//!!!!!!!!!!!!!障礙物新增

		Destroy (pieces[4,0].gameObject);//!!!!!!!!!!!!!這是放障礙物的地方改變即可不同位置，障礙物新增
		SpawNewPiece(4,0,PieceType.BUBLE);//!!!!!!!!!!!!!障礙物新增
		//.............................

		StartCoroutine(Fill());
    }


	void Update () { 
	
	} 
	//.......................................................填充滿所有網格功能
	public IEnumerator Fill()
	{
		bool needsRefill = true;
		while (needsRefill) {
		 yield return new WaitForSeconds (fillTime);
			while (FillStep ()) {
				inverse = !inverse;//!!!!!!!!!!障礙物填充新增
				yield return new WaitForSeconds (fillTime);
			}
			needsRefill = ClearAllValidMatches ();
		}
	}

	//.......................................................一步步移動填滿功能
	public bool FillStep()
	{
		bool movedPiece = false;
		for(int y = yDim-2; y >= 0; y--)//檢查下一行是否有，空白有就下來
		{

			for(int loopX=0; loopX< xDim; loopX++)//!!!!!!!!!!障礙物填充更改 x>>loopX
			{
				int x = loopX;					//!!!!!!!!!!障礙物填充新增 
				if (inverse) 					//!!!!!!!!!!障礙物填充新增 
				{ 								//!!!!!!!!!!障礙物填充新增 
					x = xDim - 1 - loopX;		//!!!!!!!!!!障礙物填充新增 
				}								//!!!!!!!!!!障礙物填充新增 

				GamePiece piece = pieces [x, y];
				if (piece.IsMovable ()) 
				{
					GamePiece pieceBelow = pieces [x, y + 1];
					if (pieceBelow.Type == PieceType.EMPTY) {
						Destroy (pieceBelow.gameObject);
						piece.MovableComponent.Move (x, y + 1, fillTime);
						pieces [x, y + 1] = piece;
						SpawNewPiece (x, y, PieceType.EMPTY);
						movedPiece = true;
					} 
					else //!!!!!!!!!!else裡面全是障礙物填充新增 
					{
						for(int diag =-1; diag<=1; diag++)
						{
							if (diag != 0) 
							{
								int diagX = x + diag;
								if (inverse) 
								{
									diagX = x - diag;
								}
								if (diagX >= 0 && diagX < xDim) 
								{
									GamePiece diagonalPiece = pieces [diagX, y + 1];
									if(diagonalPiece.Type == PieceType.EMPTY)
									{
										bool hasPieceAbove = true;
										for(int aboveY = y; aboveY >= 0; aboveY--)
										{
											GamePiece pieceAbove = pieces [diagX, aboveY];
											if (pieceAbove.IsMovable ()) 
											{
												break;
											}
											else if(!pieceAbove.IsMovable() && pieceAbove.Type != PieceType.EMPTY)
											{
												hasPieceAbove = false;
												break;
											}
										}
										if (!hasPieceAbove)
										{
											Destroy (diagonalPiece.gameObject);
											piece.MovableComponent.Move (diagX, y + 1, fillTime);
											pieces [diagX, y + 1] = piece;
											SpawNewPiece (x, y, PieceType.EMPTY);
											movedPiece = true;
											break;
										}
									}
								}
							}
						}
					}
				}
			}	
		}
		for(int x=0;x<xDim;x++)//判斷第一行
		{
			GamePiece pieceBelow = pieces [x, 0];
			if(pieceBelow.Type == PieceType.EMPTY)
			{
				Destroy (pieceBelow.gameObject);
				GameObject newPiece =
					(GameObject)Instantiate (piecePrefabDict [PieceType.NORMAL], GetWorldPosition (x, -1), Quaternion.identity);
				newPiece.transform.parent = transform;
				pieces [x, 0] = newPiece.GetComponent<GamePiece> ();
				pieces [x, 0].Init (x, -1, this, PieceType.NORMAL);
				pieces [x, 0].MovableComponent.Move (x, 0,fillTime);
				pieces [x, 0].ColorComponent.SetColor ((ColorPiece.ColorType)Random.Range (0, pieces [x, 0].ColorComponent.NumColors));
				movedPiece = true;
			}	

		}

		return movedPiece;
	}

	//.......................................................取得世界座標，讓網格置中功能
    public Vector2 GetWorldPosition(int x, int y) {
        return new Vector2(transform.position.x - xDim / 2.0f + x,
            transform.position.y + yDim / 2.0f - y);

    }

	//.......................................................一開始清空功能
	public GamePiece SpawNewPiece(int x,int y ,PieceType type) 
	{
		GameObject newPiece = (GameObject)Instantiate (piecePrefabDict [type], GetWorldPosition (x, y), Quaternion.identity);
		newPiece.transform.parent = transform;
		pieces [x, y] = newPiece.GetComponent<GamePiece> ();
		pieces[x,y].Init(x,y,this,type);

		return pieces [x, y];
	}
	//.......................................................判斷元素是否相鄰
	public bool IsAdjacent(GamePiece piece1,GamePiece piece2)
	{
		return(piece1.X == piece2.X && (int)Mathf.Abs (piece1.Y - piece2.Y) == 1)
		|| (piece1.Y == piece2.Y && (int)Mathf.Abs (piece1.X - piece2.X) == 1);
	}

	//.......................................................滑動互換動作功能
	public void SwapPieces(GamePiece piece1,GamePiece piece2)
	{
		if (piece1.IsMovable () && piece2.IsMovable ()) 
		{
			pieces [piece1.X, piece1.Y] = piece2;
			pieces [piece2.X, piece2.Y] = piece1;

			if (GetMatch (piece1, piece2.X, piece2.Y) != null || GetMatch (piece2, piece1.X, piece1.Y) != null) {

				int piece1X = piece1.X;
				int piece1Y = piece1.Y;

				piece1.MovableComponent.Move (piece2.X, piece2.Y, fillTime);
				piece2.MovableComponent.Move (piece1X, piece1Y, fillTime);

				ClearAllValidMatches ();
				StartCoroutine (Fill ());

			} else {
				pieces [piece1.X, piece1.Y] = piece1;
				pieces [piece2.X, piece2.Y] = piece1;

			}
		}
	}

	//.......................................................滑鼠功能
	public void PressPiece(GamePiece piece)
	{
		pressedPiece = piece;
	}
	public void EnterPiece(GamePiece piece)
	{
		enteredPiece = piece;
	}

	public void ReleasePiece()
	{
		if (IsAdjacent (pressedPiece, enteredPiece)) 
		{
			SwapPieces (pressedPiece, enteredPiece);
		}
	}


	//.......................................................直線配對功能，l型配對功能
	public List<GamePiece> GetMatch(GamePiece piece, int newX,int newY)
	{
		if (piece.IsColored ()) 
		{
			ColorPiece.ColorType color = piece.ColorComponent.Color;
			List<GamePiece> horizontalPieces = new List<GamePiece> ();
			List<GamePiece> verticalPieces = new List<GamePiece> ();
			List<GamePiece> matchingPieces = new List<GamePiece> ();

			//水平
			horizontalPieces.Add (piece);
			for (int dir = 0; dir <= 1; dir++)
			{
				for(int xOffset = 1; xOffset < xDim; xOffset++)
				{
					int x;
					if (dir == 0) {//left
						x = newX - xOffset;
					} 
					else {//right
					
						x = newX + xOffset;
					}
					if (x < 0 || x >= xDim) {
						break;
					}
					if (pieces [x, newY].IsColored () && pieces [x, newY].ColorComponent.Color == color) {
						horizontalPieces.Add (pieces [x, newY]);
					} else {
						break;
					}
				}
			}

			if (horizontalPieces.Count >= 3) 
			{
				for(int i=0; i < horizontalPieces.Count; i++){
					matchingPieces.Add (horizontalPieces [i]);
				}
			}
			//...........................................水平l型
			if(horizontalPieces.Count >= 3){
				for(int i=0 ;i<horizontalPieces.Count;i++){
					for(int dir =0; dir <= 1; dir++){
						for(int yOffset = 1; yOffset < yDim; yOffset++){
							int y;
							if (dir == 0) {//up
								y = newY - yOffset;
							} else { //down
								y=newY + yOffset;
							}
							if (y < 0 || y >= yDim) {
								break;
							}
							if (pieces [horizontalPieces [i].X, y].IsColored () && pieces [horizontalPieces [i].X, y].ColorComponent.Color == color) {
								verticalPieces.Add (pieces [horizontalPieces [i].X, y]);
							} else {
								break;
							}
						}
					}
					if (verticalPieces.Count < 2) {
						verticalPieces.Clear ();
					} else {
						for(int j =0; j <verticalPieces.Count; j++){
							matchingPieces.Add (verticalPieces [j]);
						}
						break;
					}
				}
			}
			//****************************
			if (matchingPieces.Count >= 3) 
			{
				return matchingPieces;
			}




			//垂直
			horizontalPieces.Clear();
			verticalPieces.Clear ();
			verticalPieces.Add (piece);
			for (int dir = 0; dir <= 1; dir++)
			{
				for(int yOffset = 1; yOffset < yDim; yOffset++)
				{
					int y;
					if (dir == 0) {	//up
						y = newY - yOffset;
					} 
					else {	//down

						y = newY + yOffset;
					}
					if (y < 0 || y >= yDim) {
						break;
					}
					if (pieces [newX, y].IsColored () && pieces [newX, y].ColorComponent.Color == color) {
						verticalPieces.Add (pieces [newX, y]);
					} else {
						break;
					}
				}
			}

			if (verticalPieces.Count >= 3) 
			{
				for(int i=0; i<verticalPieces.Count; i++){
					matchingPieces.Add (verticalPieces [i]);
				}
			}

			//...........................................垂直l型
			if(verticalPieces.Count >= 3){
				for(int i=0 ;i<verticalPieces.Count;i++){
					for(int dir =0; dir <= 1; dir++){
						for(int xOffset = 1; xOffset < xDim; xOffset++){
							int x;
							if (dir == 0) {//left
								x = newX - xOffset;
							} else { //right
								x=newX + xOffset;
							}
							if (x < 0 || x >= xDim) {
								break;
							}
							if (pieces [x,verticalPieces [i].Y].IsColored () && pieces [x,verticalPieces [i].Y].ColorComponent.Color == color) {
								horizontalPieces.Add (pieces [x,verticalPieces [i].Y]);         ////這個是自己改的，如果之後執行有誤改他式式，原本是 verticalPieces.Add (pieces [x,verticalPieces [i].Y]); 
							} else {
								break;
							}
						}
					}
					if (horizontalPieces.Count < 2) {
						horizontalPieces.Clear (); 
						for(int j =0; j <horizontalPieces.Count; j++){
							matchingPieces.Add (horizontalPieces [j]);
						}
						break;
					}
				}
			}
			//************************

			if (matchingPieces.Count >= 3) 
			{
				return matchingPieces;
			}
				
		}

		return null;

	}

	//.......................................................清除後配對
	public bool ClearAllValidMatches()
	{
		bool needsRefill = false;
		for(int y = 0; y < yDim; y++){
			for(int x = 0; x < xDim; x++){
				if(pieces[x,y].IsClearable()){
					List<GamePiece> match = GetMatch (pieces [x, y], x, y);
					if(match != null){
						for(int i = 0; i < match.Count; i++){
							if (ClearPiece (match [i].X, match [i].Y)) {
								needsRefill = true;
							}
						}
					}
				}
			}
		}
		return needsRefill;
	}


	//.......................................................清除
	public bool ClearPiece(int x ,int y)
	{
		if (pieces [x, y].IsClearable () && !pieces [x, y].ClearableComponent.IsBeingCleared) 
		{
			pieces [x, y].ClearableComponent.Clear ();
			SpawNewPiece (x, y, PieceType.EMPTY);

			ClearObstacles(x, y);		//!!!!!!!!!!障礙物清除新增 

			return true;
		}
		return false;
	}

	//.......................................................清除障礙物 //!!!!!!!!!!障礙物清除新增 
	public void ClearObstacles(int x,int y)
	{
		for(int adjacentX = x-1; adjacentX <= x+1; adjacentX++ ){
			if(adjacentX != x && adjacentX >= 0 && adjacentX < xDim)
			{
				if(pieces[adjacentX,y].Type == PieceType.BUBLE && pieces [adjacentX,y].IsClearable())
				{
					pieces [adjacentX, y].ClearableComponent.Clear ();
					SpawNewPiece (adjacentX, y, PieceType.EMPTY);
				}
			}
		}
		for(int adjacentY=y-1; adjacentY<=y+1;adjacentY++)
		{
			if (adjacentY != y && adjacentY >= 0 && adjacentY < yDim) {
				if(pieces[x,adjacentY].Type == PieceType.BUBLE && pieces[x,adjacentY].IsClearable())
				{
					pieces [x, adjacentY].ClearableComponent.Clear ();
					SpawNewPiece (x, adjacentY, PieceType.EMPTY);
				}
			}
		}
	}


}
    