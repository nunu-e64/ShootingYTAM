<h1>Debug</h1>
<table>
<?php
for($i = 0;$i < count($data);$i++){
	$arr = $data[$i]['User'];
	echo "<tr><td>{$arr['id']}</td>";
	echo "<td>{$arr['name']}</td>";
	echo"<td>{$arr['score']}</td></tr>";
}
?>
</table>

<pre>
<?php
print_r($data);
?>
</pre>