﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gfi.Hiring {
    /// <summary>
    /// Checks that the move endpoint is on the board
    /// </summary>
    class MoveEndpointOnBoardRule : IRule {
        public bool IsMoveValid(IChessBoard board, Move move)
        {
            return board.IsLegalBoardPosition(move.EndingX, move.EndingY);
        }
    }
}
