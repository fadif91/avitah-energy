# avitah-energy
Note that I decided to group the responses by key so I added a new layer in the proto which is the grouping
This is because I thought that when we search by one argument we can get results for different Quantity, Category etc and that I should assign the time series to each.
Else we don't know which time series is for which key, also there would be confusion what Quantity to choose for the object if we end up having two.
This won't happen with this data set when joining all 3 collections but given the structure I thought would be best.

Tried to use the group by in the IQueryable, though I had some exceptions regarding the conversion to linq. Looks like that conversion doesn't work yet properly in Mongo

Also the epoch time in the given     
quantity: gas,
    country: UK,
    dateTime: 2019-09-01 (29968531200000) ----> should be 1567296000000 ??
    
   I saw this represents the year 2919, and not 2019 at least using the BsonUtils library and converting using an online converter.
   
   I also ommitted the other fields in data since we are interested in the "value" field for the time series, but can be added easily:
   - Another property/BsonElement in the Data class, along with the appropriate field in the proto, with another equality filter in the query.
