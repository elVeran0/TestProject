chcp 65001
"C:\Program Files\PostgreSQL\10\bin\psql.exe" postgresql://postgres:1234@localhost:5432 < createDb.sql
"C:\Program Files\PostgreSQL\10\bin\psql.exe" postgresql://postgres:1234@localhost:5432/test_project_db < agregate.sql
pause