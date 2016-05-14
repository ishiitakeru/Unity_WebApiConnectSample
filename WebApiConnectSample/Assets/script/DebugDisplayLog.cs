using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/**
 * フレームレート表示のためのクラス
 * 
 * 下記URLに記載してあったもの。
 * http://d.hatena.ne.jp/komiyak/20141216/1418760578
 */
public class DebugDisplayLog : MonoBehaviour
{
	//表示ログのリスト
	static public List<string> displayLog = new List<string>();

	// FPS
	private int fps;		//可変FPS表示値
	private int fpsCounter; //可変FPSカウンター

	// Fixed FPS
	private int fixedFps;		//固定FPS表示値
	private int fixedFpsCounter; //固定FPSカウンター

	// Debug Log
	private string debugString;

	// Timer 1秒を測る
	private float oneSecondTime;

	//////////////////////////////
	//メソッド
	//////////////////////////////

	//////////////////////////////
	/**
	 * スタート
	 */
	private void Start()
	{
		this.oneSecondTime = 0f;
	}//Start


	//////////////////////////////
	/**
	 * アップデート
	 */
	private void Update()
	{
		// FPSを計測
		if (this.oneSecondTime >= 1f){
			//1秒経った

			//FPSと固定FPSを取り出し
			this.fps = this.fpsCounter;
			this.fixedFps = this.fixedFpsCounter;

			// FPSカウンターの値をリセット
			this.fpsCounter = 0;
			this.fixedFpsCounter = 0;
			this.oneSecondTime = 0f;

		}else{
			//1秒経ってない間
			//FPSカウンターと固定FPSカウンターをカウントアップ
			this.fpsCounter++;
			this.oneSecondTime += Time.deltaTime;
		}//if

		// structure debug string
		this.debugString = "";
		int count = DebugDisplayLog.displayLog.Count;

		for (int i = 0; i < DebugDisplayLog.displayLog.Count; i++){
			this.debugString += DebugDisplayLog.displayLog[i];
			this.debugString += "\n";
		}//for

		DebugDisplayLog.displayLog.Clear();
	}//Update


	//////////////////////////////
	/**
	 * 固定フレームアップデート
	 */
	private void FixedUpdate()
	{
		//固定FPSカウンターをカウントアップ
		this.fixedFpsCounter++;
	}//FixedUpdate


	//////////////////////////////
	/**
	 * GUI表示
	 * 作成したFPS報告文字列をGUIに表示
	 */
	private void OnGUI(){
		GUI.Label(
			new Rect(0f, 0f, Screen.width, Screen.height),
			"FPS: " + this.fps + "  FixedUpdate: " + this.fixedFps + "\n" + this.debugString
		);
	}//OnGUI

}//class
