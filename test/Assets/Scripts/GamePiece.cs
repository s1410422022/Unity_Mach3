﻿using System.Collections;
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

    private MovablePiece movableComponent;

    public MovablePiece MovableComponent {
        get { return movableComponent;}
    }


    void Awake() {
        movableComponent = GetComponent<MovablePiece>();
    }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Init(int _x, int _y,Grid _grid,Grid.PieceType _type) {
        x = _x;
        y = _y;
        grid = _grid;
        type = _type;
    }

    public bool IsMovable() {
        return movableComponent != null;
    }
}
