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
	public function user_score(){
		$this->autoRender = false;
		$data = $this->Score->find('all',array(
			'fields' => array('Score.user_id','Score.score'),
			'conditions' => array('Score.user_id' => $this->request->query['user_id']),
			'order' => '',
			'group' => '',
			'limit' => '',
			)
		);
		echo json_encode($data);
	}
	//スコアの追加
	public function add(){
		$this->autoRender = false;
		print_r($this->request->query['score']);
		$record['Score']['score'] = $this->request->query['score'];
		$this->Score->save($record);
	}
	
}

?>