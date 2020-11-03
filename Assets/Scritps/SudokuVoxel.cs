using UnityEngine;

//23 Create the SudokuVoxel class, inheriting from Voxel
public class SudokuVoxel : Voxel
{
    //25 Create the State property
    public int State { get; private set; }

    //26 Create the SudokuVoxel constructor
   public SudokuVoxel(Vector3Int index, VoxelGrid voxelgrid, int state)
    {
        //Populate the inherited properties
        //Turn into protected the ones that were private
        Index = index;
        _voxelGrid = voxelgrid;
        State = state;
        _size = _voxelGrid.VoxelSize;

        //27 Create the empty GameObject for this SudokuVoxel
        _voxelGO = new GameObject($"SVoxel_{Index.x}_{Index.y}_{Index.z}");
        _voxelGO.transform.position = new Vector3(Index.x, Index.y, Index.z) * _size;
        _voxelGO.transform.localScale *= _size;

        //28 Add the MeshFilter and MeshRenderer components
        _voxelGO.AddComponent<MeshFilter>();
        _voxelGO.AddComponent<MeshRenderer>();

        //29 Assign the Mesh and the Material to the GameObject according to its state
        //var meshFilter = _voxelGO.GetComponent<MeshFilter>();
        //var meshRenderer = _voxelGO.GetComponent<MeshRenderer>();
        //if (State != 0)
        //{
        //    meshFilter.mesh = Resources.Load<Mesh>($"Prefabs/Components/{State}");
        //    meshRenderer.enabled = true;
        //    meshRenderer.material = Resources.Load<Material>($"Materials/{State}");
        //}
        //else
        //{
        //    meshRenderer.enabled = false;
        //}

        //31 Call the ChangeState method on the constructor
        ChangeState(State);
    }

    //30 Refactor the state setup into a public method
    /// <summary>
    /// Changes the state of the SudokuVoxel, assigning the correct Mesh and Material
    /// </summary>
    /// <param name="newState"></param>
    public void ChangeState(int newState)
    {
        State = newState;
        var meshFilter = _voxelGO.GetComponent<MeshFilter>();
        var meshRenderer = _voxelGO.GetComponent<MeshRenderer>();
        if (State != 0)
        {
            meshFilter.mesh = Resources.Load<Mesh>($"Prefabs/Components/{State}");
            meshRenderer.enabled = true;
            meshRenderer.material = Resources.Load<Material>($"Materials/{State}");
        }
        else
        {
            meshRenderer.enabled = false;
        }
    }
}
