<?php

class ScoresController extends AppController{
	
	public $name = 'Score';
	public $uses = array('Score','User');
	
	//スコアの全件検索
	public function index(){
		$this->autoRender = false;
		$data = $this->Score->find('all');
		echo json_encode($data);
		 $this->set('data',$data);
	}
	//ユーザのスコア情報
	public function userScore(){
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
	//スコアの追加
	public function add(){
		$this->autoRender = false;
		$record['Score']['user_id'] = $this->request->query['user_id'];
		$record['Score']['score'] = $this->request->query['score'];
		$this->Score->save($record);
	}
	//全体のランキング情報
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
	
}

?>