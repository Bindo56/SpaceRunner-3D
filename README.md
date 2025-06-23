


https://github.com/user-attachments/assets/410f8807-4197-4ce2-9125-dc92ebe168c3


This project is a modular endless runner shooter built using Unity’s MonoBehaviour lifecycle and follows a clean MVC-inspired pattern:

Model – Game data and logic (e.g., player stats, scoring, inventory)

View – Visual representation and UI updates

Controller – Game flow logic, player input, and state transitions

 
Controller.cs
+ Handles joystick input for player movement.


+ Updates score every 0.5 seconds and drains energy every 1 second.


+ Fires bullets at random intervals using View.FireProjectiles.


+ Manages thruster activation, game over, restart, and UI transitions.



Model.cs

+ Stores all runtime Data Variables

View.cs

+ Displays score, orbs, hits, etc.


+ Plays pop animations on updates.


+ Moves the player ship with rotation smoothing.


+ Changes exhaust color during thruster mode.


+ Uses PoolManager to spawn bullets from guns.


PoolManager.cs

+ Manages object reuse to reduce instantiation overhead.
  
+ Pools defined with PoolTag and size

+ Can spawn bullets, metroids, and orb powerups

+ Supports random Y-axis rotation if enabled

SpawnController.cs

+ Spawns powerups and asteroids ahead of the player using weighted probability

+ Ensures dynamic obstacle variety

DespawnManager.cs

+ Keeps world clean by disabling objects that are far behind the player


Visual Particle System

+ Particle exhaust color changes with thruster

+ Text pop animations on orb/shield collection

+ Bullets and powerups all use object pooling



Key Features Summary

+ Object Pooling

+ MVC Separation

+ Score Management (Realtime + High Score Tracking)



Project demonstrates clean game architecture, efficient pooling, and responsive UI.

Ideal for mobile endless runner or space shooter genres.

Shows practical use of Unity UI, coroutines, pooling, prefab systems, and gameplay logic separation.


