<?php
define("DEBUG", false);

$DB_HOST = "localhost";
$DB_USER = "ytam";
$DB_PASS = mb_convert_encoding("ｹｯｴｳｵ｡ｭ｡", "SJIS");   //ソースの文字コードUTF-8では、半角カナの前に"EFBD"をつけて目印としているため復号に失敗する。SJISにエンコードすることで余計なバイトを除去する
$DB_NAME = "fullcharge";


$TABLE = array("SCORE" => "score",
    "ACCOUNT" => "account");
$TABLE2 = array("TEST" => "test",
    "ACCOUNT" => "account",
    "WORD" => "question_word",
    "SCORE" => "score_log",
    "ANSWER_LOG" => "answer_log");


    //終了時に自動で接続は切られる
    //明示的に切りたい時→Disconnect($pdo) or $pdo = null;



//連想配列の一覧出力////////////////////////////////////////////////////////
function printArray($array, $array_name = NULL){
    print ("array " . (($array_name!==NULL)? $array_name : "") . "(\n");
    foreach ($array as $k => $v) {
    	if (is_array($v)){
            printArray($v, $k);
        } else {
            print "  " . $k . " => ". $v . "\n";
        }
    }
    print (")\n");
}

////////////////////////////////////////////////////////////////////////////

//暗号化////////////////////////////////////////////////////////////////////
function myCrypt($pass, $key){

    $result = "";
	for ($i = 0; $i < mb_strlen($pass); $i++) {
    	$tmp = (ord($pass[$i]) + $key) % 256;
        $result .= chr($tmp);
    }

    return $result;

}
////////////////////////////////////////////////////////////////////////////


//復号//////////////////////////////////////////////////////////////////
function myDecrypt($pass, $key){

    $result = "";
	for ($i = 0; $i < mb_strlen($pass); $i++) {
    	$tmp = (ord($pass[$i]) + 256 - $key) % 256;
        $result .= chr($tmp);
    }

    return $result;

}
////////////////////////////////////////////////////////////////////////////

//MySQL接続と初期化/////////////////////////////////////////////////////////
function Connect(&$error){
    global $DB_HOST, $DB_USER, $DB_PASS, $DB_NAME;
    $pdo = null;

    //不要？
    //mb_language("uni");
    //mb_internal_encoding("utf-8"); //内部文字コードを変更
    //mb_http_input("auto");
    //mb_http_output("utf-8");

    set_exception_handler('exceptionHandler'); //デフォルト例外ハンドラのセット

    try {
        $pdo = new PDO(
            'mysql:host=' . $DB_HOST . ';dbname=' . $DB_NAME . ';charset=utf8',
            $DB_USER,
            myDecrypt($DB_PASS, 64),
            array(
                PDO::ATTR_ERRMODE => PDO::ERRMODE_EXCEPTION,
                PDO::ATTR_DEFAULT_FETCH_MODE => PDO::FETCH_ASSOC
                )
            );
        //$pdo->setAttribute(PDO::ATTR_EMULATE_PREPARES, true);    //動的プレースホルダ指定。無効SQL文に対してはexequteの段階でエラーを出す。PHP5.2以降では自動設定されている。通信回数を1回に減らしてパフォーマンス向上（参考http://qiita.com/mpyw/items/b00b72c5c95aac573b71）
        //$pdo->setAttribute(PDO::ATTR_EMULATE_PREPARES, false);  // 静的プレースホルダ指定。通信2回。無効SQL文に対してはprepareの段階でエラーを出す。セキュリティ的にはこちらが優秀。
    }
    catch (PDOException $e) {
        //exit("ERROR: CONNECT_ERROR\n\t->".__FILE__." (".__LINE__.")\n\t->".$e->getMessage());
        array_push($error, "エラー: データベース接続に失敗しました (サーバーのMySQLが起動しているか確認してください)");
    }

    return $pdo;
}
////////////////////////////////////////////////////////////////////////////////

