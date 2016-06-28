# MinesweeperSolver
Silly C# Minesweeper bot - made to work with [minesweeperonline.com](http://minesweeperonline.com/)

It's able to find the Minesweeper window (if it's completely visible), detect the row/column count and start solving.

It takes control of the mouse, and doesn't stop until it's complete or the window can't be found. This means it can be stopped by ctrl+tabbing to another tab, or ctrl+w to close the tab.

---

Right now it goes through guaranteed bomb places, flags them and clears satisfied bomb counts.

When it runs out of guaranteed moves, it uses a super simple probability check to find a relatively safe square to open. This can only beat Expert ~4% of the time.

It also lets you take a screenshot of the game and/or its interpretation of the board upon failing/winning.
