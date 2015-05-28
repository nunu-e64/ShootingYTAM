<?php
class UsersController extends AppController {

	public $name = 'User';
	public $uses = array('Score','User');

	public function index(){
		$data = $this->Score->find('all');
		$this->set('data',$data);
	}
	//ユーザ登録
	public function add(){
		$this->autoRender = false;
		$record['User']['name'] = $this->request->query['name'];
		print_r($this->request->query['name']);
		$this->User->save($record);
	}

}
