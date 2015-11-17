using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using MiniJSON;
using System.IO;
using System.Text;

/// <summary>
/// ランキング表示
/// ・jsonデータをパースしてリストに格納し、uGUIで表示
/// </summary>
public class RankingManager : MonoBehaviour {

	public GameObject rankingContainer;
	public RectTransform guiRankingNode;

	public Text message;

	private List<RankingDataNode> rankingDataList = new List<RankingDataNode>();

	void Start () {
//		TextAsset rankingDataText = Resources.Load ("rankingData") as TextAsset;		//DEBUG: あらかじめ作っておいたソート済みjsonファイルをローカルから読み込み
		message.gameObject.SetActive (false);
		StartCoroutine (DownLoad ());
	}

    IEnumerator DownLoad () {
        //message.gameObject.SetActive (true);
        //message.text = "   サーバー接続中...";
        //message.color = new Color (180f / 255, 170f / 255, 0f / 255);

        string url;
        
        //URLの読込み/////////////////////////////////////////////////////////////////////
        FileInfo fi = new FileInfo(Application.dataPath + "/" + "URL.txt"); //記載URLに誤りがあれば404エラー	//TODO: 外部テキストの書き換えによるインジェクションが容易に可能
        if (fi.Exists) {
            using (StreamReader sr = new StreamReader(fi.OpenRead(), Encoding.UTF8)) {
                url = sr.ReadToEnd();
            }
        } else {
            Debug.LogError("FileOpenError:" + Application.dataPath + "/" + "URL.txt");
            yield break;
        }
        //////////////////////////////////////////////////////////////////////////////////

        WWWForm wwwForm = new WWWForm();
        wwwForm.AddField("keyword", "GetRanking");
		WWW www = new WWW (url, wwwForm);

		yield return www;

		message.gameObject.SetActive (true);
		message.color = new Color (230f / 255, 70f / 255, 70f / 255);

		if (www.error != null) {
			Debug.LogWarning ("WWWERROR: " + www.error);
			message.text = "通信エラー\n" + www.error;
			yield break;

		} else if (!www.isDone) {
			Debug.LogWarning ("WWWERROR: " + "UNDONE");
			message.text = "通信エラー";
			yield break;

		} else {
			message.gameObject.SetActive (false);
			Debug.Log (www.text);
		}

		//外部データ（ソート済みjsonファイル）からランキングデータList作成
		if (!SetRankingData (www.text)) {
			Debug.LogWarning ("LoadError->RankingData");
			message.text = "システムエラー";
			message.gameObject.SetActive (true);
			yield break;
		}

		//Listに基づいてScrollViewにインスタンスを追加して画面表示
		foreach (RankingDataNode node in rankingDataList) {
			var item = GameObject.Instantiate (guiRankingNode) as RectTransform;
			item.SetParent (rankingContainer.transform, false);

			var texts = item.GetComponentsInChildren<Text> ();
			texts[0].text = node.Rank.ToString ();
			texts[1].text = node.Name;
			texts[2].text = node.Score.ToString();
		}
	}

	//JsonデータをパースしList作成/////////////////////////////////////////
	public bool SetRankingData (string dataText) {

		Debug.Log (dataText);	//DEBUG: wwwの内容一覧表示
		var json = Json.Deserialize (dataText);

		List<object> list;
		if (json is List<object>) {
			list = json as List<object>;
		} else {
			return false;
		}

		var dic = new Dictionary<string, object>();
		int i = 0;
		foreach (object node in list) {
			if (node is Dictionary<string, object>) {
				dic = node as Dictionary<string, object>;
			} else {
				return false;
			}
			++i;

			var scoreDic = dic["Score"] as Dictionary<string, object>;
			var userDic = dic["User"] as Dictionary<string, object>;

			RankingDataNode item = new RankingDataNode (-1, "", -1);
			item.Score = int.Parse(scoreDic["score"] as string);
			item.Name = userDic["name"] as string;
			item.Rank = i;
			rankingDataList.Add (item);
		}
		return true;
	}
	/////////////////////////////////////////////////////////////////////////

	//ランキングデータ要素クラス
	private class RankingDataNode {
		public RankingDataNode (int rank, string name, int score) {
			Rank = rank;
			Score = score;
			Name = name;
		}

		private int rank;
		private int score;
		private string name;
		
		public int Rank {
			get { return rank; }
			set { rank = value; }
		}
		public int Score {
			get { return score; }
			set { score = value; }
		}
		public string Name {
			get { return name; }
			set { name = value; }
		}
	}

	//タイトルに戻る	//ボタンから呼び出し
	public void BackToTitle () {
		Application.LoadLevel ("Main");
	}
}
