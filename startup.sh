cd /var/www/Embyte
dotnet run --environment Production --urls=http://localhost:5001/ > Logs/hostingLog.log 2> Logs/hostingErrLog.log