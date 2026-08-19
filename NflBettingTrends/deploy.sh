echo "Building..."
dotnet publish -c Release -o ./publish

echo "Zipping..."
cd ./publish && zip -r ../publish.zip . && cd ..

echo "Deploying..."
az webapp deploy --resource-group BettingProject --name NflBettingProjectWebApp --src-path publish.zip --type zip

echo "Done!"