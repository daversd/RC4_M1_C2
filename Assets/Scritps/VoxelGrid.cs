using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoxelGrid : MonoBehaviour
{
    //02 Create the properties of the VoxelGrid
    public Vector3Int GridSize;
    public Voxel[,,] Voxels;
    public Vector3 Origin;
    //02.1 VoxelSize with custom 
    public float VoxelSize { get; private set; }

    //04 Create the basic VoxelGrid constructor
    /// <summary>
    /// Constructor for a basic <see cref="VoxelGrid"/>
    /// </summary>
    /// <param name="size">Size of the grid</param>
    /// <param name="origin">Origin of the grid</param>
    /// <param name="voxelSize">The size of each <see cref="Voxel"/></param>
    public VoxelGrid(Vector3Int size, Vector3 origin, float voxelSize)
    {
        GridSize = size;
        Origin = origin;
        VoxelSize = voxelSize;

        //05 Create the cubeDummy prefab in Unity
        var cubePrefab = Resources.Load<GameObject>("Prefabs/cubeDummy");

        //06 Initiate the Voxel array
        Voxels = new Voxel[GridSize.x, GridSize.y, GridSize.z];

        //07 Populate the array with the new Voxels
        for (int x = 0; x < GridSize.x; x++)
        {
            for (int y = 0; y < GridSize.y; y++)
            {
                for (int z = 0; z < GridSize.z; z++)
                {
                    //C# allows functions to be broken down in lines
                    Voxels[x, y, z] = new Voxel(
                        new Vector3Int(x,y,z),
                        this,
                        cubePrefab);
                }
            }
        }
    }

    //20 Create the VoxelGrid constructor for a tower of sudoku games
    /// <summary>
    /// Constructs a VoxelGrid of Sudoku games
    /// </summary>
    /// <param name="numberOfGames">The amount of layers - games to be stacked</param>
    /// <param name="origin">The origin of the grid</param>
    /// <param name="voxelSize">The size of each <see cref="Voxel"/></param>
    public VoxelGrid(int numberOfGames, Vector3 origin, float voxelSize)
    {
        GridSize = new Vector3Int(9, numberOfGames, 9);
        Origin = origin;
        VoxelSize = voxelSize;

        Voxels = new Voxel[GridSize.x, GridSize.y, GridSize.z];

        //21 Start the population of the grid from Y, reading a new game at each level
        for (int y = 0; y < GridSize.y; y++)
        {
            //Generate a random index to be read
            int gameNumber = Random.Range(1, 1000);
            
            //Read the sudoku game
            var sudokuGame = CSVReader.ReadSudokuGame(gameNumber);
            
            //Store which index of the 81 is being read for the current voxel
            //This also resets the value at each layer
            int valueIndex = 0;

            //22 Populate the layer with SudokuVoxels
            for (int x = 0; x < GridSize.x; x++)
            {
                for (int z = 0; z < GridSize.z; z++)
                {
                    //32 Create the SudokuVoxel and store it in the Voxels array
                    Voxels[x, y, z] = new SudokuVoxel(new Vector3Int(x, y, z), this, sudokuGame[valueIndex]);
                    valueIndex++;
                }
            }
        }
    }
}
