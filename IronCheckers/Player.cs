using IronEngine;

namespace IronCheckers;

/// <summary>
/// Represents a player in a game of Checkers.
/// </summary>
public class Player : Actor
{
    #region Public Fields/Properties
    public string Name { get; private set; }

    public Action<Piece> PieceTaken;

    /// <summary>
    /// The number of remaining pieces for the player.
    /// </summary>
    /// <remarks>
    /// This property represents the number of active pieces that the player has on the board.
    /// The count is calculated based on the number of elements in the ActivePieces list.
    /// </remarks>
    public int RemainingPieces => activePieces.Count;
    
    #endregion

    #region Private Fields
    /// <summary>
    /// The currently active pieces on the board
    /// </summary>
    private List<Piece> activePieces;

    /// <summary>
    /// Pieces that have been removed from the board
    /// </summary>
    private List<Piece> removedPieces;

    private GameInstanceManager gameManager;
    #endregion


    #region Public Functions
    
    /// <summary>
    /// Represents a player in a game of Checkers.
    /// </summary>
    public Player(string name, GameInstanceManager gameInstanceManager)
    {
        Name = name;
        gameManager = gameInstanceManager;
        activePieces = new List<Piece>();
        removedPieces = new List<Piece>();
    }
    
    /// <summary>
    /// Adds a piece to the list of the player's active pieces.
    /// </summary>
    /// <param name="piece">The piece to add.</param>
    public void AddPiece(Piece piece)
    {
        activePieces.Add(piece);
    }

    /// <summary>
    /// Retrieves a collection of available actions for the player.
    /// </summary>
    /// <returns>A collection of available actions represented by Func(bool). </returns>
    public IEnumerable<Func<bool>> GetAvailableActions()
    {
        foreach (var piece in activePieces)
        {
            var actions = piece.GetAvailableActions();
            if (actions != null)
            {
                foreach (var action in actions)
                {
                    yield return action;
                }
            }
        }
    }

    /// <summary>
    /// Removes a piece from the player's active pieces and adds it to the removed pieces list.
    /// </summary>
    /// <param name="piece">The piece to be removed.</param>
    public void RemovePiece(Piece piece)
    {
        removedPieces.Add(piece);
        activePieces.Remove(piece);
        
        PieceTaken?.Invoke(piece);

        if (RemainingPieces == 0)
        {
            GameInstanceManager.EndGame(this);
        }
    }

    /// <summary>
    /// Gets the piece at a specified tile.
    /// </summary>
    /// <param name="tile">The tile at which to get the piece.</param>
    /// <returns>The piece at the specified tile. Returns null if there is no piece at the tile.</returns>
    /*public bool TryGetPieceAt(Tile tile, out Piece piece)
    {
        return activePieces.FirstOrDefault(p => p.Tile == tile);
    }*/

    /// <summary>
    /// Retrieves all active pieces belonging to the player.
    /// </summary>
    /// <returns>An IEnumerable of Piece objects representing the active pieces.</returns>
    public IEnumerable<Piece> GetAllPieces()
    {
        return activePieces;
    }

    /// <summary>
    /// Moves a given piece to a specified tile.
    /// </summary>
    /// <param name="piece">The piece to be moved.</param>
    /// <param name="to">The target tile to move the piece to.</param>
    public void MovePiece(Piece piece, Tile to)
    {
        if (piece != null && to != null)
        {
            piece.Move(to);
        }
    }

    /// <summary>
    /// Destroys all pieces belonging to the player.
    /// </summary>
    public void DestroyAllPieces()
    {
        foreach (var piece in activePieces)
        {
            piece.Destroy();
        }

        activePieces.Clear();
    }

    /// <summary>
    /// Retrieves all pieces of a specified type owned by the player.
    /// </summary>
    /// <typeparam name="T">The type of the pieces to retrieve.</typeparam>
    /// <returns>An IEnumerable collection of pieces of type T.</returns>
    public IEnumerable<Piece> GetPiecesOfType<T>() where T : Piece
    {
        return activePieces.OfType<T>();
    }
    #endregion

    #region Private Functions

    //temp

    #endregion
}