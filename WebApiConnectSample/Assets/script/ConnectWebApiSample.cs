using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

/**
 * WebAPI接続サンプル
 */
public class ConnectWebApiSample : MonoBehaviour {
	////////////////////////////////////////////////////////////
	//フィールド、プロパティ
	////////////////////////////////////////////////////////////
	//接続先URL
	//自分のレンタルサーバー上に設置したもの。
	//POSTで「keyword」というキーに「civilwar」という値を送信してやると正しい値を返す。
	private string web_api_url = "http://loveandcomic.com/160514api_test/01.php";

	//表示テキスト
	private string text_to_show = "not yet";

	//WebAPIに接続する WWW インスタンス
	private WWW my_www = null;

	////////////////////////////////////////////////////////////
	//自動実行メソッド
	////////////////////////////////////////////////////////////

	//////////////////////////////
	/**
	 * スタート
	 */
	void Start () {

		//POST送信値のコレクション
		Dictionary<string, string> post_data_set = new Dictionary<string, string>();
		//ディクショナリに追加するときもAdd（JavaのMapだとSetだった気がするがAdd）
		post_data_set.Add("keyword", "civilwar");

		//WebAPI接続のためのコルーチンをスタート
		StartCoroutine(
			ConnectWebApiPostingDataAngGetResponse(
				this.web_api_url,
				post_data_set
			)
		);

	}//Start


	//////////////////////////////
	/**
	 * アップデート
	 */
	// Update is called once per frame
	void Update () {
	}//Update


	//////////////////////////////
	/**
	 * GUI表示
	 * WebAPIから取得した文字列をGUIに表示
	 */
	private void OnGUI(){
		GUI.Label(
			new Rect(0f, 20f, Screen.width, Screen.height), //GUIテキストを表示する四角形領域
			"WebApiResponse : " + this.text_to_show         //表示するテキスト
		);
	}//OnGUI


	////////////////////////////////////////////////////////////
	//自作メソッド
	////////////////////////////////////////////////////////////

	//////////////////////////////
	/**
	 * APIにPOSTデータを投げてレスポンスを受け取る。
	 * 
	 * @param  string 接続先URL
	 * @param  Dictionary<string, string> POSTする値のキーとバリューのセット
	 * @return IEnumerator 戻り値を使うというよりはこのメソッド内で書き換えたフィールドの値を使う。
	 */
	private IEnumerator ConnectWebApiPostingDataAngGetResponse(
		string web_api_url,
		Dictionary<string, string> post_data_set
	){
		//WebAPIにPOSTデータを送信するための WWWForm インスタンス
		WWWForm my_www_form = new WWWForm();

		//受け取ったDictionaryコレクションをループ処理
		//C# の Dictionary は forreach が使える
		foreach (string key in post_data_set.Keys) {
			//キー => 値 をセット
			my_www_form.AddField(
				key,
				post_data_set[key]
			);
		}//foreach

		//指定urlにデータを送信
		//URLに接続するときは WWW クラスを使う。
		this.my_www = new WWW(
			web_api_url, //WebAPIのURL
			my_www_form  //POST送信値をセットしたWWWFormインスタンス
		);

		//このWWWインスタンスは時間の移り変わりとともに状態が変わるので、
		//WWWインスタンス生成後に yield return のループを作ってやって状態を監視している。
		//WWWインスタンスの生成自体を繰り返さないこと。
		while(true){
			//WWWインスタンスは各種プロパティにその取得してきた内容を持つ。今回はテキスト。
			yield return null;

			//読み込みが完了したらテキストを取得して終了
			//読み込み完了前にプロパティにアクセスしようとするとエラーとなり、それ以降の処理を中断してしまうらしい。
			if(this.my_www.isDone == true){
				Debug.Log("Apiレスポンス : " + this.my_www.text);
				this.text_to_show = this.my_www.text;
				break;
			}//if
		}//while
	}//function

}//class
