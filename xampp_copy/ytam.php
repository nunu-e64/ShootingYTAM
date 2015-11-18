<?php
include "DBManager.php";

define ("UNITY", false);


//変数宣言
$keyword = "";	    //動作キーワード
$resultData = NULL;	//返すデータ
$result = array();
$error = array();
$keyword = (isset($_POST['keyword'])? $_POST['keyword'] : 'GetRanking');

//接続
$pdo = Connect($error);


//処理分岐
if ($pdo != null){
    switch ($keyword)
    {
      case 'GetRanking':
          GetRankingYTAM($result, $error);
          break;
      case 'RegisterScore':
          RegisterScoreYTAM($result, $error);
          break;

	    case 'SignUp':
            SignUp($result, $error);
            break;
        case 'LogIn':
            LogIn($result, $error);
            break;
        case 'GetWord':
            GetWordData($result, $error);
            break;
        case 'GetRank':
            GetRank($result, $error);
            break;
        case 'GetHighScore':
            GetHighScore($result, $error);
            break;
        case 'UpdatePoint':
            UpdatePoint($result, $error);
            break;
        case 'UpdateEquipment':
            UpdateEquipment($result, $error);
            break;
        case 'UpdateAnswerLog';
            UpdateAnswerLog($result, $error);
            break;
        case 'GetAnswerLog';
            GetAnswerLog($result, $error);
            break;
        default:
            array_push($error, "PHPシステムエラー->KeyWord[" . $keyword . "] didn't match any key."); //Check \"keyword\".\n\t->".__FILE__." (".__LINE__.")\n");
    }

    if (!UNITY) print("<br>------------------------------<br>");
}

//読み込んだデータをjson形式で端末に送信する
//header('Content-type: application/json');

//$json = array('result' => $result, 'error' => $error);

print json_encode($result);

if (!UNITY) {
    print ("<br><br>\n");
    printArray($error, "Error");
    printArray($result, "Result");
    print ("<br><br>\n");
    var_dump($result);
}


function GetRankingYTAM(&$result, &$error){
    //入力チェック宣言///////////
    global $pdo, $TABLE;

    //{"Score":{"score":"4900","user_id":"0"},"User":{"name":null}}

    //コースが一致するデータを抽出
    $query = "SELECT y.score, x.name, y.user_id FROM ".$TABLE["ACCOUNT"]." AS x JOIN ".$TABLE["SCORE"]." AS y WHERE y.user_id=x.id";
    //$query = $query . " WHERE y.cource=? AND (SELECT COUNT(*) FROM ".$TABLE["SCORE"]." AS z WHERE y.cource=z.cource AND y.level=z.level AND y.score<z.score) <= ?";
    $query = $query . "  ORDER BY y.score DESC";

    if (!UNITY) print($query."<br>");

    $stmt = $pdo->prepare($query);
    $stmt->execute();

    $rows = $stmt->fetchAll();  //PDOはフェッチしたすべての数値を文字列に強制変換する。糞仕様。（参考：http://d.hatena.ne.jp/erio_nk/20120621/1340267044）

    if ($rows){
        $result += $rows;

        $result2 = array();
        foreach ($result as $k=>$v){
            array_push($result2, array('Score' => array('score'=>$v['score'], 'user_id'=>$v['user_id']), 'User' => array('name'=>$v['name'])));
        }

        $result = $result2;

    } else {
        array_push($error, "MySQL_Error: Get null.");
    }

}
////////////////////////////////////////////////////////////////////////////////////////////////////////////////

//スコア登録////////////////////////////////////////////////////////////////////////////////////////////////////
function RegisterScoreYTAM(&$result, &$error){

    //入力チェック&宣言////////////
    global $pdo, $TABLE;
    $postValues = array();

    array_push($postValues, ($user_id   = (isset($_POST['user_id'])?    $_POST['user_id']   : NULL)));
    array_push($postValues, ($score  = (isset($_POST['score'])?   $_POST['score']  : NULL)));

    if (!UNITY) {
        var_export($postValues); print"\n";
    }

    foreach ($postValues as $k => $v) {
    	if ($v === NULL) {
        	array_push($error, "PHPシステムエラー: POST_ERROR: Check Posted Values[" . $k . "]");
            goto fin;
        }
    }

    //////////////////////////////

    //スコアデータを挿入
    $query = "INSERT INTO ".$TABLE["SCORE"]." (user_id, score) VALUES (?,?)";
    if (!UNITY) print $query."\n";
    $stmt = $pdo->prepare($query);
    $stmt->bindValue(1, (int)$user_id, PDO::PARAM_INT);
    $stmt->bindValue(2, (int)$score, PDO::PARAM_INT);
    $stmt->execute();

    fin:
}
////////////////////////////////////////////////////////////////////////////////////////////////////////////////




