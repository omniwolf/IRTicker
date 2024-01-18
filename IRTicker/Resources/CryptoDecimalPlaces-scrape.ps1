####
##
## screen scrapes the API page to pull the volume and fiat decimal places for each crypto into a 
## file.  The format is:
##
## <crypto>,<decimals for volume>,<decimals for fiat price orders>
##
####

$WebClient = New-Object System.Net.WebClient
$Results = $WebClient.DownloadString("https://www.independentreserve.com/products/api")
$ResArray = $Results -split "class=`"decimal-places-table__code`""  # let's hope this doesn't change
$count = 0
$masterStr = ""

write-host "Parsing /api page..." -ForegroundColor Yellow
do {
    $count++  # have to do this at the top so the until clause doesn't evaluate the next array element before we've used it
    $crypto = $ResArray[$count] -split "<span>"
    $cryptoTicker = $crypto[1].SubString(0, $crypto[1].IndexOf("</span>"))

    if ($cryptoTicker -eq "Xbt") {  # Xbt has extra HTML - there is an info popup thing to talk about how "Xbt" is legacy.  Need special code to handle this, all the other cryptos are normal
        $masterStr += $cryptoTicker + ","
        $subSplit = $crypto[1] -split "</div></div></div></div></div></td> <td>"
        $masterStr += $subSplit[1].Substring(0,1) + ","
        $masterStr += $subSplit[1].Substring(11,1) + "`n"
    }
    else {
        $masterStr += $cryptoTicker + ","
        $masterStr += $crypto[1].Substring((23 + $cryptoTicker.Length), 1) + ","
        $masterStr += $crypto[1].Substring((34 + $cryptoTicker.Length), 1) + "`n"
    }
    write-host "Added " $cryptoTicker
} until ($ResArray[$count] -match "The following public API methods are available")

write-host "Writing CSV..." -ForegroundColor Yellow
$masterStr | Out-File -FilePath .\cryptoDPs.csv  # this doesn't actually create a usable CSV, but it's good enough for IRTicker to parse
Write-Host "Complete!" -ForegroundColor Green
