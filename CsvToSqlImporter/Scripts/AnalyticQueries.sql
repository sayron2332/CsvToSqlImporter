SELECT TOP 1 
    [PULocationID], 
    AVG([TipAmount]) AS AverageTip
FROM [AppTrips]
GROUP BY [PULocationID]
ORDER BY AverageTip DESC;



SELECT TOP 100 
    * FROM [AppTrips]
ORDER BY [TripDistance] DESC;


SELECT TOP 100 
    *, 
    DATEDIFF(SECOND, [TpepPickupDatetime], [TpepDropoffDatetime]) AS DurationInSeconds
FROM [AppTrips]
ORDER BY DurationInSeconds DESC;



SELECT * FROM [AppTrips]
WHERE [PULocationID] = 161;