//新規登録//////////////////////////////////////////////////////////////////////////////////////////////////////
function SignUp(&$result, &$error){
    //入力チェック宣言///////////
    global $pdo, $TABLE;
    $postValues = array();

    array_push($postValues, ($user_id   = (isset($_POST['user_id'])?    $_POST['user_id']   : NULL)));
    array_push($postValues, ($password  = (isset($_POST['password'])?   $_POST['password']  : NULL)));
    array_push($postValues, ($name      = (isset($_POST['name'])?       $_POST['name']      : NULL)));
    array_push($postValues, ($age       = (isset($_POST['age'])?        $_POST['age']       : NULL)));

    if (!UNITY) {var_export($postValues); print"\n";}

    foreach ($postValues as $k => $v) {
    	if ($v === NULL) {
        	array_push($error, "PHPシステムエラー: POST_ERROR: Check Posted Values[" . $k . "]");
            goto fin;
        }
    }

    //////////////////////////////

    //user_id重複チェック
    $query = "SELECT EXISTS (SELECT 1 FROM ".$TABLE["ACCOUNT"]." WHERE user_id = ?) as isExist";
    if (!UNITY) print $query . "\n";
    $stmt = $pdo->prepare($query);
    $stmt->execute(array($user_id));

    $rows = $stmt->fetchAll(PDO::FETCH_COLUMN, 0);
    if ($rows[0]){
        array_push($error, "既にこのIDは登録されています。他のIDを入力してください。 " . $user_id);
        goto fin;
    }

    //name重複チェック
    $query = "SELECT EXISTS (SELECT 1 FROM ".$TABLE["ACCOUNT"]." WHERE name = ?) as isExist";
    if (!UNITY) print $query . "\n";
    $stmt = $pdo->prepare($query);
    $stmt->execute(array($name));

    $rows = $stmt->fetchAll(PDO::FETCH_COLUMN, 0);
    if ($rows[0]) {
        array_push($error, "既にこの名前は登録されています。他の名前を入力してください。" . $name);
        goto fin;
    }


    $query = "INSERT INTO ".$TABLE["ACCOUNT"]." (user_id, password, name, age) VALUES (?,?,?,?)";
    if (!UNITY) print $query."\n";
    $stmt = $pdo->prepare($query);
    $stmt->bindValue(1, $user_id, PDO::PARAM_STR);
    $stmt->bindValue(2, $password, PDO::PARAM_STR);
    $stmt->bindValue(3, $name, PDO::PARAM_STR);
    $stmt->bindValue(4, (int)$age, PDO::PARAM_INT);
    $stmt->execute();

    $result["id"] = $user_id;

    fin:

}
////////////////////////////////////////////////////////////////////////////////////////////////////////////////

//ログイン//////////////////////////////////////////////////////////////////////////////////////////////////////
function LogIn(&$result, &$error){
    //入力チェック&宣言////////////
    global $pdo, $TABLE;
    $postValues = array();

    array_push($postValues, ($user_id   = (isset($_POST['user_id'])?    $_POST['user_id']   : NULL)));
    array_push($postValues, ($password  = (isset($_POST['password'])?   $_POST['password']  : NULL)));

    if (!UNITY) {
        var_export($postValues); print"\n";
    }

    foreach ($postValues as $k => $v) {
    	if ($v === NULL) {
        	array_push($error, "PHPシステムエラー: POST_ERROR: Check Posted Values[" . $k . "]");
            goto fin;
        }
    }

    //////////////////////////////

    //userIDとPWが一致する行を拾ってくる
    $query = "SELECT name, point, shell, weapon, acce FROM ".$TABLE["ACCOUNT"]." WHERE user_id = ? AND password = ?";
    if (!UNITY) print $query . "\n";
    $stmt = $pdo->prepare($query);
    $stmt->execute(array($user_id, $password));

    $rows = $stmt->fetchAll();      //連想配列と数値配列両方

    if ($rows){
        $result += $rows[0];  //1レコードのすべての結果をこれで取得できている
        $stmt = $pdo->prepare("UPDATE ".$TABLE["ACCOUNT"]."  SET lastloginDate = NOW() WHERE user_id = ?");
        $stmt->execute(array($user_id));
        goto fin;
    } else {
        array_push($error, "ログインエラー: IDまたはパスワードが間違っています(ID:" . $user_id . ")");
    }

    fin:
}
////////////////////////////////////////////////////////////////////////////////////////////////////////////////

