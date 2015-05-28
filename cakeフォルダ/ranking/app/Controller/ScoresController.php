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
	
}

?>