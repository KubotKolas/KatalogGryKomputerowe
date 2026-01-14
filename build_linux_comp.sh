#!/bin/bash
# Buduje wszystko co nie jest aplikacją okienkową (WPF)
for proj in KolasinskiMarcinek.KatalogGryKomputerowe.{CORE,INTERFACES,DAOMock,DAOFile,DAOSQL,BL,WebApp,ConsoleTest}; do
    echo "--- Budowanie: $proj ---"
    dotnet build "$proj"
done
