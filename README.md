# IBetting API
Application that retrieves betting data from an online XML source and saves it to a local database. The data is refreshed every 60 seconds to show active Events, Matches and their Bets and Odds.

## The Web API consists of two endpoints.
* **First endpoint** - api/matches - Retrieve all matches starting in the next 24 hours along with all their active Bets from type "Match Winner", "Map Advantage" or "Total Maps Played" and all their active Odds if they have such.
* **Second endpoint** - api/matches/{matchXmlId} - Retrieve a specific match according to given Id along with all of its active and past Bets and Odds.
 
## Technologies
* ASP.NET Core
* Entity Framework Core
* MS SQL Server
* Automapper
* xUnit
* Moq

## Notes
* Before starting the application execute the IBettingDbScript.sql file found in the repository to create the needed database on your local machine.
