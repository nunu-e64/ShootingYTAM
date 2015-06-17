<?php

class ScoresController extends AppController{
	
	public $name = 'Score';
	public $uses = array('Score','User');
	
	public function index(){
		$this->autoRender = false;
		$data = $this->Score->find('all');
		echo json_encode($data);
		$this->set('data',$data);
	}
	//ユーザごとにスコアを追加
	//最高スコアを取らない場合でもインサートされる。（後々処理が重くなりそうなのでハイスコアのみ追加する処理に変更します）
	public function scoreAdd(){
		$this->autoRender = false;
		// print_r($this->request->query['user_id']);
		// print_r($this->request->query['score']);
		$record['Score']['user_id'] = $this->request->query['user_id'];
		$record['Score']['score'] = $this->request->query['score'];
		$this->Score->save($record);
		print_r("success!!!");
		if($this->Score->save($record)){
			echo "true";
		}else{
			echo "false";
		}
	}
	//全ユーザのスコアを元に全体ランキング
	public function ranking(){
		$this->autoRender = false;
		$data = $this->Score->find('all',array(
			'fields' => array('Score.score','Score.user_id','User.name'),
			'conditions' => '',
			'order' => 'Score.score DESC',
			'group' => '',
			'limit' => '',
			)
		);
		echo json_encode($data);
	}
	//ユーザのベストスコア
	public function userScore(){
		$this->autoRender = false;
		$data = $this->Score->find('all',array(
			'fields' => array('User.name','Score.score'),
			'conditions' => array('User.id' => $this->request->query['user_id']),
			'order' => 'Score.score DESC',
			'group' => '',
			'limit' => '',
			)
		);
		echo json_encode($data);
	}

	//ユーザ個人のスコアデータの削除
	public function usersScoreDelete(){
		$this->autoRender = false;
		$data = $this->Score->find('all',array(
			'fields' => 'Score.id',
			'conditions' => array('Score.user_id' => $this->request->query['user_id']),
			'order' => '',
			'group' => '',
			'limit' => '',
			)
		);
		if($this->Score->delete($data)){
			print_r("delete success!!!");
		}else{
			print_r("not delete success///");
		}
	}	
}
?>