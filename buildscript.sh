rm -r bin
rm -r obj
dotnet build
cp bin/Debug/net6.0/SC2ExpansionLoader.dll /home/silentstorm/.steam/steam/steamapps/common/BloonsTD6/Mods