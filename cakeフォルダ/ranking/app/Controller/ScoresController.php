<?php

class ScoresController extends AppController{
	
	public $name = 'Score';
	public $uses = array('Score','User');
	
	public function index(){
		$data = $this->Score->find('all');
		 $this->set('data',$data);
	}
	
}

?>