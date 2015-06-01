<?php
class UsersController extends AppController {

	public $name = 'User';
	public $uses = array('Score','User');
	
	//ユーザの全件検索
	public function index(){
		$this->autoRender = false;
		$data = $this->User->find('all');
		json_encode($data);

	}
	//ユーザ登録
	public function add(){
		$this->autoRender = false;
		$record['User']['name'] = $this->request->query['name'];
		$this->User->save($record);
	}

}
