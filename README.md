# Kentico MVC Focuspoint tool
[![MIT](https://camo.githubusercontent.com/52ec9e2dfec7264e254fb7af5ac87f301ced9180/68747470733a2f2f696d672e736869656c64732e696f2f707970692f6c2f417270656767696f2e737667)](https://raw.githubusercontent.com/hyperium/hyper/master/LICENSE)

Focuspoint provides helper tool for Kentico CMS and handle image presentation, by ensuring the 'spare' parts of your image (negative space) are cropped out before the important parts.

## Requirements
* **Kentico 12.0.77** or later version is required to use this component.

## Download & instalation
1. Download and install form control package:
    * Download and install Kentico export package from this [link](https://github.com/drilic/kentico-mvcformcontrol-focuspoint/blob/master/EXLRT.Kentico.CMS.FormControls.Focuspoint.1.0.0.zip).
      * Import package **EXLRT.Kentico.CMS.FormControls.Focuspoint.1.0.0.zip** through Kentico Admin Portal.
      * Make sure that checkbox 'Import code files' is checked.
      * After import, navigate through the newly imported files and include them into project:
        * \CMSModules\Content\FormControls\EXLRT\ImageUrlFocusPoint
        * \CMSScripts\EXLRT\ImageUrlFocusPointSelector
    * Download and instal MVC site extension:
      * Download nuget package from this [link](https://github.com/drilic/kentico-mvcformcontrol-focuspoint/blob/master/EXLRT.Kentico.Mvc.FormControls.Focuspoint.1.0.0.nupkg).
      * Setup local nugetfeed and copy nuget package **EXLRT.Kentico.Mvc.FormControls.Focuspoint.1.0.0.nupkg** to newly created feed.
      * Install package to your MVC solution.
2. [Assign 'Image with focuspoint' form control to text field on page type](#using-focuspoint-component)
3. [Register necessary styles and scripts for full calendar in layout](#register-layout-scripts)
4. [Prerequisites for HTML styles](#prerequisites-html-styles)
5. [Render image with focuspoint extension](#focuspoint-extension)

### Using focuspoint component

After configuring page type field to use 'Image with focuspoint' form control and selecting image, control will render the image bellow which will provide you some kind of the preview where you will be able to select the focus point by clicking on the image itself.

![Image of focuspoint control](https://github.com/drilic/kentico-mvcformcontrol-focuspoint/blob/master/focuspoint-helper-tool.png)

### Prerequisites HTML styles

Focus point prerequisite is to already have defined containers in which image will be cropped. That is pretty much common flow in the design which will prevent content jumping around after image is loaded:
```html
<style>
	// sets the height of the image container
        .focuspoint-testing {
            height: 380px;
        }

	// hide image inside container to prevent image to be loaded before calculation was finished
        .focuspoint-testing img {
            display: none;
        }
</style>
```



### Register layout scripts

Example of configuration for DancingGoat site (_Layout.cshtml):
```csharp
@using EXLRT.Kentico.Mvc.FormControls.Focuspoint.Extensions;
...
<head>
	...
	 @Html.RenderFocuspointStyles()
</head>
<body>
	...
	@Html.RenderFocuspointSripts();
</body>

```

### Focuspoint extension

After all is done, you can just pass the value that you get from the CMS side for the image into the FocuspointImage extension:
```csharp
@Html.FocuspointImage(Model.BannerImageUrl, "focuspoint-testing")
```
Extension above will render the HTML with all necessary parameters for focuspoint to work properly. Also, there is a few extra parameters which will allow you to customize the classes, alt and title text and much more.
```html
<div class="focuspoint-testing js-focuspoint" data-focus-x="0.640859375" data-focus-y="-0.2651646447140381">
     <img src="/DancingGoatMvc/media/CoffeeGallery/hompage-turkish-airlines-case-study.jpg" style="top: -209.474%; left: 0px; display: inline;">
</div>
```

## Contributions and Support
Feel free to fork and submit pull requests or report issues to contribute. Either this way or another one, we will look into them as soon as possible. 
