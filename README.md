# TanksTDGame
2D Top-down shooter Game made with C# and Godot 4 Game Engine. \
>Inputs: W/A/S/D or Jostic Left Axis to move the main character & Mouse Left click or joystick "B" button to fire. \

Structure:
* src/scripts -> the main code for the game 
* assets -> contains the art
* Entity ->  contains the Godot Scenes for the entities for the game

Features:
* Player Movement and Attack with basic animation and projectile-based bullet system.
* Enemy AI with 2 States:
* *  Patrol the area if the Player is not in the specified range.
  *  if the player is in the attacking range, find a path to reach the enemy while attacking with projectile bullets at a specified fire rate

![image](https://github.com/Aangirasa/TanksTDGame/assets/31532976/c0b29bf2-f158-4358-93b3-9b53972d517b)

