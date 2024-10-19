## 🚗 About
Dive into Velocity Pursuit, where you race through dynamic tracks, challenge your limits in time trials, and engage in high-stakes pursuits.

## 🕹️ Installation

### 📁 Clone the Repository

1. Make sure you have Unity (version 2022.3.9f1 or later) installed on your machine.
2. Clone this repository:
   ```
   git clone https://github.com/Aaronmedhavi/Racing-Unity-Game.git
   ```
3. Open the project in Unity.
4. Open the game scene located in the "Assets/Scenes" folder.
5. Press the Play button in Unity Editor to start the game.

## 🎮 Controls

- Drive Forward: W / R2 / RT
- Reverse: S / L2(Hold) / LT(Hold)
- Steer Left: A / Left Stick Left
- Steer Right: D / Left Stick Right
- Brake: Spacebar / L2 / LT
- Gear Up: E / R1 / RB
- Gear Down: Q / L1 / LB
- Change Driving Mode: M / D-pad Up

## 📺 Gameplay Footage / Screenshot

## ⚙️ Mechanics
<h3>Netcode For GameObjects</h3>
<p align="justify">Experience online multiplayer experience made possible with Netcode. Through the use of a network manager, it allows players to join the game as a host or a client in a menu. The game will start when there is 2 players in the game, the ball will spawn once all the players have joined. The built in network manager only provide one slot for the player prefab but with the use of an index based on the client ID, it's now possible for players to play with distinct sprites.</p>

<h3>Post Processing</h3>
<p align="justify">Implementation of basic post processing which includes bloom and color grading to increase visual fidelity and enhance the player experience without sacrificing any performance.</p>

## 📚 Features and Script
- Engaging and smooth Driving
- Beautiful scenery
- Stunning visuals
- Responsive controls and challenging tracks

|  Script       | Description                                                  |
| ------------------- | ------------------------------------------------------------ |
| `CarController.cs` | Handles the car movement mechanics such as gear shift, driving mode, and gamepad controls. |
| `UIManager.cs`  | Manages various UI elements such as timer, speed, driving mode, and gear change. |
| `LapCount.cs`  | Lap counter for when everytime the car pass through the finish line. |
