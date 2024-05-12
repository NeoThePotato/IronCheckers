using IronEngine;

namespace IronCheckers;

/// <summary>
/// Represents a player in a game of Checkers.
/// </summary>
public class Player : Actor
{
    #region Public Fields/Properties
    public string Name { get; private set; }
    public byte Color { get; private set; }

    public int RemainingPieces => MyPieces.Count();

    public bool HasPiecesLeft => MyPieces.Any();
    #endregion

    private IEnumerable<Piece> MyPieces => MyObjects.Where(o => o is Piece).Select(o => o! as Piece);
    
    /// <summary>
    /// Represents a player in a game of Checkers.
    /// </summary>
    public Player(string name, byte color)
    {
        Name = name;
        Color = color;
	}

	protected override IEnumerable<ICommandAble> FilterCommandAble(IEnumerable<ICommandAble> source)
	{
        var zigZag = source.FirstOrDefault(c => c is Piece piece && piece.capturedThisTurn);
        if (zigZag != null)
        {
            yield return zigZag;
            yield break;
        }
        else
        {
            foreach (var commandable in source)
                yield return commandable;
        }
	}

	protected override void OnTurnStart()
	{
		Console.WriteLine($"{this}'s turn.");
	}

	protected override void OnTurnOver()
	{
        foreach (var piece in MyPieces)
            piece.capturedThisTurn = false;
	}

	public override string ToString() => Name;
}