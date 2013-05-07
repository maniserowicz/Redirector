$scriptDir = Split-Path $MyInvocation.MyCommand.Path -Parent
$secretsXmlPath = "$scriptDir\..\..\secrets.xml"
$redirectorConfigPath = "$scriptDir\redirector.config"

$googleKey = $env:RedirectorGoogleKey
$googleSecret = $env:RedirectorGoogleSecret

if (test-path $secretsXmlPath) {
    write-host "secrets.xml exists in sln dir, using values from there"
    [xml]$secrets = get-content $secretsXmlPath
    $googleKey = $secrets.secrets.googleKey
    $googleSecret = $secrets.secrets.googleSecret
}
else {
    write-host "secrets.xml does not exist in sln dir, using values from env variables"
}

$replacedContent = get-content $redirectorConfigPath | % {
    $_ -replace "{{google-key}}" , $googleKey `
    -replace "{{google-secret}}" , $googleSecret
}
set-content $redirectorConfigPath $replacedContent