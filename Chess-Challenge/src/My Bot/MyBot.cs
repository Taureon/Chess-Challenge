using ChessChallenge.API;
using System;
using System.Collections.Generic;

public class MyBot : IChessBot {
    public Move Think(Board board, Timer timer) {

        //get all available moves and filter out king moves
        Move[] moves = board.GetLegalMoves();
        List<Move> kingMoves = new();
        foreach (Move move in moves) {
            if (move.MovePieceType == PieceType.King) {
                kingMoves.Add(move);
            }
        }

        //if no available king moves, move a random piece
        if (kingMoves.Count == 0) {
            System.Random rng = new();
            return moves[rng.Next(moves.Length)];
        }

        //find the king move that moves us closest to the enemy king
        Move closest = Move.NullMove;
        double dist = double.PositiveInfinity;
        Square goal = board.GetKingSquare(!board.IsWhiteToMove);
        foreach (Move move in kingMoves) {
            if (closest == Move.NullMove) {
                closest = move;
                continue;
            }

            //if closer than current, replace
            double distCurrent = Math.Sqrt(Math.Pow(Convert.ToDouble(goal.File - closest.TargetSquare.File), 2) + Math.Pow(Convert.ToDouble(goal.Rank - closest.TargetSquare.Rank), 2));
            if (distCurrent < dist) {
                closest = move;
                dist = distCurrent;
                continue;
            }
        }

        return closest;
    }
}