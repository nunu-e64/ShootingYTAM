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
	public function userAdd(){
		$this->autoRender = false;
		$record['User']['name'] = $this->request->query['name'];
		if($this->User->save($record)){
			// print_r("save success!!!");
			//ユーザ登録時に発行されたidをUnityのコアデータとして保持
			$data = $this->User->find('all',array(
				'fields' => 'User.id',
				'conditions' => array('User.name' => $this->request->query['name']),
				'order' => '',
				'group' => '',
				'limit' => '',
				)
			);
			//最近ユーザ登録されたユーザのIDを返す
			echo $this->User->getLastInsertID();
		}else{
			//ユーザ名が重複した
			echo "false";
		}
    }
	//ユーザ消去(ID)
	//:Unity側にidをコアデータとして渡す。
	public function userDelete(){
		$this->autoRender = false;
		$data = $this->request->query["id"];
		if($this->User->delete($data)){
			print_r("delete success!!!");
		}else{
			print_r("not delete success///");
		}
	}

}
