DROP DATABASE IF EXISTS FinanceManager;
CREATE DATABASE FinanceManager;
CREATE USER "finance_manager"@"localhost" IDENTIFIED BY "1337";
GRANT ALL ON FinanceManager.* TO "finance_manager"@"localhost";