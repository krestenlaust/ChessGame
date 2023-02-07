# ChessGame
Chess-engine designed and implemented by my friends, Cogo and Patrick, and I. Features AI opponent (using minimax), Lichess integration and multiplayer-on-LAN.

# How to play
The program has several different gamemodes and playmodes.

## Gamemodes
A gamemode is defined by a change in the rules and/or the starting position of the board. Technically, a gamemode is implemented by deriving from `Gamemode`-class, where custom logic can be implemented by overriding the virtual methods, `StartTurn`, `UpdateGameState`, `ValidateMove`. Currently, no gamemodes make use of overriding the methods.

### Classic
This is the normal gamemode of chess. All normal rules apply.

There are a couple rules that still need to be implemented, 'Threefold repetition' (#13), '50-move rule' (#17) and forced draw in dead positions (#18).

![Starting position of classic gamemode](Doc/gamemode-classic.png)

### Horde
Horde plays the same way as regular chess, but white plays only as pawns.

![Starting position of horde gamemode](Doc/gamemode-horde.png)

### Tiny
Tiny isn't an official gamemode, but a gamemode made to test the bot in positions with fewer moves, but still realistic situations. The queen and one bishop has been removed to make room on a 6x8 board.

![Starting position of tiny gamemode](Doc/gamemode-tiny.png)

### Test gamemodes
The remaining available gamemodes are used for testing, and aren't really playable.

Pawn test:
![Starting position of pawntest gamemode](Doc/gamemode-pawntest.png)

## Playmodes
Playmodes are the ways to play a gamemode, locally, remotely or maybe with a bot. Technically, a playmode is defined by a class inheriting from `Player`. The only method that is virtual on this class is `TurnStarted` which expects the class to call Chessboard for the game to continue.

### Hot-seat
This is the most basic way to play. It works by both players selecting 'local', and then they both make moves by selecting a piece with the UI. The boards rotation is currently fixed, (maybe the board is going to flip every move in the future when #11 has been resolved).

![Setup of hotseat playmode](Doc/playmode-setup-hotseat.png)

### Networked / LAN
(Make good description here)

Player 1 setup:
![Player 1 (host) setup of networked playmode](Doc/playmode-setup-host-networked)
Player 2 setup:
![Player 2 (client) setup of networked playmode](Doc/playmode-setup-client-networked)

### Person vs. Bot
(Make good description here)

### Bot vs. Bot
(Make good description here)

### Lichess API
(Remember how the Lichess player-class works and make good description here)

(insert image of setup, both Lichess documentation and ChessForms UI)



