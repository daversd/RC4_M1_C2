using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class EnvironmentManager : MonoBehaviour
{
    //08 Create the VoxelGrid field in the Environment
    VoxelGrid _voxelGrid;

    private bool _solutionInitialized;

    void Start()
    {
        //09 Create the basic VoxelGrid
        //_voxelGrid = new VoxelGrid(new Vector3Int(9, 9, 9), transform.position, 1);

        //17 Read the game from the csv file, printing the outputs
        //var readGame = CSVReader.ReadSudokuGame(3);
        //print(readGame);
        
        //18 Print the final result of the game reader
        //foreach (var item in CSVReader.ReadSudokuGame(3))
        //{
        //    print(item);
        //}
        
        //33 Create a Sudoku tower
        _voxelGrid = new VoxelGrid(10, transform.position, 1f);
        
        //49 Solve each level of the tower
        //for (int i = 0; i < _voxelGrid.GridSize.y; i++)
        //{
        //    Solve(i);
        //}
        //StartCoroutine(AnimatedSolution());
    }

    //50 Copy Solve to an IEnumerator
    /// <summary>
    /// Animates the solution of the tower
    /// </summary>
    /// <returns>Each step of the solution</returns>
    IEnumerator AnimatedSolution()
    {
        //51 Add the Y iteration to the begining
        for (int y = 0; y < _voxelGrid.GridSize.y; y++)
        {
            for (int x = 0; x < _voxelGrid.GridSize.x; x++)
            {
                for (int z = 0; z < _voxelGrid.GridSize.z; z++)
                {
                    SudokuVoxel sVoxel = (SudokuVoxel)_voxelGrid.Voxels[x, y, z];
                    if (sVoxel.State == 0)
                    {
                        for (int i = 1; i < 10; i++)
                        {
                            if (Possible(sVoxel, i))
                            {
                                sVoxel.ChangeState(i);
                                
                                //52 Add yield return after changing the state
                                yield return new WaitForSeconds(0.1f);
                                
                                //53 Start recursion with Coroutine
                                StartCoroutine(AnimatedSolution());
                                
                                if (GameAsList(y).Count(v => v.State == 0) != 0)
                                {
                                    sVoxel.ChangeState(0);
                                    
                                    //54 Add yield return after backtracking
                                    yield return new WaitForSeconds(0.1f);
                                }
                            }
                        }
                        //55 Switch return to yield return
                        yield return new WaitForSeconds(0.1f);
                    }
                }

            }
            //56 Switch return to yield return
            yield return new WaitForSeconds(0.15f);
        }
    }

    //38 Create the Solve method for a single layer
    /// <summary>
    /// Solves the a layer(game) of the tower
    /// </summary>
    /// <param name="y">The y coordinate of the game to be solved</param>
    private void Solve(int y)
    {
        //39 Iterate through X and Z indexes of layer Y
        for (int x = 0; x < _voxelGrid.GridSize.x; x++)
        {
            for (int z = 0; z < _voxelGrid.GridSize.z; z++)
            {
                //40 Get the SudokuVoxel to be evalauted, casting from Voxel
                SudokuVoxel sVoxel = (SudokuVoxel)_voxelGrid.Voxels[x, y, z];
                
                //41 Test if state of SudokuVoxel is 0 (empty cell)
                if (sVoxel.State == 0)
                {
                    //42 Try to change to the numbers between 1 and 9
                    for (int i = 1; i < 10; i++)
                    {
                        if (Possible(sVoxel, i))
                        {
                            //43 If it is possible, change it
                            sVoxel.ChangeState(i);

                            //44 Initiate the process again, moving to the next empty cell, recursively
                            Solve(y);

                            //45 This step is only reached if the solution succedded or failed
                            //Count the amount of empty spaces to see if solution succeeded
                            if (GameAsList(y).Count(v => v.State == 0) != 0)
                            {
                                //47 If solution failed, change the state of this voxel back to 0, backtracking
                                sVoxel.ChangeState(0);
                            }
                        }
                    }
                    //48 Exit method if all numbers have been tested and failed
                    return;
                }
            }
        }
        return;
    }

    //34 Create the method to evaluate if a SudokuVoxel can be changed to a value
    /// <summary>
    /// Evaluates if a <see cref="SudokuVoxel"/> can be changed to a specific state
    /// </summary>
    /// <param name="sVoxel">The <see cref="SudokuVoxel"/> to be evaluated</param>
    /// <param name="test">The value to be tested</param>
    /// <returns>A bool representing if the movement is valid or not</returns>
    private bool Possible(SudokuVoxel sVoxel, int test)
    {
        //35 Check in row (X)
        for (int i = 0; i < 9; i++)
        {
            SudokuVoxel rowVoxel = (SudokuVoxel)_voxelGrid.Voxels[i, sVoxel.Index.y, sVoxel.Index.z];
            if (rowVoxel.State == test)
            {
                return false;
            }
        }

        //36 Check in column (Z)
        for (int i = 0; i < 9; i++)
        {
            SudokuVoxel columnVoxel = (SudokuVoxel)_voxelGrid.Voxels[sVoxel.Index.x, sVoxel.Index.y, i];
            if (columnVoxel.State == test)
            {
                return false;
            }
        }

        //37 Check in 3x3 cell
        //Starting indexes to be evaluated
        int x0;
        int z0;

        //Define starting index in X
        if (sVoxel.Index.x < 3)
        {
            x0 = 0;
        }
        else if (sVoxel.Index.x < 6)
        {
            x0 = 3;
        }
        else
        {
            x0 = 6;
        }

        //Define starting index in Z
        if (sVoxel.Index.z < 3) { z0 = 0; }
        else if (sVoxel.Index.z < 6) { z0 = 3; }
        else { z0 = 6; }

        for (int x = x0; x < x0 + 3; x++)
        {
            for (int z = z0; z < z0 + 3; z++)
            {
                SudokuVoxel cellVoxel = (SudokuVoxel)_voxelGrid.Voxels[x, sVoxel.Index.y, z];
                if (cellVoxel.State == test)
                {
                    return false;
                }
            }
        }

        return true;
    }

    //46 Create the method to return a game as a list
    /// <summary>
    /// Gets a level of the Sudoku tower (game) as a list
    /// </summary>
    /// <param name="y">The level to be used</param>
    /// <returns>The <see cref="SudokuVoxel"/> as a list</returns>
    private List<SudokuVoxel> GameAsList(int y)
    {
        List<SudokuVoxel> result = new List<SudokuVoxel>();
        for (int x = 0; x < 9; x++)
        {
            for (int z = 0; z < 9; z++)
            {
                result.Add((SudokuVoxel)_voxelGrid.Voxels[x, y, z]);
            }
        }

        return result;
    }

    //57 Create method to initate solution
    /// <summary>
    /// Initiates the solution of the tower externally
    /// </summary>
    public void InitiateSolution()
    {
        //Check if solution has already been initialized to avoid conflicts
        if (!_solutionInitialized)
        {
            _solutionInitialized = true;
            StartCoroutine(AnimatedSolution());
        }
    }
}
