# SqlServerRegex

Provides the ability to run regular expressions as efficient User Defined Functions (UDFs). Currently Sql Server has no implementation of Regular Expressions, however they can be very useful when searching for data with more complex pattern than provided by LIKE or PATINDEX. Although there is not a standard method of using Regular Expressions in SQL many database systems provide native implementations.

Normally you should build logic into an external application rather than extending a database server with additional functionality, but imagine needing to search for a particular pattern in a large dataset. Extracting all that data to an external application would be very inefficient & could cause network overload issues, so being able to run a regular expression directly in the where clause of a query can save both. 

Using UDFs has a performance hit, but generally CLR UDFs are generally more efficient and faster than pure TSQL UDFs (although that depends on the situation) especially when no data access is happening internally. Ideally you should still use the built in functions where possible, as the execution planner knows how to use those functions in the most efficient way and how to take advantage of indexes while using them.

### Installation

- Compile the project in `Release` configuration
- Then open the `UserDefinedFunctions.sql` file from the project and change the value in the `@dllLocation` variable to that of the new dll that has just been created
- Run the SQL against the Sql Server you want to add the functionality too.
- You can then delete the dll file as it will be cached into the Sql Server.

### Support

The clr methods should work on all Sql Server versions above 2005. It does also require .Net 4 be installed, although you could recompile the projects against .Net 3.5 and it should work with no issues. It is recommended you stick with the newest version of .Net possible though as Regular Expressions are a lot more efficient since they were re-written for .Net 4