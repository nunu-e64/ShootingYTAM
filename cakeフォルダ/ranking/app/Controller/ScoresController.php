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
	//スコアの追加
	public function add(){
		$this->autoRender = false;
		print_r($this->request->query['score']);
		$record['Score']['score'] = $this->request->query['score'];
		$this->Score->save($record);
	}
	
}

?>