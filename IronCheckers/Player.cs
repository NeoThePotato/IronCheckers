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

    public int RemainingPieces => MyPieces.Count();

    public bool HasPiecesLeft => MyPieces.Any();
    #endregion

    #region Private Fields

    private IEnumerable<Piece> MyPieces => MyObjects.Where(o => o is Piece).Select(o => o! as Piece);

    /// <summary>
    /// Pieces that have been removed from the board
    /// </summary>
    private List<Piece> removedPieces;
    #endregion

    #region Public Functions
    
    /// <summary>
    /// Represents a player in a game of Checkers.
    /// </summary>
    public Player(string name)
    {
        Name = name;
        removedPieces = new List<Piece>();
    }

    /// <summary>
    /// Removes a piece from the player's active pieces and adds it to the removed pieces list.
    /// </summary>
    /// <param name="piece">The piece to be removed.</param>
    public void RemovePiece(Piece piece)
    {
        removedPieces.Add(piece);
        
        PieceTaken?.Invoke(piece);
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
    /// Retrieves all pieces of a specified type owned by the player.
    /// </summary>
    /// <typeparam name="T">The type of the pieces to retrieve.</typeparam>
    /// <returns>An IEnumerable collection of pieces of type T.</returns>
    public IEnumerable<Piece> GetPiecesOfType<T>() where T : Piece
    {
        return MyPieces.OfType<T>();
    }
	#endregion

	public override string ToString() => Name;
}