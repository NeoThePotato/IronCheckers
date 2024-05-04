//CHESS GAME CLIENT//

using System.Drawing;
using IronEngine;
using IronCheckers;

internal static class Program
{
    public static void Main()
    {
        Console.WriteLine("White Player, please enter your name:");
        var whitePlayerName = Console.ReadLine() ?? "White Player";
        Console.WriteLine("Black Player, please enter your name:");
        var blackPlayerName = Console.ReadLine() ?? "Black Player";
        Console.WriteLine("Let the games begin!");

        GameInstanceManager.StartNewGame(whitePlayerName, blackPlayerName);
    }
}

/*class GameManager
{
    private int currentTurnIndex;
    
    public void MakeMove()
    {
        // code for making a move
    }
    public void EndGame()
    {
        // code for ending the game
    }
    public void StartGame()
    {
        // code for starting the game
    }
    public void RestartGame()
    {
        // code for restarting the game
    }
    public void UndoMove()
    {
        // code for undoing a move
    }

    public void SelectPawn()
    {
        //code for clicking on and selecting a pawn
    }
    public void DeSelectPawn()
    {
        //code for deselecting the currently selected pawn
    }
}

class BoardManager : TileMap
{
    private Pawn? selectedPawn;
    private PawnAction? queuedAction;
    
    public void DisplayActionableTiles(PawnAction action, Pawn performingPawn)
    {
        foreach (var tile in this)
        {
            TileActionabilityPreview preview = action.GetTileActionability(tile);
            //tile.DisplayActionabilityPreview(preview); //Renderer stuff
        }
    }
    
    //this was entirely guessed by AI, impressive
    public void OnTileSelected(Tile tile)
    {
        //TODO: Add a case for cancelling. Perhaps right clicking will un-queue the current action and de-select the current pawn.
        
        if (selectedPawn != null && queuedAction != null)
        {
            if (queuedAction.IsActionExecutableHere(tile))
            {
                selectedPawn.OnActionableTileSelected(tile, queuedAction);
                
                //Mayyyyyyybe we'll add multi-select actions which won't cancel themselves after 1 execution.
                selectedPawn = null;
                queuedAction = null;
            }
        }

        else
        {
            // When the player selects a tile, check if it is actionable
            if (tile.HasObject)
            {
                // Get the pawn object on the tile
                var pawn = tile.Object as Pawn;
                if (pawn != null)
                {
                    //enables switching of pawns
                    selectedPawn?.OnPawnDeSelected();
                    selectedPawn = pawn;
                    pawn.OnPawnSelected();
                }
            }
        }
    }

    public BoardManager(int sizeJ, int sizeI) : base(sizeJ, sizeI)
        {
            // initialize the pawns and actions
            selectedPawn = null;
            queuedAction = null;
        }
}

/// <summary>
/// tileable = takes a spot on the tilemap
/// moveable = can move
/// controllable = can be chosen & controlled/moved by the player
/// </summary>
class Pawn : TileObject
{
    private BoardManager myBoard;
    private List<PawnAction> Actions; // Not 100% sure on the implementation,
                                      // But I think that mayyyyybe we'll want to open an actions window with available actions to choose from like in a JRPG
    
    //private VisualObject ActionsBar // maybe thinking too far here, but perhaps we'll get there
    
    private Pawn(Tile tile) : base(tile)
    {
        myBoard = base.TileMap as BoardManager; //possible null reference assignment?
        foreach (var action in Actions)
        {
            action.OnActionSelected += OnActionSelected;
        }
    }
                                      
    // When the player selects the pawn, display available actions (or default to "Move" for chess/checkers units)
    public void OnPawnSelected()
    {
        if (Actions.Count > 1)
        {
            //Renderer: DisplayActionSelection(ActionsBar); //Opens up the UI container from which you can choose all available actions
        }
        else
        {
            OnActionSelected(Actions[0]);
        }
    }

    public void OnPawnDeSelected()
    {
        
    }

    /// <summary>
    /// When an action is selected, color/visualize all actionable tiles, according to their ActionType
    /// (Blue: "You can move there", Red: "You can't move there", Grey: "Out of range", etc.)
    /// You then await an input from the player, either tile selection or action deselection
    /// </summary>
    /// <param name="Action"></param>
    /// <param name="tile"></param>
    public void OnActionSelected(PawnAction Action)
    {
        
    }

    /// <summary>
    /// Called when input is received to execute the action on a tile
    /// </summary>
    /// <param name="tile"></param>
    /// <param name="action"></param>
    public void OnActionableTileSelected(Tile tile, PawnAction action)
    {
        //needs further implementation on how the action affects this pawn
        action.Execute(tile, this);
    }

    public override IEnumerable<Func<bool>>? GetAvailableActions()
    {
        throw new NotImplementedException();
    }
}

abstract class PawnAction
{
    public TileSelectionParameters tileSelectionParameters; //The parameters via which you filter the Actionable Tiles

    public Dictionary<int, TileActionabilityPreview> TilePreviews; //What to display at each level of tile actionability.

    public Action<PawnAction> OnActionSelected;
    public Action<PawnAction, Tile> OnActionExecuted;

    private PawnAction(TileSelectionParameters tileSelectionParameters, Action<PawnAction> onActionSelected)
    {
        this.tileSelectionParameters = tileSelectionParameters;
        TilePreviews = new Dictionary<int, TileActionabilityPreview>();
        OnActionSelected = onActionSelected;
    }

    public virtual void OnSelection() //select the action on the UI (skipped if only 1 action avabilable, such as 'Move' in chess)
    {
        OnActionSelected(this);
        // Renderer: Close Selection Window
    }
    
    /// <summary>
    /// (For example, in an RPG, when selecting an "Attack" action:
    /// The 'ValidTile' will be applied to tiles you can attack,
    /// The 'MixedTile' will be applied to tiles you can attack but with a drawback (perhaps the enemy is resistant, or you have to pass through hazardous terrain on your way there),
    /// And the 'InvalidTile' will be applied to tiles you cannot attack, due to being out of reach or not being a valid target for the attack.
    /// </summary>
    /// <param name="tile"></param>
    /// <returns></returns>
    public virtual TileActionabilityPreview GetTileActionability(Tile tile)
    {
        return TilePreviews[tileSelectionParameters.GetTileActionabilityRating(tile)];
    }

    public bool IsActionExecutableHere(Tile tile)
    {
        return GetTileActionability(tile).CanExecute;
    }
    public abstract bool CanExecute(Tile tile);


    /// <summary>
    /// Execute the action
    /// </summary>
    public abstract void Execute(Tile selectedTile, Pawn performingPawn);
    
    
    
    /// <summary>
    /// Executes the action on multiple tiles (perhaps will be need for an AoE effect)
    /// </summary>
    /// <param name="selectedTiles"></param>
    //public abstract void Execute(List<Tile> selectedTiles, Pawn performingPawn);
}

/// <summary>
/// This is the "Answer" you get for every tile when you filter it through the TileSelectionParameters.
/// This struct contains the visual representation of each tile depending on its actionability.
/// </summary>
public struct TileActionabilityPreview
{
    public Color Color { get; private set; } //The color the tile will be colored at
    public string Text { get; private set; } //the text displayed
    public bool CanExecute { get; private set; } //can the action be executed 

    public TileActionabilityPreview(Color color, string text, bool canExecute)
    {
        Color = color;
        Text = text;
        CanExecute = canExecute;
    }
}

public abstract class TileSelectionParameters
{
    public int Range;
    
    /// <summary>
    /// This is something i'm not quite sure about:
    /// I *think* that we'll want the selection parameters to assign different "Actionability Ratings" to tile
    /// only partially pass through the filter, or perhaps tiles that are actionable but with some drawback.
    /// ...
    /// this is the real tough part, and requires the creation of a flexible filtering system.
    /// 
    /// </summary>
    /// <param name="tile">The tile to set the actionability for.</param>
    /// <returns>The actionability rating of the tile.</returns>
    public abstract int GetTileActionabilityRating(Tile tile);
}*/