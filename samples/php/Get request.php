<?php
echo 'Hello world';
echo PHP_EOL;

$utcDate = time();
$contentSize = 0;
$apiKey = "apiKey";
$apiSecret = "apiSecret";
$customerName = "customerName";
$relativeUrl = "Test";

$tokenParts = $apiKey . ":" . $utcDate . ":" . $contentSize;
$hashedRaw = hash_hmac("sha256", $tokenParts, $apiSecret);
$hashedDashRemoved = str_replace("-", "", $hashedRaw);
$hashed = strtolower($hashedDashRemoved);

$headers = array();
$headers[] = 'Authorization: bloxs ' . $apiKey . ':' . $hashed;
$headers[] = 'x-timestamp: ' . $utcDate;

$url = "https://" . $customerName . ".bloxs.io/" . $relativeUrl;
echo 'Making request to ' . $url;
echo PHP_EOL;
$opt = array('http'=>array(
       'method' => 'GET',
       'header' => $headers
       ));
$context = stream_context_create($opt);
$response = file_get_contents($url, false, $context);

echo PHP_EOL;
echo 'Response status code: ' . $http_response_header[0];

echo PHP_EOL;
echo 'Goodbye world';
echo PHP_EOL;
?>