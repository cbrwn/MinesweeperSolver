# MinesweeperSolver
Silly C# Minesweeper bot - made to work with [minesweeperonline.com](http://minesweeperonline.com/)

It's able to find the Minesweeper window (if it's completely visible), detect the row/column count and start solving.

It takes control of the mouse, and doesn't stop until it's complete or the window can't be found. This means it can be stopped by ctrl+tabbing to another tab, or ctrl+w to close the tab.

Right now it goes through guaranteed bomb places, flags them and clears satisfied bomb counts.

When it runs out of guaranteed moves, it randomly chooses a square and clicks it. It's super bad, and I'll be making it better with actual guessing strategy.

When it fails, it takes a screenshot so you can see how far it got.
