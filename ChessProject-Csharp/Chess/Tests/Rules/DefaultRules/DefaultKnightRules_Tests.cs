﻿using System;
using NSubstitute;
using NUnit.Framework;

namespace Gfi.Hiring {

	[TestFixture]
	public class DefaultKnightRules_Tests : UniversalRules_Tests {


		protected override IRuleSet GetRuleSet()
		{
			return new DefaultKnightRules();
		}

		[Test]
		public void Knight_RuleSet_Contains_CannotMoveHorizontallyRule()
		{
			Assert.That(RuleSetContainsRule(_ruleset, typeof(CannotMoveHorizontallyRule)), Is.True);
		}

		[Test]
		public void Knight_RuleSet_Contains_CannotMoveVerticallyRule()
		{
			Assert.That(RuleSetContainsRule(_ruleset, typeof(CannotMoveVerticallyRule)), Is.True);
		}

		[Test]
		public void Knight_RuleSet_Contains_MustMoveThreeSquaresRule()
		{
			Assert.That(RuleSetContainsRule(_ruleset, typeof(MustMoveThreeSquaresRule)), Is.True);
		}

        [Test]
        public void KnightCannotMoveThreeSquaresLeft()
        {
            var board = Substitute.For<IChessBoard>();
            var piece = Substitute.For<IChessPiece>();
            Move move = new Move(piece, 3, 0, 0, 0);
            board.IsMoveValid(move).ReturnsForAnyArgs(_ruleset.IsMoveValid(board, move));
            
            Assert.That(board.IsMoveValid(move), Is.False);
        }

        [Test]
        public void KnightCannotMoveThreeSquaresUp()
        {
            var board = Substitute.For<IChessBoard>();
            var piece = Substitute.For<IChessPiece>();
            Move move = new Move(piece, 0, 0, 0, 3);
            board.IsMoveValid(move).ReturnsForAnyArgs(_ruleset.IsMoveValid(board, move));

            Assert.That(board.IsMoveValid(move), Is.False);
        }

        [Test]
        public void KnightCannotMoveThreeSquaresDown()
        {
            var board = Substitute.For<IChessBoard>();
            var piece = Substitute.For<IChessPiece>();
            Move move = new Move(piece, 0, 3, 0, 0);
            board.IsMoveValid(move).ReturnsForAnyArgs(_ruleset.IsMoveValid(board, move));

            Assert.That(board.IsMoveValid(move), Is.False);
        }

        [Test]
        public void KnightCannotMoveThreeSquaresRight()
        {
            var board = Substitute.For<IChessBoard>();
            var piece = Substitute.For<IChessPiece>();
            Move move = new Move(piece, 0, 0, 3, 0);
            board.IsMoveValid(move).ReturnsForAnyArgs(_ruleset.IsMoveValid(board, move));

            Assert.That(board.IsMoveValid(move), Is.False);
        }
    }
}
