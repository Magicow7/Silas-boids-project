# N-Body & Boids Simulations

A set of gravity and flocking simulations built in Unity for IMDM 327 (Fall 2024, instructor Myungin Lee), extended from the course's N-body starter code.

## Contents

- `NewSolarSystem.cs` — an N-body solar system simulation: bodies attract each other under Newtonian gravity (`F = G*m1*m2/r^2`) rather than following fixed orbits.
- `ThreeBody.cs` — a three-body gravitational simulation exploring the classic three-body problem's chaotic, non-repeating orbits.
- `BoidsHandler.cs` — a flocking simulation combining boids rules (cohesion, alignment, avoidance) with gravitational attraction toward a center point, with an audio cue (`Crow Sound Effect cut.mp3`) tied to the flock.
- `SelfDestruct.cs` — a small utility that destroys a GameObject after a set delay.

## Tech

Unity, C#.

## Project structure

The Unity project lives in `327 Work/`. Open that folder in Unity Hub to load the project.