//単語データ取得//////////////////////////////////////////////////////////////////////////////////////////////////////
function GetWordData(&$result, &$error){
    //入力チェック&宣言////////////
    global $pdo, $TABLE;


    //単語データを全行引っこ抜いてくる
    $query = "SELECT answer, english_hint, japanese_hint FROM ".$TABLE["WORD"];
    if (!UNITY) print $query . "\n";
    $stmt = $pdo->prepare($query);
    $stmt->execute();

    $rows = $stmt->fetchAll();

    if ($rows == null){
        array_push($error, "PHPシステムエラー: 単語データが見つかりませんでした");
    } else{
        $result += $rows;
    }

    fin:
}
////////////////////////////////////////////////////////////////////////////////////////////////////////////////


//スコア登録////////////////////////////////////////////////////////////////////////////////////////////////////
function RegisterScore(&$result, &$error){

    //入力チェック&宣言////////////
    global $pdo, $TABLE;
    $postValues = array();

    array_push($postValues, ($user_id   = (isset($_POST['user_id'])?    $_POST['user_id']   : NULL)));
    array_push($postValues, ($score  = (isset($_POST['score'])?   $_POST['score']  : NULL)));
    array_push($postValues, ($cource  = (isset($_POST['cource'])?   $_POST['cource']  : NULL)));
    array_push($postValues, ($level  = (isset($_POST['level'])?   $_POST['level']  : NULL)));
    array_push($postValues, ($comment  = (isset($_POST['comment'])?   $_POST['comment']  : NULL)));

    if (!UNITY) {
        var_export($postValues); print"\n";
    }

    foreach ($postValues as $k => $v) {
    	if ($v === NULL) {
        	array_push($error, "PHPシステムエラー: POST_ERROR: Check Posted Values[" . $k . "]");
            goto fin;
        }
    }

    //////////////////////////////

    //user_idからid番号を取得
    $query = "SELECT id FROM ".$TABLE["ACCOUNT"]." WHERE user_id = ?";
    if (!UNITY) print $query."\n";
    $stmt = $pdo->prepare($query);
    $stmt->execute(array($user_id));

    while($row = $stmt -> fetch(PDO::FETCH_ASSOC)) {    //すべての取得が終わるとfalseをかえす
        if (!UNITY) printArray($row);
        $id = $row['id'];
    }

    //スコアデータを挿入
    $query = "INSERT INTO ".$TABLE["SCORE"]." (id, cource, level, score, comment) VALUES (?,?,?,?,?)";
    if (!UNITY) print $query."\n";
    $stmt = $pdo->prepare($query);
    $stmt->bindValue(1, $id, PDO::PARAM_INT);
    $stmt->bindValue(2, (int)$cource, PDO::PARAM_INT);
    $stmt->bindValue(3, (int)$level, PDO::PARAM_INT);
    $stmt->bindValue(4, (int)$score, PDO::PARAM_INT);
    $stmt->bindValue(5, $comment, PDO::PARAM_STR);
    $stmt->execute();

    $result["id"] = $user_id;

    fin:
}
////////////////////////////////////////////////////////////////////////////////////////////////////////////////


