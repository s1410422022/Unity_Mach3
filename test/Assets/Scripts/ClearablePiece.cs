using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearablePiece : MonoBehaviour {

	public AnimationClip clearAnimation;

	//......................................是否被消除
	private bool isBeingCleared = false;
	public bool IsBeingCleared{
		get{ return isBeingCleared;}
	}

	//......................................取得腳本
	protected GamePiece piece;

	void Awake(){
		piece = GetComponent<GamePiece>();
	}
		
	void Start(){ 
	}


	void Update () {
	} 

	//......................................清除
	public void Clear()
	{
		isBeingCleared = true;
		StartCoroutine (ClearCoroutine());
	}


	//......................................清除的過程,清除完銷毀
	private IEnumerator ClearCoroutine()
	{	
		Animator animator = GetComponent<Animator> ();
		if(animator){
			animator.Play (clearAnimation.name);
			yield return new WaitForSeconds (clearAnimation.length);
			Destroy (gameObject);
		}

	}


}
