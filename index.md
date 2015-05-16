---
layout: default
---

# VRBar

Team:

* Cole Gleason (cagleas2)
* Clare Curtis (cfcurti2)
* Austin Walters (awalte9)

For our CS 498SL (Virtual Reality) final project, we wanted to make a
virtual bar. Many of our favorite places on campus are the bars on
Green St., and after we graduate we felt it would be hard for us all
to get together. Why not get together over the internet over a few
drinks?

Our project had two main components. First, we wanted to be able to
track a beer mug in the physical world and have it move in the virtual
world. Second, we wanted the virtual world to be a social, multiplayer
environment with voice chat.

## Tracking

TODO: Austin's text goes here.

## Social environment

For the social aspect, we chose to use [VRChat](http://vrchat.net), a universe of
connected virtual environments. It provided us with voice chat and
avatars, which was nice, while also taking care of the networking. We
used their proprietary SDK to integrate the Pub scene and assets into
a download-able level.

We ran into a few hiccups, unfortunately. The SDK does not currently
give us a reference to the Player object, so we couldn't add tracking
with reference to the player like we intended. Additionally, the SDK
does not support threading and networking properly, so the tracking
could not be added at all. We have communicated this need to the
developers, however, so they added that request to their backlog.

To build the VRChat level, do the following:

1. Download the VRBar code and open `BarScene/DemoScene.unity`.
2. Select the "Root" object.
3. In the toolbar, click VRChat -> Build Custom Scene From Selection.
4. Save the file and open it in VRChat by creating a custom room.

Just want to join the room directly? Open up VRChat and join the room
called "Sports Pub".