//ランキング取得////////////////////////////////////////////////////////////////////////////////////////////////
function GetRanking(&$result, &$error){
    //入力チェック&宣言////////////
    global $pdo, $TABLE;
    $postValues = array();

    array_push($postValues, ($cource = (isset($_POST['cource'])?    $_POST['cource']   : NULL)));
    array_push($postValues, ($rankNum = (isset($_POST['number'])?    $_POST['number']   : NULL)));

    if (!UNITY) {
        var_export($postValues); print"\n";
    }

    foreach ($postValues as $k => $v) {
    	if ($v === NULL) {
        	array_push($error, "PHPシステムエラー: POST_ERROR: Check Posted Values[" . $k . "]");
            goto fin;
        }
    }

    //////////////////////////////

    //コースが一致するデータを抽出
    $query = "SELECT y.level, x.name, y.score, y.comment FROM ".$TABLE["ACCOUNT"]." AS x JOIN ".$TABLE["SCORE"]." AS y USING(id)";
    $query = $query . " WHERE y.cource=? AND (SELECT COUNT(*) FROM ".$TABLE["SCORE"]." AS z WHERE y.cource=z.cource AND y.level=z.level AND y.score<z.score) <= ?";
    $query = $query . "  ORDER BY y.level ASC, y.score DESC";

    if (!UNITY) print($query."<br>");

    $stmt = $pdo->prepare($query);
    $stmt->bindValue(1, (int)$cource, PDO::PARAM_INT);
    $stmt->bindValue(2, (int)$rankNum-1, PDO::PARAM_INT);

    $stmt->execute();

    $rows = $stmt->fetchAll();  //PDOはフェッチしたすべての数値を文字列に強制変換する。糞仕様。（参考：http://d.hatena.ne.jp/erio_nk/20120621/1340267044）

    if ($rows){
        $result += $rows;
        goto fin;
    } else {
        array_push($error, "MySQLシステムエラー: コース[" . $cource . "]のランキングデータが取得できませんでした");
    }

    fin:
}
////////////////////////////////////////////////////////////////////////////////////////////////////////////////

//特定ユーザーのランキング順位取得//////////////////////////////////////////////////////////////////////////////////
function GetRank(&$result, &$error){
    //入力チェック&宣言////////////
    global $pdo, $TABLE;
    $postValues = array();

    array_push($postValues, ($cource = (isset($_POST['cource'])?    $_POST['cource']   : NULL)));
    array_push($postValues, ($user_id = (isset($_POST['user_id'])?    $_POST['user_id']   : NULL)));

    if (!UNITY) {
        var_export($postValues); print"\n";
    }

    foreach ($postValues as $k => $v) {
    	if ($v === NULL) {
        	array_push($error, "PHPシステムエラー: POST_ERROR: Check Posted Values[" . $k . "]");
            goto fin;
        }
    }

    //////////////////////////////

    //コースが一致するデータを抽出
    $query = "SELECT DISTINCT y.level, COUNT(y.score < z.score or null)+1 AS rank FROM ".$TABLE["ACCOUNT"]." AS x JOIN ".$TABLE["SCORE"]." AS y USING(id)  JOIN ".$TABLE["SCORE"]." AS z ON y.cource=z.cource AND y.level=z.level ";
    $query = $query . " WHERE x.user_id=? AND y.cource=?";
    $query = $query . " AND y.score = (SELECT MAX(score) FROM ".$TABLE["SCORE"]." AS w WHERE x.id=w.id AND y.cource=w.cource AND y.level=w.level)";
    $query = $query . " GROUP BY y.data_id";


    if (!UNITY) print($query."<br>");
    $stmt = $pdo->prepare($query);
    $stmt->bindValue(1, $user_id, PDO::PARAM_STR);
    $stmt->bindValue(2, (int)$cource, PDO::PARAM_INT);

    $stmt->execute();

    $rows = $stmt->fetchAll();  //PDOはフェッチしたすべての数値を文字列に強制変換する。糞仕様。（参考：http://d.hatena.ne.jp/erio_nk/20120621/1340267044）

    if ($rows){
        $result += $rows;
        goto fin;
    } else {
        array_push($error, "MySQLシステムエラー: コース[" . $cource. "]の自分の順位が取得できませんでした");
    }

    fin:
}
////////////////////////////////////////////////////////////////////////////////////////////////////////////////

