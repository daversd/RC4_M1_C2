using UnityEngine;
using System.Linq;

//10 Set as a static class, remove inheritance from MonoBehaviour
public static class CSVReader
{
    //11 Create the ReadSudokuGame. You can change the return type
    // to test the different types of output at each step
    /// <summary>
    /// Returns an <see cref="int[]"/> with a single game, organized linearly
    /// </summary>
    /// <param name="gameNumber">The index of the game to be read from the document</param>
    /// <returns>The array with each value</returns>
    public static int[] ReadSudokuGame(int gameNumber)
    {
        //12 Read the content of the file, stored in the resources folder
        string fileContent = Resources.Load<TextAsset>("Data/sudoku_subset").text;
        
        //13 Break the lines of the file into a string array 
        string[] lines = fileContent.Split('\n');
        
        //14 Use Linq (add to the using namespace) to split each line into game and solution, keeping the game
        //var allGames = lines.Select(l => l.Split(',')[0]); -> var here allows to see the output before being cast to array 
        string[] allGames = lines.Select(l => l.Split(',')[0]).ToArray();

        //15 Get the game from the desired index break it into characters
        var gameChars = allGames[gameNumber].ToCharArray();

        //16 Parse the game into an int[]
        int[] game = gameChars.Select(c => int.Parse(c.ToString())).ToArray();
        return game;
    }

    //19 Create the ReadSudokuSolution
    /// <summary>
    /// Returns an <see cref="int[]"/> with a single game, organized linearly
    /// </summary>
    /// <param name="gameNumber">The index of the game to be read from the document</param>
    /// <returns>The array with each value</returns>
    public static int[] ReadSudokuSolution(int gameNumber)
    {
        string fileContent = Resources.Load<TextAsset>("Data/sudoku_subset").text;
        string[] lines = fileContent.Split('\n');
        //Change the index that is being read to 1
        string[] allGames = lines.Select(l => l.Split(',')[1]).ToArray();
        int[] game = allGames[gameNumber].ToCharArray().Select(c => int.Parse(c.ToString())).ToArray();
        return game;
    }
}
