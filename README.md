# Inworld Lite
Inworld lite is a Unity library that uses the [inworld-api](https://github.com/yolanother/inworldapi) to create a very lightweight wrapper around inworld functionality.

## Setup
1. Import UnityInworldLight via package manager. You can add it by adding https://github.com/yolanother/UnityInworldLight.git in the add git package dropdown. <img width="277" alt="image" src="https://user-images.githubusercontent.com/645359/206382972-64b1d7ed-1311-4e92-90a8-fbabef5ab5b3.png">
2. Create an inworld server config and set it up to point to your node.js server that is running [inworld-api](https://github.com/yolanother/inworldapi). <img width="627" alt="image" src="https://user-images.githubusercontent.com/645359/206383060-50c3e89c-157d-4748-bff6-ffbb8bfb0e70.png"><img width="364" alt="image" src="https://user-images.githubusercontent.com/645359/206383370-8169c21b-c0b3-441e-9700-63a7d923fda5.png">
3. Depending on the version of Unity you are using you may get an exception about missing Newtonsoft Json. You can fix this by adding com.unity.nuget.newtonsoft-json to your package dependencies via "Add Package by Name".

3. Create an Inworld integration config and enter your integration key and secret <img width="670" alt="image" src="https://user-images.githubusercontent.com/645359/206383181-c22e1514-6916-49af-a57c-20778010fb97.png"> <img width="368" alt="image" src="https://user-images.githubusercontent.com/645359/206383261-911e7bbb-fe92-4b54-bd21-281a7664c34e.png">
4. Create an interaction path either Character or Scene. <img width="705" alt="image" src="https://user-images.githubusercontent.com/645359/206382848-5b8a54eb-ee98-471d-a99b-9d45f8442d6e.png">
5. Add a game object to your scene that you will use to send text requests through.
6. Add an InworldInteraction script to that game object and set its configuration SOs.
![image](https://user-images.githubusercontent.com/645359/206382252-70107c23-e956-4016-9093-69ccbbbe5d74.png)
7. Hook up events for handling callbacks from the interactions