//特定ユーザーのハイスコア取得//////////////////////////////////////////////////////////////////////////////////
function GetHighScore(&$result, &$error){
    //入力チェック&宣言////////////
    global $pdo, $TABLE;
    $postValues = array();

    array_push($postValues, ($cource = (isset($_POST['cource'])?    $_POST['cource']   : NULL)));
    array_push($postValues, ($user_id = (isset($_POST['user_id'])?    $_POST['user_id']   : NULL)));

    if (!UNITY) {
        var_export($postValues); print"\n";
    }

    foreach ($postValues as $k => $v) {
    	if ($v === NULL) {
        	array_push($error, "PHPシステムエラー: POST_ERROR: Check Posted Values[" . $k . "]");
            goto fin;
        }
    }

    //////////////////////////////

    //コースが一致するデータを抽出
    $query = "SELECT DISTINCT y.level, y.score FROM ".$TABLE["ACCOUNT"]." AS x JOIN ".$TABLE["SCORE"]." AS y USING(id)";
    $query = $query . " WHERE y.cource=? AND x.user_id=? AND y.score = (SELECT MAX(score) FROM ".$TABLE["SCORE"]." AS z WHERE x.user_id=? AND x.id=z.id AND y.cource=z.cource AND y.level=z.level)";
    $query = $query . "  ORDER BY y.level ASC, y.score DESC";

    if (!UNITY) print($query."<br>");

    $stmt = $pdo->prepare($query);
    $stmt->bindValue(1, (int)$cource, PDO::PARAM_INT);
    $stmt->bindValue(2, $user_id, PDO::PARAM_STR);
    $stmt->bindValue(3, $user_id, PDO::PARAM_STR);

    $stmt->execute();

    $rows = $stmt->fetchAll();  //PDOはフェッチしたすべての数値を文字列に強制変換する。糞仕様。（参考：http://d.hatena.ne.jp/erio_nk/20120621/1340267044）

    if ($rows){
        $result += $rows;
        goto fin;
    } else {
        array_push($error, "MySQLシステムエラー: コース[" . $cource . "]の自分のハイスコアが取得できませんでした");
    }

    fin:
}
////////////////////////////////////////////////////////////////////////////////////////////////////////////////

//ユーザーポイント更新//////////////////////////////////////////////////////////////////////////////////////////
function UpdatePoint(&$result, &$error){

    //入力チェック&宣言////////////
    global $pdo, $TABLE;
    $postValues = array();

    array_push($postValues, ($user_id   = (isset($_POST['user_id'])?    $_POST['user_id']   : NULL)));
    array_push($postValues, ($point  = (isset($_POST['point'])?   $_POST['point']  : NULL)));

    if (!UNITY) {
        var_export($postValues); print"\n";
    }

    foreach ($postValues as $k => $v) {
    	if ($v === NULL) {
        	array_push($error, "PHPシステムエラー: POST_ERROR: Check Posted Values[" . $k . "]");
            goto fin;
        }
    }

    //////////////////////////////

    //ポイントデータを更新
    $query = "UPDATE ".$TABLE["ACCOUNT"]." SET point = ? WHERE user_id = ?";
    if (!UNITY) print $query."\n";
    $stmt = $pdo->prepare($query);
    $stmt->bindValue(1, $point, PDO::PARAM_INT);
    $stmt->bindValue(2, $user_id, PDO::PARAM_STR);
    $stmt->execute();

    $result["id"] = $user_id;

    fin:
}
////////////////////////////////////////////////////////////////////////////////////////////////////////////////

//ユーザー装備更新//////////////////////////////////////////////////////////////////////////////////////////
function UpdateEquipment(&$result, &$error){

    //入力チェック&宣言////////////
    global $pdo, $TABLE;
    $postValues = array();

    array_push($postValues, ($user_id   = (isset($_POST['user_id'])?    $_POST['user_id']   : NULL)));
    array_push($postValues, ($shell     = (isset($_POST['shell'])?      $_POST['shell']  : NULL)));
    array_push($postValues, ($weapon    = (isset($_POST['weapon'])?     $_POST['weapon']  : NULL)));
    array_push($postValues, ($acce      = (isset($_POST['acce'])?       $_POST['acce']  : NULL)));

    if (!UNITY) {
        var_export($postValues); print"\n";
    }

    foreach ($postValues as $k => $v) {
    	if ($v === NULL) {
        	array_push($error, "PHPシステムエラー: POST_ERROR: Check Posted Values[" . $k . "]");
            goto fin;
        }
    }
    //////////////////////////////

    //装備情報を更新
    $query = "UPDATE ".$TABLE["ACCOUNT"]." SET shell=?, weapon=?, acce=? WHERE user_id = ?";
    if (!UNITY) print $query."\n";
    $stmt = $pdo->prepare($query);
    $stmt->bindValue(1, $shell, PDO::PARAM_INT);
    $stmt->bindValue(2, $weapon, PDO::PARAM_INT);
    $stmt->bindValue(3, $acce, PDO::PARAM_INT);
    $stmt->bindValue(4, $user_id, PDO::PARAM_STR);
    $stmt->execute();

    $result["id"] = $user_id;

    fin:
}
////////////////////////////////////////////////////////////////////////////////////////////////////////////////


