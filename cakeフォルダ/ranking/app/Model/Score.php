<?php

class Score extends AppModel{
	
	public $name = 'Score';
	
	public $hasOne = array(
		"User" => array(
			'className' => 'Score',
			'condition' => '',
			'order' => 'User.score DESC',
			'dependent'  => false,
			'foreignKey' => 'user_id',
		)
	);
}