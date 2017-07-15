# CooP AR - Interactive Collaboration Platform in Augmented Reality

This project was developed for a thesis at the Faculty of Engineering of the University of Porto. CooP AR is a system that enables asynchronous collaboration between two users (e.g. between a mentor and an apprentice). The following video demonstrates the system in action:

[![Demo](https://img.youtube.com/vi/xh1jzFGrfvU/0.jpg)](https://youtu.be/xh1jzFGrfvU "Interactive Collaboration Plaftorm in Augmented Reality demo")

This project is also described on [Behance](https://www.behance.net/gallery/53293979/Coop-AR-2017).

## Getting Started

These instructions will get you a copy of the project up and running on your local machine for development and testing purposes. See deployment for notes on how to deploy the project on a live system.

### Prerequisites

* [Unity](https://unity3d.com/)
* Android device supporting [Vuforia's system requirements](https://library.vuforia.com/articles/Solution/Vuforia-Supported-Versions)

### Installing

1. Open the project using Unity, or by clicking on "CooP AR/DefaultScene.unity".
2. To run the mentor's application, check that the "Application > Controller > Mentor Controller" node is enabled on the Unity editor hierarchy panel.
3. To run the apprentice's application, disable the "Mentor Controller" and enable the "Apprentice Controller".

## Deployment

In order to deploy the mentor's application:

1. Check that the "Application > Controller > Mentor Controller" node is enabled and that "Application > Controller > Apprentice Controller" is disabled on the Unity editor hierarchy panel.
2. Go to "File -> Build Settings" and select "PC, Mac & Linux Standalone" under "Platform".
3. Click on "Build And Run".

In order to deploy the apprentice's application:

1. Check that the "Application > Controller > Apprentice Controller" node is enabled and that "Application > Controller > Mentor Controller" is disabled on the Unity editor hierarchy panel.
2. Connect an Android device to your system.
2. Go to "File -> Build Settings" and select "Android" under "Platform".
3. Click on "Build And Run".

## Usage

1. Print the markers that are located at "CooP AR/Assets/Editor/QCAR/ImageTargetTextures/RUBIK_Database".
2. Download the [latest release](/releases/latest) or follow the instructions provided by the Installing and Deployment sections.
2. Open the mentor's application and prepare a new task.
3. Run the apprentice's application and enter the IP address of the device running the mentor's application.
4. Orient the apprentice's device in order to capture the marker associated with the selected task.
5. Use the apprentice's application to visualize augmented reality instructions and the mentor's application to create new ones.

## Built With

* [Unity](https://unity3d.com/) - Game engine
* [Vuforia](https://vuforia.com/) - Augmented reality framework

## Authors

* **João Maia** - *Developer* - [JPMMaia](https://github.com/JPMMaia)
* **Rui Nóbrega** - *Supervisor*
* **João Jacob** - *Co-supervisor*
* **Ana Alves** - *Designer* - [AnaFlipa](https://github.com/AnaFlipa)