//ユーザー問題別正誤数更新//////////////////////////////////////////////////////////////////////////////////////
function UpdateAnswerLog(&$result, &$error){

    //入力チェック&宣言////////////
    global $pdo, $TABLE;
    $postValues = array();

    array_push($postValues, ($user_id       = (isset($_POST['user_id'])?        $_POST['user_id']   : NULL)));
    array_push($postValues, ($question_id   = (isset($_POST['question_id'])?   $_POST['question_id']  : NULL)));
    array_push($postValues, ($is_correct    = (isset($_POST['is_correct'])?     $_POST['is_correct']  : NULL)));

    if (!UNITY) {
        var_export($postValues); print"\n";
    }

    foreach ($postValues as $k => $v) {
    	if ($v === NULL) {
        	array_push($error, "PHPシステムエラー: POST_ERROR: Check Posted Values[" . $k . "]");
            goto fin;
        }
    }
    //////////////////////////////

    //アカウントIDと問題IDの組み合わせレコードが存在しないなら挿入
    $query = "INSERT IGNORE INTO ". $TABLE["ANSWER_LOG"]." (account_id, question_id) VALUES ((SELECT id FROM ".$TABLE["ACCOUNT"] ." WHERE user_id = ?), ?)";

    if (!UNITY) print $query."\n";
    $stmt = $pdo->prepare($query);
    $stmt->bindValue(1, $user_id, PDO::PARAM_STR);
    $stmt->bindValue(2, $question_id, PDO::PARAM_INT);
    $stmt->execute();

    //正答数あるいは誤答数をインクリメント
    $query = "UPDATE ".$TABLE["ANSWER_LOG"]." AS y JOIN ".$TABLE["ACCOUNT"]." AS x ON x.id = y.account_id";

    if ($is_correct) {
        $query = $query . " SET correct = correct + 1 WHERE x.user_id = ? AND y.question_id = ?";
    } else {
        $query = $query . " SET incorrect = incorrect + 1 WHERE x.user_id = ? AND y.question_id = ?";
    }

    if (!UNITY) print $query."\n";
    $stmt = $pdo->prepare($query);
    $stmt->bindValue(1, $user_id, PDO::PARAM_STR);
    $stmt->bindValue(2, $question_id, PDO::PARAM_INT);
    $stmt->execute();

    $result["id"] = $user_id;   //完了通知用の目印

    fin:
}
////////////////////////////////////////////////////////////////////////////////////////////////////////////////

//ユーザー問題別正誤数更新//////////////////////////////////////////////////////////////////////////////////////
function GetAnswerLog(&$result, &$error){

    //入力チェック&宣言////////////
    global $pdo, $TABLE;
    $postValues = array();

    array_push($postValues, ($user_id       = (isset($_POST['user_id'])?        $_POST['user_id']   : NULL)));

    if (!UNITY) {
        var_export($postValues); print"\n";
    }

    foreach ($postValues as $k => $v) {
    	if ($v === NULL) {
        	array_push($error, "PHPシステムエラー: POST_ERROR: Check Posted Values[" . $k . "]");
            goto fin;
        }
    }
    //////////////////////////////

    $query = "SELECT question_id, correct, incorrect FROM ". $TABLE["ANSWER_LOG"]." AS y JOIN ".$TABLE["ACCOUNT"]." AS x ON y.account_id = x.id WHERE x.user_id = ?";

    if (!UNITY) print $query."\n";
    $stmt = $pdo->prepare($query);
    $stmt->bindValue(1, $user_id, PDO::PARAM_STR);
    $stmt->execute();

    $rows = $stmt->fetchAll();  //PDOはフェッチしたすべての数値を文字列に強制変換する。

    if ($rows){
        $result += $rows;
    }

fin:
}
////////////////////////////////////////////////////////////////////////////////////////////////////////////////
