for proj in **/*.csproj; do [[ $proj == *"LocalApp"* ]] || dotnet build "$proj"; done
