use makedb.exe to generate database scheme and generate db

makedb -s mssql -c "server=.\localhost;database=test" -f -p "C:\project\*.mkdb" -o "C:\project\scripts\" -d

 -s - database server type:
		mssql	- Microsoft SQL Server
		mysql	- MySQL
		postgre - PostgreSQL 
		oracle	- Oracle DB
 -c - connection string
 -f - force scheme/db rewrite
 -p - sources path C:\project\*.mkdb - path to .mkdb file(s)
 -o - output for scripts
 -d - create database