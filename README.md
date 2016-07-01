# MinesweeperSolver
Silly C# Minesweeper bot - made to work with [minesweeperonline.com](http://minesweeperonline.com/)

It's able to find the Minesweeper window (if it's completely visible), detect the row/column count and start solving.

It takes control of the mouse, and doesn't stop until it's complete or the window can't be found. This means it can be stopped by ctrl+tabbing to another tab, or ctrl+w to close the tab.

It also lets you take a screenshot of the game and/or its interpretation of the board upon failing/winning.

---
#Solvers
There's 2 different "solvers" that I've made - these are basically different "AIs" with different guessing techniques.

###ProbabilitySolver
This was the first solver, it goes through each uncovered square (the numbers) and assigns each nearby square a probability of it being a bomb. For example, if there was a "3" square with 2 flagged squares and 2 un-flagged squares near it, it would assign a probability of 50% to each of the un-flagged squares.

It then chooses the square with the lowest probability of being a mine and clicks it.

###SmartSolver
My second idea for a solver, much smarter than the old one. This one goes through each and every combination of bomb positions and gets a probability based on how many times a square could have been a bomb. While this is much smarter, it's also much slower. I've tried to split up the squares so it doesn't take as long to make a move, but a lot of the time there are just *tons* of possibilities to sort through. 

###Comparison
~24 hours of testing each solver (on Expert) has shown that SmartSolver is about 4x more effective than ProbabilitySolver (is that how it works?), with Smart having a ~12% win rate and Probability having a ~2-3% win rate. 

SmartSolver's downfall is its speed. I have a simple way of splitting up the border squares, however this isn't always effective and it ends up with 25+ possible mine positions. While this doesn't seem like a lot, the possibilities increase exponentially with each new square. This means a case of 30 possible mines has over 1 billion combinations. It is possible to drastically narrow down the number, however it can still take upwards of 5 minutes to find a suitable solution.
