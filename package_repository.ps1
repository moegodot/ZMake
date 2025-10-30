
# check git repository is committed and updated

if ($null -eq (git status --porcelain=v1 --short)) {
    git archive --format=tar.gz -o ZMake/resources/zmake.tar.gz master
    Write-Host "Packaged zmake.tar.gz to ZMake/resources/zmake.tar.gz"
}
else {
    Write-Error "Git repository is not committed and updated"
    exit 1
}
