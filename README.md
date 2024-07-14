Game Design:

![image](https://github.com/user-attachments/assets/f59f5250-dff7-4b0d-81b9-6ccd748f6867)

The panel on the side allows you to change the panel colors, the color of your pieces and the player against you (player or AI):

![image](https://github.com/user-attachments/assets/9124660a-351f-4a89-9e7a-95938ff5651a)

When you start playing, you have 2 options:
- Preview strategy (Click on piece to check available moves):

![image](https://github.com/user-attachments/assets/9d47409d-b833-4f55-87d3-c392fa9d46a9)

- Move strategy (Click on new location to move):

![image](https://github.com/user-attachments/assets/1c33be58-4222-4602-a662-35a6d671feed)

Then the turn switches and only the other player/AI can move:

![image](https://github.com/user-attachments/assets/26a73d94-6072-4d18-ba7c-319cbd6abbc5)

The game includes the following options:
- Castling:

![image](https://github.com/user-attachments/assets/88922a4a-cd92-42cb-b6a3-92ffa2252fd4)

- Promotion:

![image](https://github.com/user-attachments/assets/4fe64503-b015-43ef-9601-205ded5b0136)
![image](https://github.com/user-attachments/assets/7db4a1aa-ec45-4400-bd92-54de85c8bcc7)

- Check:

![image](https://github.com/user-attachments/assets/0763dd61-e091-4904-b9d0-6d2afa6c466a)

with the following moves available when the king is under check:
1) Move a piece to block the path between the piece checking (queen in this case) and the king. In case of a knight, this move doesn't apply

![image](https://github.com/user-attachments/assets/cdb54755-724a-4f6b-bd6c-9335a840dacb)

2) Move the king in a position not attacked by any piece:

![image](https://github.com/user-attachments/assets/a3315d77-56aa-48e3-a17e-900b6afcf86c)

3) Taking the checking piece:

![image](https://github.com/user-attachments/assets/0995158a-7f84-43df-9ad4-1f68e42a2e62)

4) No other piece is allowed to move

- Checkmate:

![image](https://github.com/user-attachments/assets/1432831b-1c77-4235-ab34-c5a18f18362d)

Some additional options that can be implemented:
- En passant
- Discovered check
- Double check
Also note that in some special cases, the game still have some glitches and not all options are mapped from the AI
DISCLAIMER: Source code for AI is from the repo: https://github.com/official-stockfish/Stockfish . In this repo, the link between the AI move and our code is done.
