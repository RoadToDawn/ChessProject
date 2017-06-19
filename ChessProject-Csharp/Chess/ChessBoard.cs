﻿using System;
using System.Collections.Generic;

namespace Gfi.Hiring
{
    /// <summary>
    /// Area where the game is played
    /// </summary>
    public class ChessBoard : IChessBoard
    {
        private readonly int _maxBoardXPosition;     
        private readonly int _maxBoardYPosition;
        private readonly int _minBoardXPosition;
        private readonly int _minBoardYPosition;

        /// <summary>
        /// Coordinates used to indicate when a piece is not on the board.
        /// </summary>
        public static readonly int OffBoardCoordinate = -1;


        /// <summary>
        /// Game configuration
        /// </summary>
        private GameSettings _settings;

        /// <summary>
        /// Array of game squares.  Empty squares should contain null
        /// </summary>
        private IChessPiece[,] _pieces;

        public int Width {  get { return _settings.BoardWidth; } }
        public int Height { get { return _settings.BoardHeight; } }

        /// <summary>
        /// Number of turns since game start.
        /// </summary>
        public int CurrentTurn { get; private set; }

        private List<IChessPiece>[,] _piecesOnBoard; //int[type, color]
        
        public ChessBoard ()
        {
            InitBoard();

            _maxBoardXPosition = _settings.BoardWidth - 1;
            _maxBoardYPosition = _settings.BoardWidth - 1;
            _minBoardXPosition = 0;
            _minBoardYPosition = 0;
        }

        private void InitBoard()
        {
            // default settings for now
            _settings = new GameSettings();

            _pieces = new IChessPiece[_settings.BoardWidth, _settings.BoardHeight];

            _piecesOnBoard = new List<IChessPiece>[(int)PieceType.Count, (int)PieceColor.Count];
            for (int type = 0; type < (int)PieceType.Count; ++type)
            {
                for (int color = 0; color < (int)PieceColor.Count; ++color)
                {
                    _piecesOnBoard[type, color] = new List<IChessPiece>();
                }
            }

            CurrentTurn = 0;
        }

        /// <summary>
        /// Places a piece on the board
        /// </summary>
        /// <param name="piece"></param>
        /// <param name="xCoordinate"></param>
        /// <param name="yCoordinate"></param>
        /// <returns></returns>
        public bool AddPiece(IChessPiece piece, int xCoordinate, int yCoordinate)
        {
            if (!IsLegalBoardPosition(xCoordinate, yCoordinate))
            {
                RemoveFromBoard(piece);
                return false;
            }
            if (_piecesOnBoard[(int)PieceType.Pawn, (int)piece.Color].Count == _settings.MaxPawnsPerSide)
            {
                RemoveFromBoard(piece);
                return false;
            }

            if(_pieces[xCoordinate, yCoordinate] == null)
            {
                _pieces[xCoordinate, yCoordinate] = piece;
                piece.XCoordinate = xCoordinate;
                piece.YCoordinate = yCoordinate;
                _piecesOnBoard[(int)PieceType.Pawn, (int)piece.Color].Add(piece);
                return true;
            }
            else
            {
                RemoveFromBoard(piece);
                return false;
            }
        }

        /// <summary>
        /// Checks whether a position lies in the board coordinate system.
        /// </summary>
        /// <param name="xCoordinate"></param>
        /// <param name="yCoordinate"></param>
        /// <returns></returns>
        public bool IsLegalBoardPosition(int xCoordinate, int yCoordinate)
        {
            return !(xCoordinate < _minBoardXPosition || xCoordinate > _maxBoardXPosition ||
                yCoordinate < _minBoardYPosition || yCoordinate > _maxBoardYPosition);
        }

        private void RemoveFromBoard(IChessPiece piece)
        {
            piece.XCoordinate = OffBoardCoordinate;
            piece.YCoordinate = OffBoardCoordinate;
        }
        
        /// <summary>
        /// Tries to fetch the piece at the specified coordinates
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="piece"></param>
        /// <returns></returns>
        public bool TryGetPieceOn(int x, int y, out IChessPiece piece)
        {
            if(!IsLegalBoardPosition(x, y))
            {
                piece = null;
                return false;
            }
            piece = _pieces[x, y];
            return (piece != null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xStart"></param>
        /// <param name="yStart"></param>
        /// <param name="xEnd"></param>
        /// <param name="yEnd"></param>
        /// <returns></returns>
        public bool IsStraightClearPathBetween(int xStart, int yStart, int xEnd, int yEnd)
        {
            if(!IsLegalBoardPosition(xStart, yStart))
            {
                return false;
            }

            if (!IsLegalBoardPosition(xEnd, yEnd))
            {
                return false;
            }

            int minX = Math.Min(xStart, xEnd);
            int maxX = Math.Max(xStart, xEnd);
            int minY = Math.Min(yStart, yEnd);
            int maxY = Math.Max(yStart, yEnd);

            if (xStart == xEnd)
            {
                return CheckVertical(minY+1, maxY-1, xStart);
            }
            if (yStart == yEnd)
            {
                return CheckHorizontal(minX+1, maxX-1, yStart);
            }
            if (xStart - xEnd == yStart - yEnd)
            {
                return CheckXEqualsY(minX+1, minY+1, maxX-1, maxY-1);
            }
            if (xStart - xEnd == yEnd - yStart)
            {
                return CheckXEqualsMinusY(minX+1, maxY-1, maxX-1, minY+1);
            }

            return false;
        }

        #region Check Clear paths on straight lines
        private bool CheckHorizontal(int left, int right, int y)
        {
            for(int x = left; x < right; ++x)
            {
                if(_pieces[x, y] != null)
                {
                    return false;
                }
            }

            return true;
        }
        private bool CheckVertical(int bottom, int top, int x)
        {
            for (int y = bottom; y < top - 1; ++y)
            {
                if (_pieces[x, y] != null)
                {
                    return false;
                }
            }

            return true;
        }
        private bool CheckXEqualsY(int left, int bottom, int right, int top)
        {
            for (int x = left; x < right - 1; ++x)
            {
                for (int y = bottom; y < top - 1; ++y)
                {
                    if (_pieces[x, y] != null)
                    {
                        return false;
                    }
                }
            }

            return true;
        }
        private bool CheckXEqualsMinusY(int left, int top, int right, int bottom)
        {
            for (int x = left; x < right - 1; ++x)
            {
                for (int y = top; y > bottom; --y)
                {
                    if (_pieces[x, y] != null)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Repositions a piece on the board.  Any pieces in the endpoint will be removed from the board
        /// </summary>
        /// <param name="move"></param>
        public void UpdateBoard(Move move)
        {
            if(move.Piece == null)
            {
                return;
            }

            if(!IsLegalBoardPosition(move.StartingX , move.StartingY))
            {
                RemoveFromBoard(move.Piece);
                return;
            }

            if (!IsLegalBoardPosition(move.EndingX, move.EndingY))
            {
                RemoveFromBoard(move.Piece);
                return;
            }

            _pieces[move.StartingX, move.StartingY] = null;
            IChessPiece captured = _pieces[move.EndingX, move.EndingY];
            _pieces[move.EndingX, move.EndingY] = move.Piece;

            if(captured != null)
            {
                RemoveFromBoard(captured);
            }
        }

        #endregion
    }
}
