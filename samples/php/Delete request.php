<?php
echo 'Hello world';
echo PHP_EOL;

$requestModel = null;
$applicationDataJson = "";
if($applicationDataJson != null) {
	$applicationDataJson = json_encode($applicationData);
}

$utcDate = time();
$contentSize = strlen($applicationDataJson);
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
$headers[] = 'Content-Type: application/json; charset=utf-8';
$headers[] = 'Content-Length: ' . strlen($applicationDataJson);


$url = "https://" . $customerName . ".bloxs.io/" . $relativeUrl;
echo 'Making request to ' . $url;
echo PHP_EOL;
$opt = array('http'=>array(
		'method' => 'DELETE',
		'header' => $headers,
		'content' => $applicationDataJson
   ));
$context = stream_context_create($opt);
$response = file_get_contents($url, false, $context);

echo PHP_EOL;
echo 'Response status code: ' . $http_response_header[0];

echo PHP_EOL;
echo 'Goodbye world';
echo PHP_EOL;
?>