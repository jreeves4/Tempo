Garageband on mac does not have a tap to select tempo except for in a collapsed menu when you make a new project. 

I made this very simple tempo finder to fill this need. 

This is a C# application because I wanted to learn a little C#. To run this, im using the dotnet sdk
From project directory: `dotnet run` is all you need

Arguments: 
You can pass in --length followed by an int to specify the length of your timestamp queue.
Another arg is --timeout to specify the timeout time between taps
