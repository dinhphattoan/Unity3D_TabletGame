# Unity3D Tablet Game

A demo of simple table games

## Notification

- The game doesn't require nescesary packages but it need the package name "Spline" (using latest Unity Editor will automatically install the packages, if it's not installed, you can install it manually in package manager).
- The warning encounter when running in editor is relate to new game font installed (doesn't affect to the game), typically the font doesn't fully support for all key characters so it will use some default font as replacement. Install a full support font would make it go away.
- Prev exception was the exception related to github lfs missing file problem. Make sure you have git (with lfs) installed in your system in order to clone this project.
- Download pre-built game (here)[]

## Game Guide

- Description: A simple tablet quite similar to Mario Party, where player can choose number of characters to join in, start the game, roll the dice, and win the journey.
- Map setting: You can change whatever tile you decide to buff or fail by pressing buttons that mark number on the screen. Decide number of players for round with up/down button.
- Player setting: Name your player when click on player tab and choosing unique model figure that player want to represent.

## Ingame guide:

- Roll the dice: simply press mouse down, drag it and release it to throw the dice.
- Camera focus: picking the player to spot or watch the global map.


## Game features:


- Same-Screen Multiplayer: Decide number of players to play.
- Unique model figure
- 3D Simulate dice roll
- Statistic shows when player finish the game.
- Music, background transistion.
- Game save data system

## Game rule
- All the player need to finish their journey in order to complete the game.
- The dice roll when it tilt too much doesn't count, so it will be rolled again.
- When step on the buff tile, gain one more turn, this bonus turn addup to the turn counts.
- When step on the fail tile, player fall back 3 tile, and continue to fall if they keep stepping on the same, fall on the buff tile will not activate it.
- In order the reach to end, player have to roll the dice with exact value of step they going to jump.
- May the first one finish the game first.

## Future planning
- Adding more challenges, which play significant impact and improce the game experiences
- More funky sound and character figures behavior.
- More map and model resources.
- Support co-op.
