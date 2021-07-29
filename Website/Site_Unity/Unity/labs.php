<?php

$time1 = DateTime::createFromFormat("H:i:s","00:17:00");
$time2 = DateTime::createFromFormat("H:i:s","00:18:00");

echo $time1->diff($time2)->format("%R");
