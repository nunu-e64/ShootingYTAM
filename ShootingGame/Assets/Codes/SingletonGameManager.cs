using UnityEngine;
using System.Collections;
using System.IO;
using System.Text;

public class SingletonGameManager : MonoBehaviour {

    static bool isExist = false;

    public string Url { get; set; }
    public bool HasLogedIn { get; set; }

    //単一性の担保
    void Awake() {
        if (isExist) {
            Destroy(gameObject);
        } else {
            isExist = true;
            DontDestroyOnLoad(gameObject);
        }
    }


	// Use this for initialization
	void Start () {

        //URLの読込み/////////////////////////////////////////////////////////////////////
        FileInfo fi = new FileInfo(Application.dataPath + "/" + "URL.txt"); //記載URLに誤りがあれば404エラー

        if (fi.Exists) {
            using (StreamReader sr = new StreamReader(fi.OpenRead(), Encoding.UTF8)) {
                Url = sr.ReadToEnd();
            }
            Debug.Log(Url);
        } else {
            Debug.LogError("FileOpenError:" + Application.dataPath + "/" + "URL.txt");
        }
        //////////////////////////////////////////////////////////////////////////////////

        HasLogedIn = false;
    }

    // Update is called once per frame
    void Update () {
	
	}
}
