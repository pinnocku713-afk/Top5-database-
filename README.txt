Top5 starter for Friday 9 Oct: your first database (EF Core)
=============================================================
CWEB2302 Web Development II

All of Lab 4 from Tuesday 6 Oct (the checkpoint after "Your turn 3"):
  - Dish model with [Required], [StringLength], [Range] and [Display]
  - GET /menu/new shows the Add form, POST /menu/add saves it and redirects
  - Errors under each box, checked in the browser and on the server
  - An IsSpicy checkbox, and an Edit page for each dish

The dishes still live in a static List in MenuController.cs, so a dish
you add is gone when you stop the app. Today that list moves into a
SQL Server database.

  1. Unzip it into Documents (next to your own project, not on top of it).
  2. Open Top5.csproj in Visual Studio and press Run (F5), or run: dotnet watch
  3. Put your own theme and dishes back in.

The project targets .NET 8 and will also run on any newer .NET installed.
