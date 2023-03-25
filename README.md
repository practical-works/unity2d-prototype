# 🎮 Prototype 

[![Unity](https://img.shields.io/badge/Unity-2022.1.10f1-blue?logo=unity)](https://github.com/topics/unity)
[![C#](https://img.shields.io/badge/C%23-9.0-blue?logo=c-sharp)](https://github.com/topics/csharp)

This is a sample **2D Platformer Game** unity project for learning and prototyping purposes.<br />
It features some basic 2D game stuff for unity.

![Screenshot0](./Screenshot0.gif)
![Screenshot1](./Screenshot1.gif)
![Screenshot2](./Screenshot2.gif)

## 🎯 Objectives

- [x] **🚀 Movement:**
    - [x] **🏃 Platformer Movement _using [Physics 2D](https://docs.unity3d.com/Manual/Physics2DReference.html)_:**
        - [x] Horizontal Movement.
        - [x] Vertical Movement (Jump).
    - [x] **🎞️ Movement Animations _using [Animator](https://docs.unity3d.com/Manual/AnimatorWindow.html) Parameters_.**
    - [x] **🎞️ Movement-Dust-Effects Animations _by [Instantiating Objects](https://docs.unity3d.com/ScriptReference/Object.Instantiate.html) and using [Animator](https://docs.unity3d.com/Manual/AnimatorWindow.html) States Names_.**

- [x] 🔌 **Mechanism:**
    - 🚪 **Switches and Doors Mechanism:**
        - [x] Switch for Multiple Doors.
        - [x] Door for Multiple Switches.
        - [x] Switch and Door Animations _by [Animating Transform Properties](https://docs.unity3d.com/Manual/animeditor-AnimatingAGameObject.html)_.

- [x] 🤺 **Combat:**
    - [x] **💥 Explosion Effect _using [Physics 2D](https://docs.unity3d.com/Manual/Physics2DReference.html)_.** 
    - [x] **🎞️ Damage Animations.**
    - [x] **🏹 Projectile Throwing _by [Instantiating Objects](https://docs.unity3d.com/ScriptReference/Object.Instantiate.html)_ and _using [Animator](https://docs.unity3d.com/Manual/AnimatorWindow.html) Parameters_.**

- [x] **🤖 Artificial Intelligence (AI):**
    - **🏃 Automated Platformer Movement:**
        - [x] Automated Horizontal Movement.
    - [x] **👁️ Detections using _[Physics 2D LineCast](https://docs.unity3d.com/ScriptReference/Physics2D.Linecast.html)_:**
        - [x] Wall Detection for Turning Round.
        - [x] Floor Detection for Gap Avoidance.
        - [x] Player Detection for Chase.
    - [x] **🏹 Automated Projectile Throwing.**


- [x] **🏕️ Environment:**
    - [x] **🗺️ Map:**
        - [x] Tile-Mapping _by [Nesting Objects](https://docs.unity3d.com/Manual/Hierarchy.html) (Classic)_.
        - [x] Tile-Mapping _using [TileMap Components](https://docs.unity3d.com/Manual/class-Tilemap.html)_.
    - [x] **🎥 Camera:**
        - [x] Player-Follower Camera.
        - [x] Smooth Camera Movement _using [Linear interpolation (Lerp)](https://en.wikipedia.org/wiki/Linear_interpolation)_.
        - [x] Pixel Perfect Camera _using [Pixel Perfect Camera Component](https://docs.unity3d.com/Packages/com.unity.2d.pixel-perfect@4.0/manual/index.html)_.
        - [x] Pixel Perfect Camera _using [Pixel Perfect Camera Component](https://docs.unity3d.com/Packages/com.unity.render-pipelines.universal@13.1/manual/2d-pixelperfect.html) of [Universal Render Pipeline (URP)](https://docs.unity3d.com/Packages/com.unity.render-pipelines.universal@13.1/manual/index.html)_.
    - [x] **💡 Light:**
        - [x] Lighting _using [Light Components](https://docs.unity3d.com/Manual/Lights.html) (3D)_.
        - [x] Lighting _using [Light 2D Components](https://docs.unity3d.com/Packages/com.unity.render-pipelines.universal@13.1/manual/Lights-2D-intro.html) of [Universal Render Pipeline (URP)](https://docs.unity3d.com/Packages/com.unity.render-pipelines.universal@13.1/manual/index.html)_.
    - [x] **📣 Audio _using [Audio Source Component](https://docs.unity3d.com/Manual/class-AudioSource.html)_:**
        - [x] Background Music (BGM) with volume, pitch, and **looping** features _using [Audio Source Play](https://docs.unity3d.com/ScriptReference/AudioSource.Play.html)_.
        - [x] Sound Effects (SFX) with volume, pitch, and **overlapping** features _using [Audio Source Play One Shot](https://docs.unity3d.com/ScriptReference/AudioSource.PlayOneShot.html)_.
        - [x] Distance-Relative Audio _using 3D Sound Settings of [Audio Source Component](https://docs.unity3d.com/Manual/class-AudioSource.html)_.

- [ ] **🔲 User Interface (UI):**
    - [ ] **💯 Head-Up Display (HUD):**
        - [ ] Health Bar.
        - [ ] Score Counter.
    - [ ] **🖼️ Title Menu:**
        - [ ] 🚧 ...
        - [ ] 🚧 ...
        - [ ] 🚧 ...
    - [ ] **⚙️ Settings Menu :**
        - [ ] 🚧 ...
        - [ ] 🚧 ...
        - [ ] 🚧 ...

## 🏭 Environment

- Runtime: **[Windows](https://www.microsoft.com/en-us/windows) 10**
- Game Engine: **[Unity](https://unity.com) 2022**
- Scripts Language: **[C#](https://github.com/dotnet/csharplang) 9.0**
- Scripts Editor: **[Visual Studio](https://visualstudio.microsoft.com) 2022**

## 📚 Learning Resources

- 📕 [Unity Documentation](https://docs.unity.com)
- 📼 [Game Dev Beginner](https://www.youtube.com/@GameDevBeginner/videos)
- 📼 [Unity 4 2D Essential Training](https://www.linkedin.com/learning/unity-4-2d-essential-training)

## 📄 License
[MIT](./LICENSE)