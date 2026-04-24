$xml = Get-Content 'e:\Spring_2026\AISWDC\CLS\Document\02_Requirements\Report3_temp\word\document.xml' -Raw
$text = $xml -replace '<[^>]+>', ' '
$text = $text -replace '\s+', ' '
$text | Out-File 'e:\Spring_2026\AISWDC\CLS\Document\02_Requirements\Report3_text.txt'
