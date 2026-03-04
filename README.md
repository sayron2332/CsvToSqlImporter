## Results
- **Total rows in database:** 29,889
- **Duplicates removed:** 111 
- **Top PULocationID by average tip:** 194 (Average tip: $12.94)
- **Longest trip distance:** 52.3 miles
- **Longest trip duration:** 1,439 minutes
- **Search performance:** Found 1,012 records for PULocationID 161

### Large Data Handling (10GB+ Scenario)
If the application were to handle a 10GB CSV file, the following architectural changes would be implemented:
- **Streaming (IEnumerable):** Instead of loading the entire file into a `List` with `ToList()`, I would use `IEnumerable` to read records one by one (Streaming), keeping memory usage low and constant.
- **Batching:** Records would be processed and inserted in batches (e.g., every 50,000 records) to prevent memory overflow and transaction log bloat in SQL Server.

How to Run?
Apply Migrations:
Open the Package Manager Console in Visual Studio and run: Update-Database
This will automatically create the database and the required table schema using Entity Framework Core.

And run Project
