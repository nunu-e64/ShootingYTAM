<?php

class User extends AppModel{
	
	public $name = 'User';
	
	public $validate = array(
		'name' => array(
				'rule' => 'isUnique',
				'required' => true,
				'message' => '既に同じ名前のユーザが存在します。'
			)
	);
	public $hasMany = 'Score';
}