//MySQL切断/////////////////////////////////////////////////////////////////////
function DisConnect($pdo){

    $pdo = null;
    return $pdo;
}
////////////////////////////////////////////////////////////////////////////////

//例外ハンドラ関数//////////////////////////////////////////////////////////////
function exceptionHandler($exception) {
    if (DEBUG) echo "UNCAUGHT EXCEPTION:\n\t->", $exception->getMessage(), "\n";
    //array_push($error, "UNCAUGHT EXCEPTION:\n\t->" . $exception->getMessage());
}
////////////////////////////////////////////////////////////////////////////////





//test: バインドの仕方2通り////////////////////
/*
//前者は数値が文字列リテラルとして''で囲まれる。後者は型を明示指定することで数値リテラルとして扱われる
//なお、プレースホルダ「?」はPDOなら名前つきでもOK
$sth = $dbh->prepare("select * from test WHERE a=? and c=?");
$sth->execute(array('abc', 1));

$sth = $dbh->prepare("select * from test WHERE a=? and c=?");
$a = 'abc';
$c = 1;
$sth->bindParam(1, $a, PDO::PARAM_STR);
$sth->bindParam(2, $c, PDO::PARAM_INT);     //指定した型と実際の型が一致しない場合、全て文字列としてバインドされる。(エミュレータ有効の場合)
$sth->execute();  //実行後にバインドした変数が文字列型にされる仕様あり（エミュレータ有効の場合）
 */
///////////////////////////////////////////////



//test: SELECT/////////////////////////////////
function TestSelect($pdo){
    global $TABLE;

    if (!($stmt = $pdo->query("SELECT * FROM " . $TABLE["TEST"]))) {        //コンストラクタに例外をスローするよう設定しているため、ここの自作エラーは通らない。
	    exit("ERROR->PDO::QUERY\n\t->".__FILE__." (".__LINE__.")\n");
        //array_push($error, "ERROR->PDO::QUERY\n\t->".__FILE__." (".__LINE__.")\n");
    }

    //$rows = $stmt->fetchAll(PDO::FETCH_NUM);

    while($row = $stmt -> fetch(PDO::FETCH_ASSOC)) {    //すべての取得が終わるとfalseをかえす  //連想配列として取得
        printArray($row);
    }
}
///////////////////////////////////////////////


//test: insert/////////////////////////////////
function TestInsert($pdo){
    global $TABLE;

    $name = "Mary"; //×
    $value = 111;   //○

    $stmt = $pdo -> prepare("INSERT INTO " . $TABLE["TEST"] . " (name, value) VALUES (:name, :value)");
    $stmt->bindParam(':name' , $name , PDO::PARAM_STR);     //パラメータをバインド（Excecuteの際に中の値を評価）
    $stmt->bindValue(':value', $value, PDO::PARAM_INT);     //値をバインド（バインドの際に中の値を評価＝上書き無効）

    $name = "Mike"; //○
    $value = 222;   //×

    $stmt->execute();
}
///////////////////////////////////////////////


//test: update/////////////////////////////////
function  TestUpdate($pdo)
{
    global $TABLE;

    $name = "Jack"; //×
    $value = 1;     //○

    $sql = 'UPDATE '. $TABLE["TEST"] . ' SET value = :value WHERE name = :name';
    $stmt = $pdo -> prepare($sql);
    $stmt->bindValue(':value', $value, PDO::PARAM_INT);
    $stmt->bindParam(':name' , $name , PDO::PARAM_STR);

    $name = "Tom"; //○
    $value = 2;    //×

    $stmt->execute();
}
//////////////////////////////////////////////



//test: DELETE////////////////////////////////
function TestDelete($pdo)
{
    global $TABLE;

    $sql = 'DELETE FROM ' . $TABLE["TEST"] . ' WHERE name = :delete_name';
    $stmt = $pdo -> prepare($sql);
    $stmt -> bindParam(':delete_name', $name, PDO::PARAM_STR);

    $name = "Mike";

    $stmt -> execute();
}
//////////////////////////////////////////////
