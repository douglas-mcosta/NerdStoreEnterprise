#aguardando 90 segundos para o provisionamento e start do banco
sleep 40s

#rodar o comando para criar o banco
/opt/mssql-tools/bin/sqlcmd -S localhost,1433 -U SA -P "MeuDB@123" -i criacao-banco-docker.sql