CREATE PROCEDURE CHANGE_INCREMENT 
	@table varchar(100), 
	@column varchar(100), 
	@incrementValue int
AS
	DECLARE @temp_table VARCHAR(15)
	DECLARE @DynamicSQL VARCHAR(1000)
	DECLARE @columns VARCHAR(8000)
BEGIN
	/* Ensure that the Query is valid i.e. column & table exist */
	IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = @table AND COLUMN_NAME = @column)
	BEGIN
		IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = @table)
		BEGIN
			PRINT 'Invalid Query: Table ' + @table + ' not found within Database'
			RETURN
		END
		PRINT 'Invalid Query: Column ' + @column + ' not found within Table ' + @table
		RETURN
	END

	/* Create a Temporary Table Name. Requires two ## to declare it as a global-temporary value */
	SET @temp_table = '##TEMP_TABLE_' + CAST((FLOOR(RAND()*(100))) AS VARCHAR(3))

	/* Ensure that it does not already exist within the database and re-generate if it does */
	WHILE EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = @temp_table)
	BEGIN 
		SET @temp_table = '##TEMP_TABLE_' + CAST((FLOOR(RAND()*(100))) AS VARCHAR(3))
	END

	/* Copy all data from the table we want to change into the temporary table and delete all current data of the old table */
	SET @DynamicSQL = N'SELECT * INTO ' + @temp_table + ' FROM ' + @table + ';TRUNCATE TABLE ' + @table

	EXEC(@DynamicSQL)

	/* Re-seed the old table with a new increment value */
	DBCC CHECKIDENT(@table, RESEED, @incrementValue)
	
	/* Get the column names of all data in our old table except the re-seeded column.
		
	   Breakdown:

			ISNULL ensures that the first string concatation begins with '' as @columns is NULL by default

			QUOTENAME ensures that column names with spaces inside them are supported as well

			ORDINAL_POSITION ensures that the columns are in the same order as they appear within the DB

	 */
	SELECT @columns = ISNULL(@columns + ', ', '') + QUOTENAME(COLUMN_NAME) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = @table AND COLUMN_NAME <> @column ORDER BY ORDINAL_POSITION

	/* Copy all data from the temporary table into the re-seeded table and drop the temporary table */
	SET @DynamicSQL = N'INSERT INTO ' + @table + '(' + @columns + ') SELECT ' + @columns + ' FROM ' + @temp_table + ';DROP TABLE ' + @temp_table

	EXEC(@DynamicSQL)
END
