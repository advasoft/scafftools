use makedomain.exe to generate domain model over mkdb serialized model (*.json)

makedomain -l cs -f -m "C:\project\model.json" -s mssql -c "server=.\localhost;database=test" -o "C:\project\domains\"

 -l - source code language:
		cs		- C#
		vb		- Visual Basic
		cpp		- C++
		jv		- Java
		php		- PHP
		py		- Python
		rb		- Ruby
		js		- Javascript
 -f - force scheme/db rewrite
 -m - model path C:\project\*.json - path to json file (serialized model)
 -s - database server type:
		mssql	- Microsoft SQL Server
		mysql	- MySQL
		postgre - PostgreSQL 
		oracle	- Oracle DB
 -c - connection string
 -o - output for domains

