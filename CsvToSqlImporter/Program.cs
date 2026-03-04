using CsvHelper;
using CsvHelper.Configuration;
using CsvToSqlImporter.ApplicationDbContext;
using CsvToSqlImporter.Entites;
using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using System.Globalization;



static List<AppTrip> GetProcessedTrips(string filePath)
{
    var trips = new List<AppTrip>();
    var duplicates = new List<AppTrip>();
    var seenKeys = new HashSet<string>();

    var config = new CsvConfiguration(CultureInfo.InvariantCulture)
    {
        HasHeaderRecord = true,
        PrepareHeaderForMatch = args => args.Header.Trim(),
    };

    using (var reader = new StreamReader(filePath))
    using (var csv = new CsvReader(reader, config))
    {
        var rawRecords = csv.GetRecords<AppTrip>().ToList();

        foreach (var record in rawRecords)
        {

            record.StoreAndFwdFlag = record.StoreAndFwdFlag.Trim();


            record.StoreAndFwdFlag = record.StoreAndFwdFlag == "Y" ? "Yes" : "No";

            var estZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
            record.TpepPickupDatetime = TimeZoneInfo.ConvertTimeToUtc(record.TpepPickupDatetime, estZone);
            record.TpepDropoffDatetime = TimeZoneInfo.ConvertTimeToUtc(record.TpepDropoffDatetime, estZone);
            string key = $"{record.TpepPickupDatetime}_{record.TpepDropoffDatetime}_{record.PassengerCount}";

            if (!seenKeys.Add(key))
            {
                duplicates.Add(record);
            }
            else
            {
                seenKeys.Add(key);
                trips.Add(record);
            }
        }
    }
    if (duplicates.Count > 0)
    {
   
        string exePath = AppDomain.CurrentDomain.BaseDirectory;
        var projectDir = Directory.GetParent(exePath)?.Parent?.Parent?.Parent;

        if (projectDir != null)
        {
            string fullPath = Path.Combine(projectDir.FullName, "Files", "duplicates.csv");
            using (var writer = new StreamWriter(fullPath))
            using (var csvWriter = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csvWriter.WriteRecords(duplicates);
            }
            Console.WriteLine($"Write {duplicates.Count} duplicates To folder");
        }
    }

    return trips;
}

static void WriteToDatabase()
{
    string csvPath = Path.Combine(Directory.GetCurrentDirectory(), "Files", "sample-cab-data.csv");

    var tripsToInsert = GetProcessedTrips(csvPath);

    using (var context = new AppDbContext())
    {
        Console.WriteLine($"Write {tripsToInsert.Count} Record To Database");

        context.Database.ExecuteSqlRaw("TRUNCATE TABLE AppTrips");
        context.BulkInsert(tripsToInsert);

        Console.WriteLine("All done!");
    }
}


static void DoAnalyticQueries()
{
    using var context = new AppDbContext();
   
    
    var topTip = context.AppTrips
        .GroupBy(t => t.PULocationID)
        .Select(g => new { LocationId = g.Key, AvgTip = g.Average(t => t.TipAmount) })
        .OrderByDescending(x => x.AvgTip)
        .FirstOrDefault();

    if (topTip != null)
    {
        Console.WriteLine($"1. PULocationID with highest average tip: {topTip.LocationId} (Avg: {topTip.AvgTip:F2})");
    }

    
    var longestDistance = context.AppTrips
        .OrderByDescending(t => t.TripDistance)
        .Take(100)
        .ToList();

    Console.WriteLine($"2. Top 100 longest trips by distance: Found {longestDistance.Count} records. Longest: {longestDistance.FirstOrDefault()?.TripDistance} miles");

    
    var longestTime = context.AppTrips
    .Select(t => new
    {
        t.Id,
        t.TpepPickupDatetime,
        t.TpepDropoffDatetime,
        DurationMinutes = SqlServerDbFunctionsExtensions.DateDiffMinute(EF.Functions, t.TpepPickupDatetime, t.TpepDropoffDatetime)
    })
    .OrderByDescending(t => t.DurationMinutes)
    .Take(100)
    .ToList();

    Console.WriteLine($"3. Top 100 longest trips by time, Longest: {longestTime.FirstOrDefault()?.DurationMinutes}");


    int searchId = 161;
    var locationSearch = context.AppTrips
        .Where(t => t.PULocationID == searchId)
        .Count();

    Console.WriteLine($"4.Found {locationSearch} trips for PULocationID {searchId}");
}

WriteToDatabase();

DoAnalyticQueries();