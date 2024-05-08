using IronEngine;

namespace IronCheckers;

/// <summary>
/// Represents a player in a game of Checkers.
/// </summary>
public class Player : Actor
{
    #region Public Fields/Properties
    public string Name { get; private set; }

    public int RemainingPieces => MyPieces.Count();

    public bool HasPiecesLeft => MyPieces.Any();
    #endregion

    private IEnumerable<Piece> MyPieces => MyObjects.Where(o => o is Piece).Select(o => o! as Piece);
    
    /// <summary>
    /// Represents a player in a game of Checkers.
    /// </summary>
    public Player(string name)
    {
        Name = name;
	}

	public override string ToString() => Name;
}