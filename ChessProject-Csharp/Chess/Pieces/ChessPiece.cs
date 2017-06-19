﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gfi.Hiring {
    public class ChessPiece : IChessPiece {

        /// <summary>
        /// Rules governing behaviour of the piece
        /// </summary>
        private List<IRule> _rules;

        protected IChessBoard _chessBoard;
        
        /// <summary>
        /// Constructor without rules governng piece behaviour
        /// </summary>
        /// <param name="board"></param>
        /// <param name="type"></param>
        /// <param name="color"></param>
        public ChessPiece(IChessBoard board, PieceType type, PieceColor color)
        {
            Init(board, type, color, new IRule[0]);
        }

        /// <summary>
        /// Constructor with initial rules governing piece behaviour
        /// </summary>
        /// <param name="board"></param>
        /// <param name="type"></param>
        /// <param name="color"></param>
        /// <param name="rules"></param>
        public ChessPiece(IChessBoard board, PieceType type, PieceColor color, IRule[] rules)
        {
            Init(board, type, color, rules);
        }

        /// <summary>
        /// Which side the piece is on, Black or White
        /// </summary>
        public PieceColor Color { get; private set; }

        /// <summary>
        /// Type of Piece - Pawn, Rook, Knight, Bishop, Queen, King
        /// </summary>
        public PieceType Type { get; private set; }

        /// <summary>
        /// X Coordinate / Column Id / A-H AN
        /// </summary>
        public int XCoordinate { get; set; }

        /// <summary>
        /// Y Coordinate / Row Id / 1-8 AN
        /// </summary>
        public int YCoordinate { get; set; }

        /// <summary>
        /// Initialises the board variables for the constructors
        /// </summary>
        /// <param name="board"></param>
        /// <param name="type"></param>
        /// <param name="color"></param>
        /// <param name="rules"></param>
        private void Init(IChessBoard board, PieceType type, PieceColor color, IRule[] rules)
        {
            _chessBoard = board;
            Type = type;
            Color = color;
            XCoordinate = ChessBoard.OffBoardCoordinate;
            YCoordinate = ChessBoard.OffBoardCoordinate;
            _rules = new List<IRule>(rules);
        }

        /// <summary>
        /// Moves a piece iff the move meets all the required rules for the piece.  Returns whehter the move happened
        /// </summary>
        /// <param name="type"></param>
        /// <param name="newXCoordinate"></param>
        /// <param name="newYCoordinate"></param>
        /// <returns></returns>
        public virtual bool Move(MovementType type, int newXCoordinate, int newYCoordinate)
        {
            Move move = new Move(this, XCoordinate, YCoordinate, newXCoordinate, newYCoordinate);

            foreach (IRule rule in _rules)
            {
                if (!rule.IsMoveValid(_chessBoard, move))
                {
                    return false;
                }
            }

            XCoordinate = newXCoordinate;
            YCoordinate = newYCoordinate;

            _chessBoard.UpdateBoard(move);

            return true;
        }

        /// <summary>
        /// Add a rule to a piece.
        /// </summary>
        /// <param name="rule"></param>
        internal void AddRule(IRule rule)
        {
            _rules.Add(rule);
        }
    }
}
