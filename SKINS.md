# MUNIA skinning

Skins for MUNIA are defined as SVG files with some custom attributes mixed in to identify and animate various controller parts.
Benefits of using SVG files include perfect scaling, self-contained files and ability to use high quality image editing software such as Adobe Illustrator or [Inkscape](https://inkscape.org/).


## Process overview
1. *Find base image:* when adopting a new skin one typically begins by finding an existing image of the desired controller. For many popular controllers well-built SVG files may already exist. Should this not be the case then tracing a high quality image of said controller is a viable alternative.

2. *Add highlights:* button presses are typically shown by changing the color of said button, but MUNIA allows a lot of freedom in this aspect. Per button you normally define 2 object groups, one for the non-pressed and one for the pressed state of this button. They will typically overlap exactly, but this is not mandatory. The MUNIA software will show only one of both states at a time.

3. *Add metadata:* the last step is to add some XML attributes to the resulting .svg file. These will allow the MUNIA software to identify the various parts of the controller, and thereby animate them in the desired way. Full details on the possible attributes to specify are detailed in the sections below.


## Skin engine
The MUNIA skinning engine initially loads the SVG file, identifies the various parts in it, and caches the drawing of most individual elements. The controller with all button, stick and trigger elements removed is used as base image. All elements are continuously rendered against this base image.

### Layering
The controller is rendered as a layered image. The base image is fixed at layer '0'. Individual elements can be placed 'behind' or 'in front' of the base by specifying the `munia:z-order=x` attribute on it, where `x` can be any integer. Elements placed at layer x<0 are placed behind the base image in order of incrementing `x`. Similarly elements with positive z-order are drawn on top of the base image, again in order of incrementing `x`.


## Skin elements
Game controllers typically contain 3 kinds of elements that require some form of animation: buttons, triggers and sticks. We also need one XML element to specify what kind of controller is defined in this file. The following sections outline how to model each of them in MUNIA skins.

### Info metadata
The root `svg` element in the resulting document should contain a `munia:info` element specifying a name for the skin and the device type it is compatible with. 
Example, taken from `gc.svg` that ships with the MUNIA software:
``` xml
<munia:info munia:skin-name="Gamecube" munia:device-type="NGC"/>
```
Supported device types at the time of writing are:
* NGC
* SNES
* N64


### Buttons/dpad
Buttons and d-pad directions are considered identical in the MUNIA software. This simplifies skinning them. These are the relevant XML attributes:
* `munia:button-id="X"`: this specifies which button links to this image element. Refer to the final section in this document for hints on finding the correct id.
* `munia:button-state="pressed/released"`: this indicates whether this element is supposed to be visible in the pressed or released state. Effectively the software replaces the image group with `munia:button-state="released"` for the one with `button-state="pressed"` group if both have matching `munia:button-id` attributes. If you specify only one of both, the other is simply never drawn.
* `munia:z-order="x `: see Layering section above

#### Example
Taken from the SNES skin `snes-light-blue.svg` skin that ships with the software:
``` xml
<circle id="A" munia:button-id="7" munia:button-state="released" cx="380" cy="125" r="25" style="fill:#4d4d4d"/>
<circle id="A-pressed" munia:button-id="7" munia:button-state="pressed" cx="380" cy="125" r="25" style="fill:#0000ff"/>
```


### Sticks
Sticks refer to the control sticks that made their appearance in the 5th generation of videogame consoles. They are typically represented as a graphic that moves its vertical and horizontal position relative to a resting point. The MUNIA software implements to following attributes for specifying how these are to be rendered:

* `munia:stick-id="id"`: indicates that this element is to be considered a stick, and assigns an id to it. The id should be any unique integer number.
* `munia:axis-h="h" munia:axis-v="v": id's of the horizontal and vertical movement axes respectively. Refer to last section.
* `munia:offset-scale="f.f"`: a real/floating-point number indicating how far the stick moves away from its center. Good values are easily determined through trial and error. Typically a number between 0.5-2.0 should work well.
* `munia:z-order="x"`: see Layering section above

#### Example
Taken from the Gamecube skin `gc.svg` skin that ships with the software:

``` xml
<g id="Control Stick" munia:axis-h="0" munia:axis-v="1" munia:offset-scale="1.8" transform="translate(8.6180222,-28.433458)"> ... </g>
```

### Triggers
The MUNIA software can render controller triggers as either a bar that fills up, or as an element that moves in certain direction.

* `munia:trigger-id="id"`: indicates that this element is to be considered a trigger, and assigns an id to it. The id should be any unique integer number.
* `munia:trigger-axis="x"`: id of this trigger axis on the controller. Refer to last section.
* `munia:trigger-type="bar/slide"`: whether this is rendered as a bar or slide. Bars are made partially visible, and slide-type triggers move some amount in one direction, depending on how far the trigger is pressed. 
* `munia:trigger-mirror="yes/no"`: only applies to bar type triggers. When non-mirrored (default), bars fill from left-to-right or top-to-bottom. Specifying `mirror="yes"` reverses that.
* `munia:trigger-orientation="vertical/horizontal"`: whether the bar/slide is filled/moved in vertical or horizontal direction.

* `munia:offset-scale="f.f"`: applies only to *slide*-type triggers. Similar to the offset-scale attribute for sticks.
* `munia:trigger-range="a-b"`: specifies the deadzone. Defaults to '0-255' but can be limited, e.g. to '30-240' for controllers with imperfect range.
* `munia:z-order="x"`: see Layering section above


### Finding id's
One can easily find the id's of buttons and axes of a controller using the [http://html5gamepad.com](HTML5 Gamepad Tester).
Hats are mapped after the buttons, in order up/down/left/right. So if the controller has 6 face buttons and a d-pad, then the last face button has `button-id=5` and the left-direction on the d-pad is assigned `button-id=8`.
