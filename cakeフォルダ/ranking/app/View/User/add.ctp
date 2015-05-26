<h1>ユーザ登録(仮)</h1>
<?php

echo $this->Form->create(false,array('type'=>'post','action'=>'add'));
echo 'ユーザ名:' . $this->Form->text('User.name');
echo $this->Form->error('User.name');
echo $this->Form->end("送信");
?>
