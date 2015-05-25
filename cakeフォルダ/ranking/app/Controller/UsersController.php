<?php

class UsersController extends AppController{
	
	//使用するコントローラー名
	public $name = 'User';
	
	public function index(){
		
		$data = $this->User->find('all');
		$this->set('data',$data);
		
		
	}
	
}

?>