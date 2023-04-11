# CosmosAPI
Create a .NET core micro service that exposes api for managing a collection of notes and stores it in cosmosdb.

a note should have the following pieces of data
1.  Id
2. Date created
2. Text of the note
3. tags

The rest api should support the following endpoints:
GET (with odata query support)
GET/{id} – fetches a note by id
PUT – updates a note
POST – creates a note
DELETE – deletes a note
 
Please use good SOLID engineering principles including use dependency injection with interfaces and unit test each layer of your code using NUnit.

Lastly, create objects to model the configuration specified in appSettings.json and inject those objects instead of passing IConfig around as a dependency.
