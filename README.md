# VegasLipSyncApp
**A MAGIX Vegas Pro extension for simple lip sync animation**
## About
The LipSync extension for Vegas Pro allows you to quickly and easily animate character facial animations that sync up to your audio files. It's similar to animation methods used in Adobe After Effects with the added benefit of being contained in the powerful, non-linear editing software Vegas Pro.
## Setup
Download the LipSyncApp.dll from the latest [release](https://github.com/thembats/VegasLipSyncApp/releases) and place it in `C:\Program Files\VEGAS\VEGAS Pro <version>\Application Extensions` folder. If the `Application Extensions` folder doesn't exist, create it.

To use the extension, start Vegas and in the toolbar navigate to `Tools > Extensions > LipSyncApp`.
## Usage
Start by adding your images to the image boxes by clicking the `...` buttons and navigating to your images.

After selecting your images, right click the main Vegas timeline and insert a video track. Click the `+` button underneath your desired image to add it to the video track. The image will be added to the track at the point where the timeline cursor is at.

* _Note: The LipSyncApp automatically puts images in the first video track of Vegas. Reserve this track only for your mouth animations._

* _Note: If you try to add an image without first inserting a video track, you will receive an error._

A faster way to add images to the video track is to hold the `Ctrl` key and scroll up or down over the LipSyncApp window. This will cycle through the images you've added to the app and once you release `Ctrl` the image will be placed on the track.

* _Note: Due to the way Vegas implemented extension scripting and Windows Forms, you must hover over the LipSyncApp and scroll to change mouth frames._

To aid in your animating, add an audio file you would like to animate over to an audio track. Hold `Ctrl` and scrub through the audio track using the timeline cursor. This will allow you to hear your audio and decide what image to add.

If you want to make changes to previously added images, drag the timeline cursor to wherever you want to add a new image and add it. The LipSyncApp automatically removes part of the previous image and inserts the newly selected one.

You can also add the body of your character to a track beneath your mouth animation track and position all of the images in the mouth track over your character. To do this, click the hamburger button to the left of the track timeline on the first track and click `Track Motion`. Here you can change where your mouths show up and position them over your character.



