use makedb.exe to generate database scheme and generate db

makedb -s mssql -c "server=.\localhost;database=test" -f C:\project\*.mkdb

 -s - database server type:
		mssql	- Microsoft SQL Server
		mysql	- MySQL
		postgre - PostgreSQL 
		oracle	- Oracle DB
 -c - connection string
 -f - force scheme/db rewrite

 C:\project\*.mkdb - path to .mkdb file(s)