declare @dllLocation nvarchar(100);
set @dllLocation = '**CHANGE TO LOCATION OF DLL**';
go

-- Set the server so that it will except CLR functionality

sp_configure 'show advanced options', 1;
GO
RECONFIGURE;
GO
sp_configure 'clr enabled', 1;
GO
RECONFIGURE;
GO

-- Drop existing UDFs and Assembly in case they exist and your updating them

IF OBJECT_ID (N'dbo.[RegexIsMatch]', N'FS') IS NOT NULL
	Drop Function [RegexIsMatch];
GO

IF OBJECT_ID (N'dbo.[RegexReplace]', N'FS') IS NOT NULL
	Drop Function [RegexReplace];
GO

IF  EXISTS (SELECT * FROM sys.assemblies asms WHERE asms.name = N'SqlServerRegex')
DROP ASSEMBLY [SqlServerRegex];
GO

-- Add the assembly

CREATE ASSEMBLY [SqlServerRegex]
AUTHORIZATION [dbo]
FROM @dllLocation
WITH PERMISSION_SET = SAFE
GO

-- Add the functions

CREATE FUNCTION [dbo].[RegexReplace](@input [nvarchar](max), @pattern [nvarchar](4000), @replacement [nvarchar](4000), @options [nvarchar](10))
-- Performs a regular expression comparison on the input passed, using the regular expression. Returns 0 on no match and 1 on success.
-- Options are a string of flags representing the regular expression options m = MultiLine, s = SingleLine, i = IgnoreCase
RETURNS nvarchar(max) WITH EXECUTE AS CALLER
AS 
EXTERNAL NAME [SqlServerRegex].[UserDefinedFunctions].[RegexReplace]
GO

CREATE FUNCTION [dbo].[RegexIsMatch](@input [nvarchar](max), @pattern [nvarchar](4000), @options [nvarchar](10))
-- Performs a regular expression comparison on the input passed, using the regular expression and replaces any matches with the replacement.
-- Options are a string of flags representing the regular expression options m = MultiLine, s = SingleLine, i = IgnoreCase
RETURNS [bit] WITH EXECUTE AS CALLER
AS 
EXTERNAL NAME [SqlServerRegex].[UserDefinedFunctions].[RegexIsMatch]
GO