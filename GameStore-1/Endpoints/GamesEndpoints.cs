using GameStore_1.Dtos;

namespace GameStore_1.Endpoints;

public static class GamesEndpoints
{
private static readonly List<GameDto> games = [
    new GameDto(1, "The Legend of Zelda: Breath of the Wild", "Action-adventure", 59.99m, new DateOnly(2017, 3, 3)),
    new GameDto(2, "Super Mario Odyssey", "Platformer", 59.99m, new DateOnly(2017, 10, 27)),
    new GameDto(3, "The Witcher 3: Wild Hunt", "Action RPG", 39.99m, new DateOnly(2015, 5, 19)),
    new GameDto(4, "Dark Souls III", "Action RPG", 39.99m, new DateOnly(2016, 3, 24)),
    new GameDto(5, "Hollow Knight", "Metroidvania", 14.99m, new DateOnly(2017, 2, 24))
];

public static WebApplication MapGamesEndpoints(this WebApplication app){
// Get all games
// This endpoint retrieves all games from the list of games.
app.MapGet("/games", () => games);


// Get a game by ID
// This endpoint retrieves a game by its ID from the list of games.
app.MapGet("/games/{id}", (int id) => {
    var game = games.FirstOrDefault(g => g.Id == id);
    return game is not null ? Results.Ok(game) : Results.NotFound();
});


// Create a new game
// This endpoint creates a new game and adds it to the list of games.
app.MapPost("/games", (CreateGameDto gameDto) => {
    var newGame = new GameDto(
        Id: games.Max(g => g.Id) + 1,
        Name: gameDto.Name,
        Genre: gameDto.Genre,
        Price: gameDto.Price,
        ReleaseDate: gameDto.ReleaseDate
    );
    games.Add(newGame);
    return Results.Created($"/games/{newGame.Id}", newGame);
});

app.MapGet("/", () => "Hello Frontend😚💛!");


// Update an existing game by ID
// This endpoint updates an existing game by its ID in the list of games.
app.MapPut("/games/{id}", (int id, UpdateGameDto gameDto) => {
    var index = games.FindIndex(g => g.Id == id);
    if (index == -1) return Results.NotFound();

    var updatedGame = new GameDto(
        Id: id,
        Name: gameDto.Name,
        Genre: gameDto.Genre,
        Price: gameDto.Price,
        ReleaseDate: gameDto.ReleaseDate
    );
    
    games[index] = updatedGame;
    return Results.Ok(updatedGame);
});

// Delete a game by ID
// This endpoint deletes a game by its ID from the list of games.
// It returns a 204 No Content response if the game is successfully deleted.
app.MapDelete("/games/{id}",(int id)=>{
    var index = games.FindIndex(g => g.Id == id);
    if (index == -1) return Results.NotFound();

    games.RemoveAt(index);
    return Results.NoContent();     
});

return app;
}

}
