@echo off
echo Pushing all local changes to the 1.4 branch.

set /p message="Commit Message: "

git add .
git commit -a -m "Pushed automatically on %date%: %message%"
git push origin 1.4