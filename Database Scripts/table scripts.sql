USE Banking
GO
------------------------Accounts table--------------------------------------------------
IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 't_accounts')
BEGIN
	CREATE TABLE t_accounts
	(
		AccountId BIGINT PRIMARY KEY,
		AccountNumber VARCHAR(20) UNIQUE NOT NULL,
		AccountName NVARCHAR(100) UNIQUE NOT NULL,
		AccountBalance NUMERIC(18,2) NOT NULL,
		CreationDate DATETIME NOT NULL,
		ClosedDate DATETIME NOT NULL,
		AccountStatus VARCHAR(50) NOT NULL CHECK (AccountStatus IN ('Closed', 'Blocked', 'Active', 'PendingActivation', 'Suspended'))
	)
END

IF NOT EXISTS (SELECT * FROM sys.triggers WHERE name = 'trg_Account_Insert' AND parent_id = OBJECT_ID('t_accounts'))
BEGIN
    -- Create the trigger if it doesn't exist
    CREATE TRIGGER trg_Account_Insert
    ON t_accounts
    AFTER INSERT
    AS
    BEGIN
        -- Declare a variable to store the generated accountId
        DECLARE @GeneratedAccountId NVARCHAR(20);

        -- Loop through all the rows inserted into the BankAccounts table
        DECLARE cur CURSOR FOR 
        SELECT * FROM INSERTED;

        OPEN cur;
        FETCH NEXT FROM cur INTO @GeneratedAccountId;
        
        WHILE @@FETCH_STATUS = 0
        BEGIN
            -- Generate the accountId: FAKEBANK + 4-digit branch code + 8-digit random number
            SET @GeneratedAccountId = 'FAKEBANK' + 5555 +
                                      RIGHT('00000000' + CAST(FLOOR(RAND(CHECKSUM(NEWID())) * 100000000) AS VARCHAR(8)), 8);

            -- Update the inserted row with the generated accountId
            UPDATE t_accounts
            SET AccountId = @GeneratedAccountId
            WHERE AccountId IS NULL;

            FETCH NEXT FROM cur INTO @GeneratedAccountId;
        END;

        CLOSE cur;
        DEALLOCATE cur;
    END;
END;

---------------------------------Transactions Table----------------------------------------
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_AccountNumber' AND object_id = OBJECT_ID('t_accounts'))
BEGIN
    CREATE NONCLUSTERED INDEX IX_AccountNumber
    ON t_accounts (AccountNumber);
END

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 't_account_transactions_transfer')
BEGIN
	CREATE TABLE t_account_transactions_transfer
	(
		TransactionID BIGINT PRIMARY KEY,
		AccountId_From BIGINT NOT NULL,
		AccountBalance_From NUMERIC(18,2) NOT NULL,
		AccountId_To BIGINT NOT NULL,
		AccountBalance_To NUMERIC(18,2) NOT NULL,
		TransactionTime DATETIME NOT NULL,
		Amount NUMERIC(18,2) NOT NULL,
		FOREIGN KEY(AccountId_From) REFERENCES t_accounts(AccountId),
		FOREIGN KEY(AccountId_To) REFERENCES t_accounts(AccountId),
	)
END

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_FromAccountId' AND object_id = OBJECT_ID('t_account_transactions_transfer'))
BEGIN
    CREATE NONCLUSTERED INDEX IX_FromAccountId
    ON t_account_transactions_transfer (AccountId_From);
END

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_ToAccountId' AND object_id = OBJECT_ID('t_account_transactions_transfer'))
BEGIN
    CREATE NONCLUSTERED INDEX IX_ToAccountId
    ON t_account_transactions_transfer (AccountId_To);
END