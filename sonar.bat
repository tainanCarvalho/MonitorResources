dotnet sonarscanner begin /k:"measure_consume_project" /d:sonar.host.url="http://127.0.0.1:9000" /d:sonar.login="sqp_5b3582cc3eff59319d037c4ff74fa9ecf81dd32f" /d:sonar.cs.opencover.reportsPaths=Interval.Test\coverage.net6.0.opencover.xml
dotnet build
dotnet test --framework net6.0 /p:CollectCoverage=true /p:CoverletOutputFormat=opencover /p:Threshold=40  /p:ThresholdStat=average /p:Exclude=[Interval.Storage.Rules]*
dotnet sonarscanner end /d:sonar.login="sqp_5b3582cc3eff59319d037c4ff74fa9ecf81dd32f"