GUI:
- displayBoard
- displayPieces
- updateBoardWithMove

Game logic:
- alternateTurns (Start with white)
- selectPiece (Then display possible moves)

- IsMoveValid
	- King is not checked, if yes then
		- Move the king
		- Block check with piece
		- Eat the piece threatening check
	- Piece is not pinned
		- Note that piece can only move along the line that the other piece pinned it
	- If castling
		- Did king move
		- Did rook move
		- Is there a piece blocking the way?
		- Is the king under check?
	- If there are pieces blocking, can the piece jump
		- All pieces can move to eat the opponent piece unless
			- Pawn: Can't eat like it moves
			- King: Can't eat a piece that is protected
	- Pawn:
		- Can't move back
		- en poiussant
		- First rank? Can move 2 
- MovePiece (Then update the GUI)
	- IsPawnOnFinalRank?
- IsCheckMate
