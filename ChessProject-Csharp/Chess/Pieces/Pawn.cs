﻿using System;

namespace Gfi.Hiring
{
    /// <summary>
    /// Instance of the Pawn class.  Pawn needs it's own class due to requirements for one of it's rules
    /// Most Types won't need their own classes - probably only Rooks and Kings (for castling and game over handling)
    /// </summary>
    public class Pawn : ChessPiece, IPawn
    {
        /// <summary>
        /// Maximum number of pawns per side.  Might need moved out into a game settings class.
        /// </summary>
        public const int Max = 8;

        /// <summary>
        /// Base Constuctor
        /// </summary>
        /// <param name="board">Board related to the game.</param>
        /// <param name="pieceColor">Which side the piece is on</param>
        public Pawn(IChessBoard board, PieceColor pieceColor) : base(board, PieceType.Pawn, pieceColor)
        {

        }

        /// <summary>
        /// Indicates the turn when the piece was first moved
        /// </summary>
        public int FirstMovedOn { get; private set; }

        /// <summary>
        /// Moves the piece.  Coordinates are only updated if the move is valid
        /// </summary>
        /// <param name="type"></param>
        /// <param name="newXCoordinate"></param>
        /// <param name="newYCoordinate"></param>
        /// <returns>True when the move was valid</returns>
        public override bool Move(MovementType type, int newXCoordinate, int newYCoordinate)
        {
            if(FirstMovedOn == 0)
            {
                FirstMovedOn = _chessBoard.CurrentTurn;
            }
            return base.Move(type, newXCoordinate, newYCoordinate);
        }

        public override string ToString()
        {
            return CurrentPositionAsString();
        }

        protected string CurrentPositionAsString()
        {
            return string.Format("Current X: {1}{0}Current Y: {2}{0}Piece Color: {3}", Environment.NewLine, XCoordinate, YCoordinate, Color);
        }

    }
}
