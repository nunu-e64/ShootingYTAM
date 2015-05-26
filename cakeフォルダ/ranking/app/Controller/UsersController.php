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
		$data = 0;
		if($this->request->isPost()){
			$record = $this->data;
			$flg = $this->User->save($record);
			if($flg){
				$this->redirect('.');
			}
		}
	}

}
